using System.IO.Pipes;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IStreamFactory
    {
        public AnonymousPipeClientStream CreateAnonymousPipeClientStream(PipeDirection direction, string pipeName);
    }
}
