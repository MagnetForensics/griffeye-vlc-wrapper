using Griffeye.VideoPlayerContract.Messages.Events;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using ProtoBuf;
using System.IO;

namespace Griffeye.VlcWrapper.Messaging
{
    public class EventService : IEventService
    {
        private readonly IMessageSerializer messageSerializer;
        private readonly IMediaPlayer mediaPlayer;

        public EventService(IMessageSerializer messageSerializer, IMediaPlayer mediaPlayer)
        {
            this.messageSerializer = messageSerializer;
            this.mediaPlayer = mediaPlayer;
        }

        public void Subscribe(Stream eventStream)
        {
            mediaPlayer.EndReached += (sender, arguments) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new EndOfFileEvent(), PrefixStyle.Base128);

            mediaPlayer.TimeChanged += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new TimeUpdatedEvent(a / 1000f), PrefixStyle.Base128);

            mediaPlayer.PositionChanged += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new PositionChangedEvent(a), PrefixStyle.Base128);

            mediaPlayer.LengthChanged += (s, a) => 
            messageSerializer.SerializeWithLengthPrefix(eventStream, new DurationEvent(a.Length / 1000f), PrefixStyle.Base128);
            
            mediaPlayer.VideoInfoChanged += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new MediaInfoEvent(a.AspectRatio, a.VideoOrientation), PrefixStyle.Base128);
            
            mediaPlayer.Playing += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new PlayingEvent(), PrefixStyle.Base128);
            
            mediaPlayer.Paused += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new PausedEvent(), PrefixStyle.Base128);
            
            mediaPlayer.VolumeChanged += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new VolumeChangedEvent(a.Volume), PrefixStyle.Base128);
            
            mediaPlayer.Muted += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new MutedEvent(), PrefixStyle.Base128);
            
            mediaPlayer.Unmuted += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new UnmutedEvent(), PrefixStyle.Base128);

            mediaPlayer.MediaTracksChanged += (s, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new MediaTrackChangedEvent(a.MediaTracks), PrefixStyle.Base128);
        }
    }
}