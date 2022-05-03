using System;
using System.Threading.Tasks;
using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.Messages.Events;

namespace Griffeye.VideoPlayerContract.MediaPlayer.Interfaces
{
    public interface IMediaPlayer
    {
        event EventHandler<EventArgs> EndReached;
        event EventHandler<float> TimeChanged;
        event EventHandler<float> PositionChanged;
        event EventHandler<float> LengthChanged;
        event EventHandler<VideoInfoEvent> VideoInfoChanged;
        event EventHandler<EventArgs> Playing;
        event EventHandler<EventArgs> Paused;
        event EventHandler<VolumeOrMuteChangedEvent> VolumeChanged;
        event EventHandler<EventArgs> UnMuted;
        event EventHandler<EventArgs> Muted;
        event EventHandler<MediaTrackChangedEvent> MediaTracksChanged;

        void ConnectLocalFileStream(string pipeName);
        void Play();
        bool IsPlaying();
        void Pause();
        Task LoadMediaAsync(StreamType type, string file, float startPosition, float stopPosition);
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
        Task SetMediaTrackAsync(TrackType trackType, int trackId);
        void UnloadMedia();
    }
}