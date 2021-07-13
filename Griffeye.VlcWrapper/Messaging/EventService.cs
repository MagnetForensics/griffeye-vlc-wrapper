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
            mediaPlayer.EndReached += (_, _) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new EndOfFileEvent(), PrefixStyle.Base128);

            mediaPlayer.TimeChanged += (_, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new TimeUpdatedEvent(a / 1000f), PrefixStyle.Base128);

            mediaPlayer.PositionChanged += (_, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new PositionChangedEvent(a), PrefixStyle.Base128);

            mediaPlayer.LengthChanged += (_, a) => 
            messageSerializer.SerializeWithLengthPrefix(eventStream, new DurationEvent(a.Length / 1000f), PrefixStyle.Base128);
            
            mediaPlayer.VideoInfoChanged += (_, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new MediaInfoEvent(a.AspectRatio, a.VideoOrientation), PrefixStyle.Base128);
            
            mediaPlayer.Playing += (_, _) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new PlayingEvent(), PrefixStyle.Base128);
            
            mediaPlayer.Paused += (_, _) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new PausedEvent(), PrefixStyle.Base128);
            
            mediaPlayer.VolumeChanged += (_, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new VolumeOrMuteChangedEvent(a.Volume, null), PrefixStyle.Base128);
            
            mediaPlayer.Muted += (_, _) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new VolumeOrMuteChangedEvent(null, true), PrefixStyle.Base128);
            
            mediaPlayer.Unmuted += (_, _) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new VolumeOrMuteChangedEvent(null, false), PrefixStyle.Base128);

            mediaPlayer.MediaTracksChanged += (_, a) =>
            messageSerializer.SerializeWithLengthPrefix(eventStream, new MediaTrackChangedEvent(a.MediaTracks), PrefixStyle.Base128);
        }
    }
}