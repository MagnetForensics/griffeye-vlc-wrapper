using System.Collections.Generic;
using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Responses
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class TracksResponse : BaseResponse
    {
        public List<(int, string)> Tracks { get; private set; }

        public TracksResponse(List<(int, string)> tracks, int sequenceNumber) : base(sequenceNumber) { Tracks = tracks; }

        public TracksResponse() : base(-1) { }
    }
}