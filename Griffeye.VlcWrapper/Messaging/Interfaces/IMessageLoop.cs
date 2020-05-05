using System.Threading.Tasks;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IMessageLoop
    {
        public Task Start();
    }
}