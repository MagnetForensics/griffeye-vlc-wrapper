using System.IO.Pipes;

namespace Griffeye.VideoPlayerContract.Stream.Factories.Interfaces
{
    public interface IStreamFactory
    {
        public System.IO.Stream CreateAnonymousPipeClientStream(PipeDirection direction, string pipeName);
    }
}
