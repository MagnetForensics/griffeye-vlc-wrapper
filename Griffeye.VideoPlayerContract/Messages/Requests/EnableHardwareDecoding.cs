using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class EnableHardwareDecoding : BaseRequest
    {
        public bool Enable { get; private set; }

        public EnableHardwareDecoding(bool enable)
        {
            Enable = enable;
        }

        public EnableHardwareDecoding() { }
    }
}