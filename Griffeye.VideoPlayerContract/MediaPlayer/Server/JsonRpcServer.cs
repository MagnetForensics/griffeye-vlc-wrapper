using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Griffeye.VideoPlayerContract.Logging;
using Griffeye.VideoPlayerContract.Logging.Interfaces;
using Griffeye.VideoPlayerContract.MediaPlayer.Client;
using Griffeye.VideoPlayerContract.MediaPlayer.Client.Interfaces;
using Griffeye.VideoPlayerContract.MediaPlayer.Server.Interfaces;
using Griffeye.VideoPlayerContract.Stream;
using Griffeye.VideoPlayerContract.Stream.Factories.Interfaces;
using StreamJsonRpc;

namespace Griffeye.VideoPlayerContract.MediaPlayer.Server;

public class RpcServer : IRpcServer
{
    public event EventHandler<string> Exited;
    private readonly JsonRpc jsonRpc;

    public RpcServer(IFullDuplexStreamFactory fullDuplexStreamFactory, IRpcMediaPlayer mediaPlayer, IRpcLoggerFactory rpcLoggerFactory)
    {
        var inStream = fullDuplexStreamFactory.CreateInStream();
        var outStream = fullDuplexStreamFactory.CreateOutStream();

        var messageHandler = new NewLineDelimitedMessageHandler(outStream, inStream, new JsonMessageFormatter());

        jsonRpc = new JsonRpc(messageHandler, mediaPlayer);

        if (rpcLoggerFactory != null)
        {
            jsonRpc.TraceSource = new TraceSource("MediaPlayerSource")
            {
                Listeners =
                {
                    rpcLoggerFactory.Create()
                },
                Switch =
                {
                    Level = rpcLoggerFactory.IsDebug() ? SourceLevels.Information : SourceLevels.Error
                }
            };
        }
        
        jsonRpc.Disconnected += (sender, args) => Exited?.Invoke(sender, args.Description);
    }

    public Task StartAsync()
    {
        jsonRpc.StartListening();
        return jsonRpc.Completion;
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        jsonRpc.Dispose();
    }
}