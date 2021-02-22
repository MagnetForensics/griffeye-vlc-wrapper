using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    [ProtoInclude(100, typeof(TimeUpdatedEvent))]
    [ProtoInclude(200, typeof(DurationEvent))]
    [ProtoInclude(300, typeof(MediaInfoEvent))]
    [ProtoInclude(400, typeof(MediaTrackChangedEvent))]
    [ProtoInclude(500, typeof(EndOfFileEvent))]
    [ProtoInclude(600, typeof(PlayingEvent))]
    [ProtoInclude(700, typeof(PausedEvent))]
    [ProtoInclude(800, typeof(VolumeChangedEvent))]
    [ProtoInclude(900, typeof(MutedEvent))]
    [ProtoInclude(1000, typeof(UnmutedEvent))]
    [ProtoInclude(1100, typeof(PositionChangedEvent))]
    public abstract class VideoPlayerEvent
    {
    }
}