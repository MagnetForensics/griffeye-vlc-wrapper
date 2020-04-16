using System.IO;
using Griffeye.VideoPlayerContract.Messages.Events;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using ProtoBuf;

namespace Griffeye.VlcWrapper.Messaging
{
    public class EventService : IEventService
    {
        private readonly IMessageSerializer messageSerializer;

        public EventService(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }

        public void Subscribe(IMediaPlayer mediaPlayer, Stream eventStream)
        {
            mediaPlayer.EndReached += (sender, arguments) => messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new EndOfFileEvent(), PrefixStyle.Base128);

            mediaPlayer.TimeChanged += (s, a) => messageSerializer
                .SerializeWithLengthPrefix(eventStream, new PositionUpdateEvent(a.Time / 1000f), PrefixStyle.Base128);

            mediaPlayer.LengthChanged += (s, a) => messageSerializer
                .SerializeWithLengthPrefix(eventStream, new DurationEvent(a.Length / 1000f), PrefixStyle.Base128);

            mediaPlayer.AspectRatioChanged += (s, a) => messageSerializer
                .SerializeWithLengthPrefix(eventStream, new AspectRatioEvent(a), PrefixStyle.Base128);
        }
    }
}