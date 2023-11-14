using System.IO.Pipes;
using Griffeye.VideoPlayerContract.Stream.Factories.Interfaces;

namespace Griffeye.VideoPlayerContract.Stream.Factories;

public class StreamFactory : IStreamFactory
{
    public System.IO.Stream CreateAnonymousPipeClientStream(PipeDirection direction, string pipeName)
    {
        return new AnonymousPipeClientStream(direction, pipeName);
    }
}