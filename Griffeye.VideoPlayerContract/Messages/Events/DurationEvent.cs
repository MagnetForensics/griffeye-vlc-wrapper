using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class DurationEvent : VideoPlayerEvent
    {
        public DurationEvent(float duration)
        {
            Duration = duration;
        }

        private DurationEvent() { }

        public float Duration { get; private set; }
    }
}
