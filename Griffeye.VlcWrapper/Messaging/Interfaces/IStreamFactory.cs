using System.IO;
using System.IO.Pipes;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IStreamFactory
    {
        public Stream CreateAnonymousPipeClientStream(PipeDirection direction, string pipeName);
    }
}
