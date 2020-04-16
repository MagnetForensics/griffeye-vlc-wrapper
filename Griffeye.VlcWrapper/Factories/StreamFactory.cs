using System.IO.Pipes;
using Griffeye.VlcWrapper.Messaging.Interfaces;

namespace Griffeye.VlcWrapper.Factories
{
    public class StreamFactory : IStreamFactory
    {
        public AnonymousPipeClientStream CreateAnonymousPipeClientStream(PipeDirection direction, string pipeName)
        {
            return new AnonymousPipeClientStream(direction, pipeName);
        }
    }
}