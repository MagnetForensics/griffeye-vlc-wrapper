using System;
using System.Threading.Tasks;
using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.Messages.Events;

namespace Griffeye.VideoPlayerContract.MediaPlayer.Client.Interfaces
{
    // The RPC service and IMediaPlayer are so closely coupled that it makes sense to reuse it here
    public interface IRpcMediaPlayer : IDisposable
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

        Task ConnectLocalFileStreamAsync(string pipeName);
        Task PlayAsync();
        Task<bool> IsPlayingAsync();
        Task PauseAsync();
        Task LoadMediaAsync(string file, float startPosition, float stopPosition);
        void LoadMediaStream(string file, float startPosition, float stopPosition);
        Task SeekAsync(float position);
        Task SetPlaybackSpeedAsync(float speed);
        Task SetVolumeAsync(int volume);
        Task SetMuteAsync(bool mute);
        Task StepForwardAsync();
        Task StepBackAsync();
        Task DisconnectLocalFileStreamAsync();
        Task<bool> CreateSnapshotAsync(int numberOfVideoOutput, int width, int height, string filePath);
        Task SetImageOptionAsync(ImageOption option, float value);
        Task EnableImageOptionsAsync(bool enable);
        Task EnableHardwareDecodingAsync(bool enable);
        Task AddMediaOptionAsync(string option);
        Task SetMediaTrackAsync(TrackType trackType, int trackId);
        Task UnloadMediaAsync();
    }
}
