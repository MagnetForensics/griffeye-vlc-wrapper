using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SetVolume : BaseRequest
    {
        public int Volume { get; private set; }

        public SetVolume(int volume)
        { 
            Volume = volume;
        }

        private SetVolume() {}
    }
}