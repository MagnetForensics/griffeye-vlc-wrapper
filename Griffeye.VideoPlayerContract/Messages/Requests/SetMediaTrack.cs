using Griffeye.VideoPlayerContract.Enums;
using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SetMediaTrack : BaseRequest
    {
        public int TrackId { get; private set; }
        public TrackType TrackType { get; private set; }
        public SetMediaTrack(TrackType trackType, int trackId)
        {
            TrackType = trackType;
            TrackId = trackId;
        }

        private SetMediaTrack() { }
    }
}
