using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class UnmutedEvent : VideoPlayerEvent
    {
        public UnmutedEvent() { }
    }
}