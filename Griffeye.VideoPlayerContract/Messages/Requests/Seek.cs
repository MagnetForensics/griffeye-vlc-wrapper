using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class Seek : BaseRequest
    {
        public float Position { get; private set; }

        public Seek(float position) : this() { Position = position; }

        private Seek() {}
    }
}