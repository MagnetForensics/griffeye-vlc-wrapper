using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VlcWrapper.Models;
using LibVLCSharp.Shared;
using Microsoft.Extensions.Logging;
using SSG.LocalFileStreamClient;

namespace Griffeye.VlcWrapper.MediaPlayer
{
    internal sealed class VLCMediaPlayer : IMediaPlayer, IDisposable
    {
        private readonly LibVLC library;
        private readonly Client localFileStreamClient;
        private readonly LibVLCSharp.Shared.MediaPlayer mediaPlayer;
        private readonly ILogger<VLCMediaPlayer> logger;
        private Stream currStream;
        private StreamMediaInput streamMediaInput;
        private int? audioTrackId;
        private bool aspectRationSet;

        public event EventHandler<EventArgs> EndReached;
        public event EventHandler<MediaPlayerTimeChangedEventArgs> TimeChanged;
        public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;
        public event EventHandler<float> AspectRatioChanged;

        public VLCMediaPlayer(InputData inputData, ILogger<VLCMediaPlayer> logger)
        {
            this.logger = logger;
            Core.Initialize();
            localFileStreamClient = new Client();
            library = new LibVLC(new[]
                {
                    "--no-video-title-show",            /* no filename displayed          */
                    "--no-stats",                       /* no stats                       */
                    "--no-sub-autodetect-file",         /* we don't want subtitles        */
                    "--no-snapshot-preview",
                    "--intf", "dummy",
                    "--no-spu",
                    "--no-osd",
                    "--no-lua",
                    // debug options below
                    "--quiet-synchro",
                    "-vvv"
                });
            library.Log += Library_Log;
            mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(library)
            {
                Hwnd = new IntPtr(inputData.Handle),
                EnableKeyInput = false,
                EnableMouseInput = false
            };

            mediaPlayer.EndReached += (sender, args) => EndReached?.Invoke(this, args);
            mediaPlayer.TimeChanged += (sender, args) =>
            {
                TimeChanged?.Invoke(this, args);
                if (!aspectRationSet && mediaPlayer.VideoTrack != -1 && mediaPlayer.Media.Tracks[mediaPlayer.VideoTrack].Data.Video.Height != 0)
                {
                    aspectRationSet = true;
                    var videoData = mediaPlayer.Media.Tracks[mediaPlayer.VideoTrack].Data.Video;
                    var ar = (float)videoData.Width / videoData.Height;
                    if (videoData.SarDen != 0)
                    {
                        ar *= (float)videoData.SarNum / videoData.SarDen;
                    }
                    AspectRatioChanged?.Invoke(this, ar);
                }
            };
            mediaPlayer.LengthChanged += (sender, args) => LengthChanged?.Invoke(this, args);

        }

        private void Library_Log(object sender, LogEventArgs e)
        {
            logger.LogDebug($"{e.Module} {e.Level} {e.Message}");
        }

        public void ConnectLocalFileStream(string pipeName)
        {
            localFileStreamClient.Connect(pipeName);
        }

        public void DisconnectLocalFileStream() { localFileStreamClient.Disconnect(); }

        public void Play()
        {
            if (mediaPlayer.State == VLCState.Ended)
            {
                mediaPlayer.Stop();
            }

            mediaPlayer.Play();
        }

        public void Pause() { mediaPlayer.SetPause(true); }

        public void Seek(float position)
        {
            if (mediaPlayer.State == VLCState.Ended)
            {
                mediaPlayer.Stop();
            }

            var isPaused = mediaPlayer.State == VLCState.Paused;

            // Play need to be called on the media before a seek can be done
            if (!mediaPlayer.IsPlaying)
            {
                mediaPlayer.Play();
                Thread.Sleep(100);
            }

            mediaPlayer.Position = position;
            if (isPaused) mediaPlayer.SetPause(true);
        }

        public void LoadMedia(StreamType type, string file)
        {
            // reset if not muted
            if (audioTrackId != -1) { audioTrackId = null; }
            // recalculate aspectratio
            aspectRationSet = false;

            if (type == StreamType.LocalFileStream)
            {

                streamMediaInput?.Dispose();
                currStream?.Dispose();
                currStream = localFileStreamClient.OpenStream(file);
                streamMediaInput = new StreamMediaInput(currStream);

                using var media = new Media(library, streamMediaInput);
                mediaPlayer.Media = media;
            }
            else
            {
                using var media = new Media(library, file);
                mediaPlayer.Media = media;
            }
        }

        public void SetPlaybackSpeed(float speed) { mediaPlayer.SetRate(speed); }

        public void SetVolume(int volume) { mediaPlayer.Volume = volume; }

        public void SetMute(bool mute)
        {
            mediaPlayer.Mute = mute;
        }

        public bool CreateSnapshot(int numberOfVideoOutput, int width, int height, string filePath)
        {
            var currentVideoTrack = mediaPlayer.VideoTrack;

            if (currentVideoTrack == -1)
            {
                return false;
            }
            
            mediaPlayer.TakeSnapshot((uint)numberOfVideoOutput, filePath, (uint)width, (uint)height);

            return true;
        }

        public List<(int, string)> GetAudioTracks()
        {
            var audioTrackDescription = mediaPlayer.AudioTrackDescription;

            return audioTrackDescription.Select(description => (description.Id, description.Name)).ToList();
        }

        public List<(int, string)> GetVideoTracks()
        {
            var videoTrackDescription = mediaPlayer.VideoTrackDescription;

            return videoTrackDescription.Select(description => (description.Id, description.Name)).ToList();
        }

        public void SetAudioTrack(int trackId)
        {
            mediaPlayer.SetAudioTrack(trackId);
        }

        public void SetVideoTrack(int trackId)
        {
            mediaPlayer.SetVideoTrack(trackId);
        }

        public void StepForward()
        {
            mediaPlayer.NextFrame();
        }

        public void StepBack()
        {
            mediaPlayer.SetPause(true);
            var time = Math.Max(
                Math.Min(mediaPlayer.Time - (long)Math.Ceiling(1000d / mediaPlayer.Fps), mediaPlayer.Length), 0);

            mediaPlayer.Time = time;
        }

        public void Dispose()
        {
            streamMediaInput?.Dispose();
            currStream?.Dispose();
            mediaPlayer.Dispose();
            library.Dispose();
            localFileStreamClient.Dispose();
        }
    }
}