using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;
using System.IO;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IMessageService
    {
        public bool Process(BaseRequest message, Stream eventStream, Stream outStream);
    }
}