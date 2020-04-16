using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class GetVideoTracks : BaseRequest
    {
        public GetVideoTracks() { }
    }
}