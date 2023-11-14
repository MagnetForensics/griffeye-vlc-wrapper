using System.Runtime.Serialization;
using Griffeye.VideoPlayerContract.Enums;

namespace Griffeye.VideoPlayerContract.Models;

[DataContract]
public class TrackInformation
{
    [DataMember]
    public int TrackId { get; set; }
    [DataMember]
    public string Description { get; set; }
    [DataMember]
    public TrackType TrackType { get; set; }
    [DataMember]
    public string Codec { get; set; }
    [DataMember]
    public uint Bitrate { get; set; }
    [DataMember]
    public bool IsActive { get; set; }
}