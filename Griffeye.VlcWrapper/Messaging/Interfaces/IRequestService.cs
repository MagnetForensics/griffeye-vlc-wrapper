using System.IO;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IRequestService
    {
        public bool IsQuitMessage(BaseRequest message);
        public bool CanHandleMessage(IMediaPlayer mediaPlayer, BaseRequest message, Stream outStream);
    }
}