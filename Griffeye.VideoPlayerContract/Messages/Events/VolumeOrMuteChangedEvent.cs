using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class VolumeOrMuteChangedEvent : VideoPlayerEvent
    {
        public readonly float? Volume;
        public readonly bool? Muted;

        public VolumeOrMuteChangedEvent(float? volume, bool? muted)
        {
            Volume = volume;
            Muted = muted;
        }

        public VolumeOrMuteChangedEvent() {}  // for protobuf
    }
}