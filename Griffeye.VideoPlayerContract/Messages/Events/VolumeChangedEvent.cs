using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class VolumeChangedEvent : VideoPlayerEvent
    {
        public float Volume { get; private set; }
        public VolumeChangedEvent(float volume)
        {
            Volume = volume;
        }
        public VolumeChangedEvent() { }
    }
}