using Griffeye.VideoPlayerContract.Enums;
using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Load : BaseRequest
    {
        public string FileToLoad { get; private set; }
        public StreamType Type { get; private set; }

        public Load(StreamType type, string fileToLoad) : this()
        {
            Type = type;
            FileToLoad = fileToLoad;
        }

        private Load() { }
    }
}