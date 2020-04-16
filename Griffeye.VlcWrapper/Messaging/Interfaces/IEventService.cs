using System.IO;
using Griffeye.VlcWrapper.MediaPlayer;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IEventService
    {
        public void Subscribe(IMediaPlayer mediaPlayer, Stream eventStream);
    }
}