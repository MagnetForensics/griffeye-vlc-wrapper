using System.IO;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IEventService
    {
        public void Subscribe(Stream eventStream);
    }
}