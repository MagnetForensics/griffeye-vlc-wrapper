using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Responses
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    [ProtoInclude(100, typeof(ResultResponse))]
    public class BaseResponse
    {
        public int SequenceNumber { get; private set; }
        public BaseResponse(int sequenceNumber) { SequenceNumber = sequenceNumber; }
        private BaseResponse() { }
    }
}