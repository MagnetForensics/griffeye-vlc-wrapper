using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class AddMediaOption : BaseRequest
    {
        public string Option { get; private set; }

        public AddMediaOption(string option)
        {
            Option = option;
        }

        public AddMediaOption() { }
    }
}