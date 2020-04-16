using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Responses
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class ResultResponse : BaseResponse
    {
        public ResultResponse(int sequenceNumber, bool success) : base(sequenceNumber)
        {
            Success = success;
        }

        private ResultResponse() : base(-1) { }

        public bool Success { get; private set; }
    }
}