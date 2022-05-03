using System.IO.Pipes;
using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.MediaPlayer.Client;
using Griffeye.VideoPlayerContract.MediaPlayer.Client.Factories;
using Griffeye.VideoPlayerContract.MediaPlayer.Client.Interfaces;
using Griffeye.VideoPlayerContract.Messages.Events;

namespace Griffeye.VideoPlayerForms.Test;

public class VideoPlayer : IDisposable
{
    public event EventHandler<float>? TimeChanged;
    public event EventHandler<float>? LengthChanged;
    public event EventHandler? Playing;
    public event EventHandler? Paused;
    public event EventHandler? EndReached;
    public event EventHandler? Exited;
    public event EventHandler<MediaTrackChangedEvent>? MediaTrackChanged;

    public bool IsRunning => subProcess.IsRunning;
    public Task<bool> IsPlaying => mediaPlayer.IsPlayingAsync();
    
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly IntPtr videoHandle;

    private readonly SubProcessHost subProcess;
    private readonly IRpcMediaPlayer mediaPlayer;
    private CancellationToken CancellationToken => cancellationTokenSource.Token;
    
    public VideoPlayer(IntPtr videoHandle)
    {
        var inPipe = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);
        var outPipe = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
        
        subProcess = new SubProcessHost(inPipe.GetClientHandleAsString(), outPipe.GetClientHandleAsString());
        this.videoHandle = videoHandle;
        cancellationTokenSource = new CancellationTokenSource();
        subProcess.Exited += (_, _) =>
        {
            Dispose();
            Exited?.Invoke(this, EventArgs.Empty);
        };
        mediaPlayer = new JsonRpcMediaPlayerFactory(outPipe, inPipe).Create();
        mediaPlayer.Playing += (_,args) => Playing?.Invoke(this, args);
        mediaPlayer.Paused += (_,args) => Paused?.Invoke(this, args);
        mediaPlayer.EndReached += (_,args) => EndReached?.Invoke(this, args);
        mediaPlayer.TimeChanged += (_, time) => TimeChanged?.Invoke(this, time);
        mediaPlayer.LengthChanged += (_, time) => LengthChanged?.Invoke(this, time);
        mediaPlayer.MediaTracksChanged += (_, args) => MediaTrackChanged?.Invoke(this, args);
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        subProcess.Dispose();
        cancellationTokenSource.Cancel();
        mediaPlayer?.Dispose();
    }

    public async Task InitAsync()
    {
        await StartSubProcessAsync(videoHandle);
    }

    public async Task LoadFileAsync(string filePath)
    {
        await mediaPlayer.LoadMediaAsync(StreamType.File, filePath, 0, 1F);
    }

    public async Task PlayAsync()
    {
        await mediaPlayer.PlayAsync();
    }

    public async Task PauseAsync()
    {
        await mediaPlayer.PauseAsync();
    }

    public async Task SkipToStartAsync()
    {
        await mediaPlayer.SeekAsync(0);
    }
    
    public async Task SkipToEndAsync()
    {
        await mediaPlayer.SeekAsync(1F);
    }
    
    public async Task StepForwardAsync()
    {
        await mediaPlayer.StepForwardAsync();
    }
    
    public async Task StepBackAsync()
    {
        await mediaPlayer.StepBackAsync();
    }

    private Task StartSubProcessAsync(IntPtr handle)
    {
        subProcess.Exited += (_, _) => Exited?.Invoke(this, EventArgs.Empty);
        return Task.Run(() => { subProcess.Start(handle); }, CancellationToken);
    }
}