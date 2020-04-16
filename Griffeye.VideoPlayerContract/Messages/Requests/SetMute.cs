using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SetMute : BaseRequest
    {
        public bool IsMuted { get; private set; }

        public SetMute(bool isMuted) : this() { IsMuted = isMuted; }

        private SetMute() {}
    }
}