using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SetAudioTrack : BaseRequest
    {
        public int TrackId { get; private set; }
        public SetAudioTrack(int trackId)
        {
            TrackId = trackId;
        }

        private SetAudioTrack() { }
    }
}