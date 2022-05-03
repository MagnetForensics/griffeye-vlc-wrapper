using Griffeye.VideoPlayerContract.MediaPlayer.Client.Interfaces;
using Griffeye.VideoPlayerContract.Stream.Factories.Interfaces;
using StreamJsonRpc;

namespace Griffeye.VideoPlayerContract.MediaPlayer.Client.Factories;

public class JsonRpcMediaPlayerFactory : IRpcMediaPlayerFactory
{
    private readonly IJsonRpcMessageHandler messageHandler;

    public JsonRpcMediaPlayerFactory(IFullDuplexStreamFactory fullDuplexStreamFactory) : this(
        fullDuplexStreamFactory.CreateOutStream(),
        fullDuplexStreamFactory.CreateInStream())
    {
    }

    public JsonRpcMediaPlayerFactory(System.IO.Stream sendStream, System.IO.Stream readStream)
    {
        messageHandler = new NewLineDelimitedMessageHandler(sendStream, readStream, new JsonMessageFormatter());
    }

    public IRpcMediaPlayer Create()
    {
        return JsonRpc.Attach<IRpcMediaPlayer>(messageHandler);
    }
}