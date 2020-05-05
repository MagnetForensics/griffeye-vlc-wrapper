using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class MutedEvent : VideoPlayerEvent
    {
        public MutedEvent() { }
    }
}