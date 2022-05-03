using System.Runtime.Serialization;

namespace Griffeye.VideoPlayerContract.Messages.Events;

[DataContract]
public class VolumeOrMuteChangedEvent
{
    [DataMember]
    public float? Volume { get; set; }
    [DataMember]
    public bool? Muted { get; set; }

    public VolumeOrMuteChangedEvent(float? volume, bool? muted)
    {
        Volume = volume;
        Muted = muted;
    }
}