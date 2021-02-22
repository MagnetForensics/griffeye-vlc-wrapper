using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.Messages.Events;
using Griffeye.VlcWrapper.Models;
using Griffeye.VlcWrapper.Services;
using LibVLCSharp.Shared;
using Microsoft.Extensions.Logging;
using SSG.LocalFileStreamClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Griffeye.VlcWrapper.MediaPlayer
{
    internal sealed class VLCMediaPlayer : IMediaPlayer, IDisposable
    {
        private readonly LibVLC library;
        private readonly Client localFileStreamClient;
        private readonly LibVLCSharp.Shared.MediaPlayer mediaPlayer;
        private readonly ILogger<VLCMediaPlayer> logger;
        private readonly IMediaTrackService mediaTrackService;
        private Stream currentStream;
        private StreamMediaInput streamMediaInput;
        private bool aspectRationSet;
        private float startPosition;
        private float stopPosition;
        private long time;

        public event EventHandler<EventArgs> EndReached;
        public event EventHandler<long> TimeChanged;
        public event EventHandler<float> PositionChanged;
        public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;
        public event EventHandler<VideoInfo> VideoInfoChanged;
        public event EventHandler<EventArgs> Playing;
        public event EventHandler<EventArgs> Paused;
        public event EventHandler<MediaPlayerVolumeChangedEventArgs> VolumeChanged;
        public event EventHandler<EventArgs> Unmuted;
        public event EventHandler<EventArgs> Muted;
        public event EventHandler<MediaTrackChangedEvent> MediaTracksChanged;

        private readonly List<string> vlcArguments = new List<string>
        {
            "--no-video-title-show",
            "--no-stats",
            "--no-sub-autodetect-file",
            "--no-snapshot-preview",
            "--intf",
            "dummy",
            "--no-spu",
            "--no-osd",
            "--no-lua",
            "--quiet-synchro"
        };

        public VLCMediaPlayer(InputData inputData, ILogger<VLCMediaPlayer> logger, IMediaTrackService mediaTrackService)
        {
            this.logger = logger;
            this.mediaTrackService = mediaTrackService;
            Core.Initialize();
            localFileStreamClient = new Client();       

            if (inputData.AttachDebugger)
            {
                vlcArguments.Add("--verbose=2");
            }
            else
            {
                vlcArguments.Add("--verbose=0");
            }

            library = new LibVLC(vlcArguments.ToArray());
            library.Log += Library_Log;
            mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(library)
            {
                Hwnd = new IntPtr(inputData.Handle),
                EnableKeyInput = false,
                EnableMouseInput = false,
                EnableHardwareDecoding = true,
            };
            mediaPlayer.PositionChanged += HandlePositionChanged;
            mediaPlayer.EndReached += (sender, args) => EndReached?.Invoke(this, args);
            mediaPlayer.TimeChanged += (sender, args) =>
            {
                time = args.Time;
                TimeChanged?.Invoke(this, args.Time);
                SendVideoInformation();
            };

            mediaPlayer.LengthChanged += (sender, args) => { if (args.Length > 0) { LengthChanged?.Invoke(this, args); } };
            mediaPlayer.Playing += (sender, args) => Playing?.Invoke(this, args);
            mediaPlayer.Paused += (sender, args) => Paused?.Invoke(this, args);
            mediaPlayer.VolumeChanged += (sender, args) => VolumeChanged?.Invoke(this, args);
            mediaPlayer.Unmuted += (sender, args) => Unmuted?.Invoke(this, args);
            mediaPlayer.Muted += (sender, args) => Muted?.Invoke(this, args);
        }

        private void SendVideoInformation()
        {
            if (aspectRationSet || mediaPlayer.VideoTrack == -1) return;

            aspectRationSet = true;

            uint x = 0;
            uint y = 0;
            float aspectRatio = 1;

            if (!mediaPlayer.Size(0, ref x, ref y)) return;

            var videoTrack = mediaPlayer.Media.Tracks.FirstOrDefault(track => track.TrackType == LibVLCSharp.Shared.TrackType.Video);
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

            VideoInfoChanged?.Invoke(this, new VideoInfo { VideoOrientation = orientation.ToString(), AspectRatio = aspectRatio });
        }

        private void HandlePositionChanged(object sender, MediaPlayerPositionChangedEventArgs e)
        {
            if (e.Position >= stopPosition) { Task.Run(() => mediaPlayer.SetPause(true)); }

            PositionChanged?.Invoke(this, e.Position);
        }

        private static bool IsFlipped(VideoOrientation orientation)
        {
            return orientation == VideoOrientation.RightTop ||
                   orientation == VideoOrientation.LeftBottom ||
                   orientation == VideoOrientation.RightBottom;
        }

        private void Library_Log(object sender, LogEventArgs e) => logger.LogDebug("{Module} {Level} {Message}", e.Module, e.Level, e.Message);

        public void ConnectLocalFileStream(string pipeName) => localFileStreamClient.Connect(pipeName);

        public void DisconnectLocalFileStream() => localFileStreamClient.Disconnect();

        public void Play() => mediaPlayer.Play();

        public void Pause() => mediaPlayer.SetPause(true);

        public void Seek(float position)
        {
            var allowedPosition = Math.Max(Math.Min(position, stopPosition), startPosition);

            if (mediaPlayer.State == VLCState.Ended)
            {
                mediaPlayer.Stop();
                PlayUntillBuffered();
                Task.Run(async () => MediaTracksChanged?.Invoke(this, new MediaTrackChangedEvent(await mediaTrackService.GetTrackInformation(mediaPlayer))));
            }

            mediaPlayer.Position = allowedPosition;
            time = mediaPlayer.Time;
            TimeChanged?.Invoke(this, time);
        }

        public async Task LoadMedia(StreamType type, string file, float startPosition, float stopPosition)
        {
            this.startPosition = startPosition;
            this.stopPosition = stopPosition;
            aspectRationSet = false;

            // Vlc crashes if image options are enabled when changing video.            
            EnableImageOptions(false);

            if (type == StreamType.LocalFileStream)
            {
                streamMediaInput?.Dispose();
                currentStream?.Dispose();
                currentStream = localFileStreamClient.OpenStream(file);
                streamMediaInput = new StreamMediaInput(currentStream);

                using var media = new Media(library, streamMediaInput);
                mediaPlayer.Media = media;
            }
            else
            {
                using var media = new Media(library, file);
                mediaPlayer.Media = media;
            }

            await mediaPlayer.Media.Parse(MediaParseOptions.ParseLocal);
            MediaTracksChanged?.Invoke(this, new MediaTrackChangedEvent(await mediaTrackService.GetTrackInformation(mediaPlayer)));
            mediaPlayer.Position = startPosition;
        }

        private void PlayUntillBuffered()
        {
            using var manualResetEvent = new ManualResetEvent(false);

            mediaPlayer.Buffering += mediaPlayerOnBuffering;
            mediaPlayer.Play();
            manualResetEvent.WaitOne(TimeSpan.FromSeconds(5));
            mediaPlayer.Buffering -= mediaPlayerOnBuffering;
            mediaPlayer.SetPause(true);

            void mediaPlayerOnBuffering(object sender, MediaPlayerBufferingEventArgs args)
            {
                if (args.Cache >= 100) { manualResetEvent.Set(); }
            }
        }

        public void SetPlaybackSpeed(float speed) => mediaPlayer.SetRate(speed);

        public void SetVolume(int volume) => mediaPlayer.Volume = volume;

        public void SetMute(bool mute) => mediaPlayer.Mute = mute;

        public bool CreateSnapshot(int numberOfVideoOutput, int width, int height, string filePath)
        {
            bool success = true;
            using var mre = new ManualResetEventSlim();

            mediaPlayer.SnapshotTaken += MediaPlayerOnSnapshotTaken;
            mediaPlayer.TakeSnapshot((uint)numberOfVideoOutput, filePath, (uint)width, (uint)height);

            if (!mre.Wait(TimeSpan.FromSeconds(1)) && !File.Exists(filePath))
            {
                logger.LogWarning("Timed out when creating snapshot");
                success = false;
            }

            mediaPlayer.SnapshotTaken -= MediaPlayerOnSnapshotTaken;

            void MediaPlayerOnSnapshotTaken(object sender, MediaPlayerSnapshotTakenEventArgs e) => mre.Set();

            return success;
        }

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

        public void SetImageOption(ImageOption option, float value) => mediaPlayer.SetAdjustFloat((VideoAdjustOption)option, value);

        public void EnableImageOptions(bool enable) => mediaPlayer.SetAdjustInt(VideoAdjustOption.Enable, enable ? 1 : 0);

        public void EnableHardwareDecoding(bool enable) => mediaPlayer.EnableHardwareDecoding = enable;

        public void AddMediaOption(string option) => mediaPlayer.Media.AddOption(option);

        public async Task SetMediaTrack(VideoPlayerContract.Enums.TrackType trackType, int trackId)
        {
            if (mediaPlayer.State == VLCState.Ended)
            {
                return;
            }

            mediaTrackService.SetMediaTrack(mediaPlayer, trackType, trackId);
            MediaTracksChanged?.Invoke(this, new MediaTrackChangedEvent(await mediaTrackService.GetTrackInformation(mediaPlayer)));
        }

        public void UnloadMedia() => mediaPlayer.Media?.Dispose();

        public void Dispose()
        {
            streamMediaInput?.Dispose();
            currentStream?.Dispose();
            mediaPlayer.Dispose();
            library.Dispose();
            localFileStreamClient.Dispose();
        }
    }
}