using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.Messages.Events;
using ProtoBuf;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VideoPlayerContract.Messages.Responses;

namespace Griffeye.VideoPlayerForms.Test;

public class VideoPlayer : IDisposable
{
    public event EventHandler<EventArgs>? Playing;
    public event EventHandler<EventArgs>? Paused;
    public event EventHandler<EventArgs>? Exited;

    public bool IsRunning => subProcess.IsRunning;
    
    private readonly TimeSpan defaultTimeout = TimeSpan.FromSeconds(4);
    private readonly CancellationTokenSource cancellationTokenSource;
    private readonly IntPtr videoHandle;

    private int currSecNum;
    private CancellationToken CancellationToken => cancellationTokenSource.Token;
    private SubProcessHost subProcess;
    
    public VideoPlayer(IntPtr videoHandle)
    {
        subProcess = new SubProcessHost();
        this.videoHandle = videoHandle;
        cancellationTokenSource = new CancellationTokenSource();
        subProcess.Exited += (_, _) =>
        {
            Dispose();
            Exited?.Invoke(this, EventArgs.Empty);
        };
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        subProcess.Dispose();
        cancellationTokenSource.Cancel();
    }

    public async Task Init()
    {
        try
        {
            await StartSubProcess(videoHandle);
            SubProcessEventProcessorAsync(CancellationToken).ForgetButLogExceptions();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public async Task LoadFile(string filePath)
    {
        await SendToSubProcess<BaseResponse>(new Load(StreamType.File, filePath, 0, 1F));
    }

    public async Task Play()
    {
        await SendToSubProcess<BaseResponse>(new Play());
    }

    public async Task Pause()
    {
        await SendToSubProcess<BaseResponse>(new Pause());
    }

    private Task StartSubProcess(IntPtr handle)
    {
        subProcess = new SubProcessHost();
        subProcess.Exited += (_, _) => Exited?.Invoke(this, EventArgs.Empty);
        return Task.Run(() => { subProcess.Start(handle); }, CancellationToken);
    }

    private async Task SubProcessEventProcessorAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (subProcess.PipeEvent.IsConnected && !cancellationToken.IsCancellationRequested)
            {
                // Wait until there is data in the stream before attempting to deserialize
                await subProcess.PipeEvent
                    .ReadAsync(Array.Empty<byte>().AsMemory(0, 0), cancellationToken)
                    .ConfigureAwait(false);

                var message = await Task.Run(() => Serializer
                        .DeserializeWithLengthPrefix<VideoPlayerEvent>(subProcess.PipeEvent, PrefixStyle.Base128),
                    cancellationToken).ConfigureAwait(false);

                if (message is not null)
                {
                    HandleMessages(message);
                }
            }
        }
        catch (ObjectDisposedException)
        {
            throw new Exception();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void HandleMessages(VideoPlayerEvent message)
    {
        switch (message)
        {
            // Handle incoming messages
            case PlayingEvent _: Playing?.Invoke(this, EventArgs.Empty); break;
            case PausedEvent _: Paused?.Invoke(this, EventArgs.Empty); break;
        }
    }

    private async Task<TReturnType> SendToSubProcess<TReturnType>(BaseRequest message)
        where TReturnType : BaseResponse
    {
        using var cts = new CancellationTokenSource();
        var messageTask = Task.Run(() =>
        {
            SendMessage(message);
            var response = Serializer.DeserializeWithLengthPrefix<TReturnType>(subProcess.PipeReceive, PrefixStyle.Base128);

            if (response?.SequenceNumber != message.SequenceNumber)
            {
                throw new Exception("Mismatching sequence number");
            }

            return response;
        }, cts.Token);

        var completedTask = await Task.WhenAny(messageTask, Task.Delay(defaultTimeout, cts.Token)).ConfigureAwait(false);

        cts.Cancel();
        
        if (completedTask != messageTask)
        {
            throw new Exception($"Timeout when sending '{message.GetType().Name}' to video sub process.");
        }

        return await messageTask.ConfigureAwait(false);
    }

    private void SendMessage(BaseRequest message)
    {
        if (subProcess == null)
        {
            throw new Exception(
                "Communication pipe not set up. SubProcess might not be started or the Presenter might be disposed.");
        }

        if (!subProcess.IsRunning)
        {
            throw new Exception("The sub process is not running.");
        }

        message.SequenceNumber = Interlocked.Increment(ref currSecNum);
        Serializer.SerializeWithLengthPrefix(subProcess.PipeSend, message, PrefixStyle.Base128);
    }
}