using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class PositionChangedEvent : VideoPlayerEvent
    {
        public float Position { get; private set; }

        public PositionChangedEvent(float position)
        {
            Position = position;
        }

        private PositionChangedEvent() { }
    }
}
