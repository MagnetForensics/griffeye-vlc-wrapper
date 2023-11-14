using System;
using System.Threading.Tasks;
using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.MediaPlayer.Client.Interfaces;
using Griffeye.VideoPlayerContract.MediaPlayer.Interfaces;
using Griffeye.VideoPlayerContract.Messages.Events;
using TrackType = Griffeye.VideoPlayerContract.Enums.TrackType;

namespace Griffeye.VideoPlayerContract.MediaPlayer.Client;

// Methods must be named according to RpcService so disable warning
#pragma warning disable VSTHRD200

public class JsonRpcMediaPlayer : IRpcMediaPlayer
{
    public event EventHandler<EventArgs> Playing;
    public event EventHandler<EventArgs> Paused;
    public event EventHandler<EventArgs> EndReached;
    public event EventHandler<float> TimeChanged;
    public event EventHandler<float> PositionChanged;
    public event EventHandler<float> LengthChanged;
    public event EventHandler<VideoInfoEvent> VideoInfoChanged;
    public event EventHandler<VolumeOrMuteChangedEvent> VolumeChanged;
    public event EventHandler<EventArgs> Muted;
    public event EventHandler<EventArgs> UnMuted;
    public event EventHandler<MediaTrackChangedEvent> MediaTracksChanged;

    private readonly IMediaPlayer mediaPlayer;

    public JsonRpcMediaPlayer(IMediaPlayer mediaPlayer)
    {
        this.mediaPlayer = mediaPlayer;

        mediaPlayer.EndReached += (_, args) => EndReached?.Invoke(this, args);
        mediaPlayer.TimeChanged += (_, time) => TimeChanged?.Invoke(this, (time / 1000f));
        mediaPlayer.PositionChanged += (_, time) => PositionChanged?.Invoke(this, time);
        mediaPlayer.LengthChanged += (_, time) => LengthChanged?.Invoke(this, (time / 1000f));
        mediaPlayer.VideoInfoChanged += (_, args) => VideoInfoChanged?.Invoke(this, args);
        mediaPlayer.Playing += (_, args) => Playing?.Invoke(this, args);
        mediaPlayer.Paused += (_, args) => Paused?.Invoke(this, args);
        mediaPlayer.VolumeChanged += (_, args) => VolumeChanged?.Invoke(this, args);
        mediaPlayer.Muted += (_, args) => Muted?.Invoke(this, args);
        mediaPlayer.UnMuted += (_, args) => UnMuted?.Invoke(this, args);
        mediaPlayer.MediaTracksChanged += (_, args) => MediaTracksChanged?.Invoke(this, args);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);

        // This proxy class is an JsonRpc instance that is Disposable
        ((IDisposable) mediaPlayer).Dispose();
    }

    public async Task ConnectLocalFileStreamAsync(string pipeName)
    {
        await Task.Run(() => mediaPlayer.ConnectLocalFileStream(pipeName));
    }

    public async Task PlayAsync()
    {
        await Task.Run(() => mediaPlayer.Play());
    }

    public async Task<bool> IsPlayingAsync()
    {
        return await Task.Run(() => mediaPlayer.IsPlaying());
    }

    public async Task PauseAsync()
    {
        await Task.Run(() => mediaPlayer.Pause());
    }

    public async Task LoadMediaAsync(string file, float startPosition, float stopPosition)
    {
        await mediaPlayer.LoadMediaAsync(file, startPosition, stopPosition);
    }

    public void LoadMediaStream(string file, float startPosition, float stopPosition)
    {
        mediaPlayer.LoadMediaStream(file, startPosition, stopPosition);
    }

    public async Task SeekAsync(float position)
    {
        await Task.Run(() => mediaPlayer.Seek(position));
    }

    public async Task SetPlaybackSpeedAsync(float speed)
    {
        await Task.Run(() => mediaPlayer.SetPlaybackSpeed(speed));
    }

    public async Task SetVolumeAsync(int volume)
    {
        await Task.Run(() => mediaPlayer.SetVolume(volume));
    }

    public async Task SetMuteAsync(bool mute)
    {
        await Task.Run(() => mediaPlayer.SetMute(mute));
    }

    public async Task StepForwardAsync()
    {
        await Task.Run(() => mediaPlayer.StepForward());
    }

    public async Task StepBackAsync()
    {
        await Task.Run(() => mediaPlayer.StepBack());
    }

    public async Task DisconnectLocalFileStreamAsync()
    {
        await Task.Run(() => mediaPlayer.DisconnectLocalFileStream());
    }

    public async Task<bool> CreateSnapshotAsync(int numberOfVideoOutput, int width, int height, string filePath)
    {
        return await Task.Run(() => mediaPlayer.CreateSnapshot(numberOfVideoOutput, width, height, filePath));
    }

    public async Task SetImageOptionAsync(ImageOption option, float value)
    {
        await Task.Run(() => mediaPlayer.SetImageOption(option, value));
    }

    public async Task EnableImageOptionsAsync(bool enable)
    {
        await Task.Run(() => mediaPlayer.EnableImageOptions(enable));
    }

    public async Task EnableHardwareDecodingAsync(bool enable)
    {
        await Task.Run(() => mediaPlayer.EnableHardwareDecoding(enable));
    }

    public async Task AddMediaOptionAsync(string option)
    {
        await Task.Run(() => mediaPlayer.AddMediaOption(option));
    }

    public async Task SetMediaTrackAsync(TrackType trackType, int trackId)
    {
        await mediaPlayer.SetMediaTrackAsync(trackType, trackId);
    }

    public async Task UnloadMediaAsync()
    {
        await Task.Run(() => mediaPlayer.UnloadMedia());
    }
}