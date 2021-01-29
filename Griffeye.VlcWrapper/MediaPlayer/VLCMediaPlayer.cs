using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.Models;
using Griffeye.VlcWrapper.Models;
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
        private Stream currStream;
        private StreamMediaInput streamMediaInput;

        private bool aspectRationSet;
        private float startPosition;
        private float stopPosition;
        private long time;

        public event EventHandler<EventArgs> EndReached;
        public event EventHandler<long> TimeChanged;
        public event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;
        public event EventHandler<VideoInfo> VideoInfoChanged;
        public event EventHandler<EventArgs> Playing;
        public event EventHandler<EventArgs> Paused;
        public event EventHandler<MediaPlayerVolumeChangedEventArgs> VolumeChanged;
        public event EventHandler<EventArgs> Unmuted;
        public event EventHandler<EventArgs> Muted;
        public event EventHandler<TrackInformation> MediaTrackChanged;

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

        public VLCMediaPlayer(InputData inputData, ILogger<VLCMediaPlayer> logger)
        {
            this.logger = logger;
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
            mediaPlayer.PositionChanged += MediaPlayer_PositionChanged;
            mediaPlayer.EndReached += (sender, args) =>
            {
                EndReached?.Invoke(this, args);
            };
            mediaPlayer.TimeChanged += (sender, args) =>
            {
                time = args.Time;
                TimeChanged?.Invoke(this, args.Time);

                if (aspectRationSet || mediaPlayer.VideoTrack == -1) { return; }

                aspectRationSet = true;

                uint x = 0;
                uint y = 0;
                float aspectRatio = 1;

                if (!mediaPlayer.Size(0, ref x, ref y)) { return; }

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

                VideoInfoChanged?.Invoke(this, new VideoInfo
                {
                    VideoOrientation = orientation.ToString(),
                    AspectRatio = aspectRatio,
                    MediaTracks = GetTrackInformation()
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
            if (e.Position >= stopPosition) { Task.Run(() => { mediaPlayer.SetPause(true); }); }
        }

        private static bool IsFlipped(VideoOrientation orientation)
        {
            return orientation == VideoOrientation.RightTop ||
                   orientation == VideoOrientation.LeftBottom ||
                   orientation == VideoOrientation.RightBottom;
        }

        private void Library_Log(object sender, LogEventArgs e) => logger.LogDebug("{Module} {Level} {Message}", e.Module, e.Level, e.Message);

        public void ConnectLocalFileStream(string pipeName) { localFileStreamClient.Connect(pipeName); }

        public void DisconnectLocalFileStream() { localFileStreamClient.Disconnect(); }

        public void Play() => mediaPlayer.Play();

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

            // Vlc crashes if image options are enabled when changing video.            
            EnableImageOptions(false);

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

        public void SetMute(bool mute) { mediaPlayer.Mute = mute; }

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

            void MediaPlayerOnSnapshotTaken(object sender, MediaPlayerSnapshotTakenEventArgs e)
            {
                mre.Set();
            }

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

        public void SetMediaTrack(VideoPlayerContract.Enums.TrackType trackType, int trackId)
        {
            switch (trackType)
            {
                case VideoPlayerContract.Enums.TrackType.Audio when mediaPlayer.AudioTrack == trackId:
                case VideoPlayerContract.Enums.TrackType.Video when mediaPlayer.VideoTrack == trackId:
                    return;
                case VideoPlayerContract.Enums.TrackType.Audio:
                    mediaPlayer.SetAudioTrack(trackId);
                    MediaTrackChanged?.Invoke(this, new TrackInformation { TrackId = trackId, TrackType = VideoPlayerContract.Enums.TrackType.Audio });
                    break;
                case VideoPlayerContract.Enums.TrackType.Video:
                    mediaPlayer.SetVideoTrack(trackId);
                    MediaTrackChanged?.Invoke(this, new TrackInformation { TrackId = trackId, TrackType = VideoPlayerContract.Enums.TrackType.Video });
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(trackType), trackType, null);
            }
        }

        private List<TrackInformation> GetTrackInformation()
        {
            var list = new List<TrackInformation>();            
            var videoTracks = mediaPlayer.VideoTrackDescription;
            var audioTracks = mediaPlayer.AudioTrackDescription;

            foreach (var track in videoTracks)
            {
                var trackInformation = new TrackInformation
                {
                    TrackId = track.Id,
                    TrackType = VideoPlayerContract.Enums.TrackType.Video,
                    Description = track.Name,
                    Codec = "unknown",
                    Bitrate = uint.MinValue,
                    IsActive = IsActiveTrack(VideoPlayerContract.Enums.TrackType.Video, track.Id)
                };

                list.Add(trackInformation);
            }

            foreach (var track in audioTracks)
            {
                var trackInformation = new TrackInformation
                {
                    TrackId = track.Id,
                    TrackType = VideoPlayerContract.Enums.TrackType.Audio,
                    Description = track.Name,
                    Codec = "unknown",
                    Bitrate = uint.MinValue,
                    IsActive = IsActiveTrack(VideoPlayerContract.Enums.TrackType.Audio, track.Id)
                };

                list.Add(trackInformation);
            }

            return list;
        }

        private bool IsActiveTrack(VideoPlayerContract.Enums.TrackType type, int trackId)
        {
            switch (type)
            {
                case VideoPlayerContract.Enums.TrackType.Audio when trackId == mediaPlayer.AudioTrack:
                case VideoPlayerContract.Enums.TrackType.Video when trackId == mediaPlayer.VideoTrack:
                    return true;
                default:
                    return false;
            }
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