using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SetPlaybackSpeed : BaseRequest
    {
        public float Speed { get; private set; }

        public SetPlaybackSpeed(float speed) : this() { Speed = speed; }

        private SetPlaybackSpeed() { }
    }
}