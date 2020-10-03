using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class EnableImageOptions : BaseRequest
    {
        public bool Enable { get; private set; }

        public EnableImageOptions(bool enable)
        {
            Enable = enable;
        }

        public EnableImageOptions() { }
    }
}