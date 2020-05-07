using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class TimeUpdatedEvent : VideoPlayerEvent
    {
        public float Time { get; private set; }
        public TimeUpdatedEvent(float time)
        {
            Time = time;
        }

        private TimeUpdatedEvent() { }       
    }
}
