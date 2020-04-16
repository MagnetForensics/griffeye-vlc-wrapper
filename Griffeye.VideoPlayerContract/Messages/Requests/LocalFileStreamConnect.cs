using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class LocalFileStreamConnect : BaseRequest
    {
        public string PipeName { get; private set; }
        public LocalFileStreamConnect(string pipeName) : this() { PipeName = pipeName; }
        private LocalFileStreamConnect() {}
    }
}