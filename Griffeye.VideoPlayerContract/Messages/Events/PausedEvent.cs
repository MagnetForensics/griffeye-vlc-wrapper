using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class PausedEvent : VideoPlayerEvent
    {
        public PausedEvent() { }
    }
}