using Griffeye.VideoPlayerContract.Enums;
using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Models
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class TrackInformation
    {
        public int TrackId { get; set; }
        public string Description { get; set; }
        public TrackType TrackType { get; set; }
        public string Codec { get; set; }
        public uint Bitrate { get; set; }
        public bool IsActive { get; set; }

        public TrackInformation() { }
    }
}
