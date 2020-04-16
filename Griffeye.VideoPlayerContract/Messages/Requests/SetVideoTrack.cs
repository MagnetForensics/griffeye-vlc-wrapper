using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SetVideoTrack : BaseRequest
    {
        public int TrackId { get; private set; }
        public SetVideoTrack(int trackId) : this()
        {
            TrackId = trackId;
        }

        public SetVideoTrack() { }
    }
}