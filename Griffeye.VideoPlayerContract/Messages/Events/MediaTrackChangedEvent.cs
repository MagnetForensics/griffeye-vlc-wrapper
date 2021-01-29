using Griffeye.VideoPlayerContract.Enums;
using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class MediaTrackChangedEvent : VideoPlayerEvent
    {
        public TrackType TrackType { get; private set; }
        public int TrackId { get; private set; }

        public MediaTrackChangedEvent(TrackType trackType, int trackId)
        {
            TrackType = trackType;
            TrackId = trackId;
        }

        public MediaTrackChangedEvent() { }
    }
}
