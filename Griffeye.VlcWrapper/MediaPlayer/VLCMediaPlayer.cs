using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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

        private bool aspectRationSet;
        private float startPosition;
        private float stopPosition;

        public event EventHandler<EventArgs> EndReached;
        public event EventHandler<long> TimeChanged;
        public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;
        public event EventHandler<VideoInfo> VideoInfoChanged;
        public event EventHandler<EventArgs> Playing;
        public event EventHandler<EventArgs> Paused;
        public event EventHandler<MediaPlayerVolumeChangedEventArgs> VolumeChanged;
        public event EventHandler<EventArgs> Unmuted;
        public event EventHandler<EventArgs> Muted;

        public VLCMediaPlayer(InputData inputData, ILogger<VLCMediaPlayer> logger)
        {
            this.logger = logger;
            Core.Initialize();
            localFileStreamClient = new Client();
            library = new LibVLC(
                "--no-video-title-show",
                "--no-stats",
                "--no-sub-autodetect-file",
                "--no-snapshot-preview",
                "--intf",
                "dummy",
                "--no-spu",
                "--no-osd",
                "--no-lua",
                "--quiet-synchro",
                "-v");
            library.Log += Library_Log;
            mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(library)
            {
                Hwnd = new IntPtr(inputData.Handle),
                EnableKeyInput = false,
                EnableMouseInput = false
            };
            mediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            mediaPlayer.EndReached += (sender, args) =>
            {
                logger.LogDebug("End reached");
                EndReached?.Invoke(this, args);
            };
            mediaPlayer.TimeChanged += (sender, args) =>
            {
                logger.LogDebug($"time = {args.Time}");
                time = args.Time;
                TimeChanged?.Invoke(this, args.Time);

                if (aspectRationSet || mediaPlayer.VideoTrack == -1) { return; }

                aspectRationSet = true;

                uint x = 0;
                uint y = 0;
                float aspectRatio = 1;

                if (!mediaPlayer.Size(0, ref x, ref y)) { return; }

                var videoTrack = mediaPlayer.Media.Tracks.FirstOrDefault(track => track.TrackType == TrackType.Video);
                var orientation = videoTrack.Data.Video.Orientation;

                if (IsFlipped(orientation))
                {
                    aspectRatio = (float)y / x;
                }
                else
                {
                    aspectRatio = (float)x / y;
                }

                if (videoTrack.Data.Video.SarDen != 0)
                {
                    aspectRatio *= (float)videoTrack.Data.Video.SarNum / videoTrack.Data.Video.SarDen;
                }

                VideoInfoChanged?.Invoke(this, new VideoInfo
                {
                    VideoOrientation = orientation.ToString(),
                    AspectRatio = aspectRatio
                });
            };

            mediaPlayer.LengthChanged += (sender, args) => { if (args.Length > 0) { LengthChanged?.Invoke(this, args); } };
            mediaPlayer.Playing += (sender, args) => Playing?.Invoke(this, args);
            mediaPlayer.Paused += (sender, args) => Paused?.Invoke(this, args);
            mediaPlayer.VolumeChanged += (sender, args) => VolumeChanged?.Invoke(this, args);
            mediaPlayer.Unmuted += (sender, args) => Unmuted?.Invoke(this, args);
            mediaPlayer.Muted += (sender, args) => Muted?.Invoke(this, args);
        }

        private void MediaPlayer_PositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            logger.LogDebug($"Position = {e.Position}");
            if (e.Position >= stopPosition)
            {
                logger.LogDebug($"pausing, e.Position = {e.Position}, stopPosition = {stopPosition}");
                Task.Run(() => { mediaPlayer.SetPause(true); });
            }
        }

        private static bool IsFlipped(VideoOrientation orientation)
        {
            return orientation == VideoOrientation.RightTop ||
                   orientation == VideoOrientation.LeftBottom ||
                   orientation == VideoOrientation.RightBottom;
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

        public void Play() { mediaPlayer.Play(); }

        public void Pause() { mediaPlayer.SetPause(true); }

        public void Seek(float position)
        {
            var allowedPosition = Math.Max(Math.Min(position, stopPosition), startPosition);

            if (mediaPlayer.State == VLCState.Ended)
            {
                mediaPlayer.Stop();
                PlayUntillBuffered();
            }
            mediaPlayer.Position = allowedPosition;
            time = mediaPlayer.Time;
            TimeChanged?.Invoke(this, time);
        }

        public void LoadMedia(StreamType type, string file, float startPosition, float stopPosition)
        {
            this.startPosition = startPosition;
            this.stopPosition = stopPosition;

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
            PlayUntillBuffered();
            mediaPlayer.Position = startPosition;
        }

        private void PlayUntillBuffered()
        {
            using var mre = new ManualResetEvent(false);
            mediaPlayer.Buffering += mediaPlayerOnBuffering;
            mediaPlayer.Play();
            mre.WaitOne(TimeSpan.FromSeconds(5));
            mediaPlayer.Buffering -= mediaPlayerOnBuffering;
            mediaPlayer.SetPause(true);

            void mediaPlayerOnBuffering(object sender, MediaPlayerBufferingEventArgs args)
            {
                if (args.Cache >= 100) { mre.Set(); }
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
            using var mre = new ManualResetEventSlim();
            mediaPlayer.SnapshotTaken += MediaPlayerOnSnapshotTaken;
            mediaPlayer.TakeSnapshot((uint)numberOfVideoOutput, filePath, (uint)width, (uint)height);
            mre.Wait();
            mediaPlayer.SnapshotTaken -= MediaPlayerOnSnapshotTaken;

            void MediaPlayerOnSnapshotTaken(object sender, MediaPlayerSnapshotTakenEventArgs e)
            {
                mre.Set();
            }

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
        private long time;
        public void StepForward()
        {
            mediaPlayer.NextFrame();

            time += (long)(1000 / mediaPlayer.Fps);
            TimeChanged?.Invoke(this, time);
        }

        public void StepBack()
        {
            mediaPlayer.SetPause(true);

            time -= (long)(1000 / mediaPlayer.Fps);
            Seek((float)time / mediaPlayer.Length);
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