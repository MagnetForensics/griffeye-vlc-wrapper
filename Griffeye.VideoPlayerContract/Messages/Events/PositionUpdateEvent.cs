using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class PositionUpdateEvent : VideoPlayerEvent
    {
        public PositionUpdateEvent(float position)
        {
            Position = position;
        }

        private PositionUpdateEvent() { }

        public float Position { get; private set; }
    }
}
