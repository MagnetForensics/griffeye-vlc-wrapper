using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.Messages.Events;
using Griffeye.VlcWrapper.Models;
using LibVLCSharp.Shared;
using System;

namespace Griffeye.VlcWrapper.MediaPlayer
{
    public interface IMediaPlayer
    {
        event EventHandler<EventArgs> EndReached;
        event EventHandler<long> TimeChanged;
        event EventHandler<MediaPlayerLengthChangedEventArgs> LengthChanged;
        event EventHandler<VideoInfo> VideoInfoChanged;
        event EventHandler<EventArgs> Playing;
        event EventHandler<EventArgs> Paused;
        event EventHandler<MediaPlayerVolumeChangedEventArgs> VolumeChanged;
        event EventHandler<EventArgs> Unmuted;
        event EventHandler<EventArgs> Muted;
        event EventHandler<MediaTrackChangedEvent> MediaTracksChanged;

        public void ConnectLocalFileStream(string pipeName);
        void Play();
        void Pause();
        void LoadMedia(StreamType type, string file, float startPosition, float stopPosition);
        void Seek(float position);
        void SetPlaybackSpeed(float speed);
        void SetVolume(int volume);
        void SetMute(bool mute);
        void StepForward();
        void StepBack();
        void DisconnectLocalFileStream();
        bool CreateSnapshot(int numberOfVideoOutput, int width, int height, string filePath);
        void SetImageOption(ImageOption option, float value);
        void EnableImageOptions(bool enable);
        void EnableHardwareDecoding(bool enable);
        void AddMediaOption(string option);
        void SetMediaTrack(VideoPlayerContract.Enums.TrackType trackType, int trackId);
    }
}