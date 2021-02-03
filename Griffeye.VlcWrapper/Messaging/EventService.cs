using System.IO;
using Griffeye.VideoPlayerContract.Messages.Events;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace Griffeye.VlcWrapper.Messaging
{
    public class EventService : IEventService
    {
        private readonly IMessageSerializer messageSerializer;
        private readonly ILogger<EventService> logger;
        private readonly IMediaPlayer mediaPlayer;

        public EventService(IMessageSerializer messageSerializer, ILogger<EventService> logger, IMediaPlayer mediaPlayer)
        {
            this.messageSerializer = messageSerializer;
            this.logger = logger;
            this.mediaPlayer = mediaPlayer;
        }

        public void Subscribe(Stream eventStream)
        {
            mediaPlayer.EndReached += (sender, arguments) =>
            {
                logger.LogDebug("End reached event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new EndOfFileEvent(), PrefixStyle.Base128);
            };

            mediaPlayer.TimeChanged += (s, a) =>
            {
                logger.LogDebug("Time changed event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new TimeUpdatedEvent(a / 1000f), PrefixStyle.Base128);
            };

            mediaPlayer.LengthChanged += (s, a) =>
            {
                logger.LogDebug("Length changed event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new DurationEvent(a.Length / 1000f), PrefixStyle.Base128);
            };
            
            mediaPlayer.VideoInfoChanged += (s, a) =>
            {
                logger.LogDebug("Video info changed event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream,
                    new MediaInfoEvent(a.AspectRatio, a.VideoOrientation), PrefixStyle.Base128);
            };
            
            mediaPlayer.Playing += (s, a) =>
            {
                logger.LogDebug("Playing event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new PlayingEvent(), PrefixStyle.Base128);
            };
            
            mediaPlayer.Paused += (s, a) =>
            {
                logger.LogDebug("Paused event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new PausedEvent(), PrefixStyle.Base128);
            };
            
            mediaPlayer.VolumeChanged += (s, a) =>
            {
                logger.LogDebug("Volume changed event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new VolumeChangedEvent(a.Volume), PrefixStyle.Base128);
            };
            
            mediaPlayer.Muted += (s, a) =>
            {
                logger.LogDebug("Muted event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new MutedEvent(), PrefixStyle.Base128);
            };
            
            mediaPlayer.Unmuted += (s, a) =>
            {
                logger.LogDebug("Unmuted event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new UnmutedEvent(), PrefixStyle.Base128);
            };

            mediaPlayer.MediaTracksChanged += (s, a) =>
            {
                logger.LogDebug("Media track event triggered.");
                messageSerializer
                    .SerializeWithLengthPrefix(eventStream, new MediaTrackChangedEvent(a.MediaTracks), PrefixStyle.Base128);
            };
        }
    }
}