using System.Runtime.Serialization;

namespace Griffeye.VideoPlayerContract.Messages.Events;

[DataContract]
public class VideoInfoEvent
{
    [DataMember]
    public float AspectRatio { get; set; }
    [DataMember]
    public string VideoOrientation { get; set; }
}