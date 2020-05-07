using Griffeye.VideoPlayerContract.Enums;
using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Load : BaseRequest
    {
        public string FileToLoad { get; private set; }
        public float StartPosition { get; private set; }
        public float StopPosition { get; private set; }
        public StreamType Type { get; private set; }

        public Load(StreamType type, string fileToLoad, float startPosition, float stopPosition)
        {
            Type = type;
            FileToLoad = fileToLoad;
            StartPosition = startPosition;
            StopPosition = stopPosition;
        }

        private Load() { }
    }
}