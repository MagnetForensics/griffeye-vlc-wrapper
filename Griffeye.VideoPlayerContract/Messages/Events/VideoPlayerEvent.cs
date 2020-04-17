using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    [ProtoInclude(100, typeof(PositionUpdateEvent))]
    [ProtoInclude(200, typeof(DurationEvent))]
    [ProtoInclude(300, typeof(MediaInfoEvent))]
    [ProtoInclude(400, typeof(LogEvent))]
    [ProtoInclude(500, typeof(EndOfFileEvent))]
    public abstract class VideoPlayerEvent
    {
    }
}
