using Griffeye.VideoPlayerContract.Models;
using ProtoBuf;
using System.Collections.Generic;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class MediaTrackChangedEvent : VideoPlayerEvent
    {
        public List<TrackInformation> MediaTracks { get; set; }

        public MediaTrackChangedEvent(List<TrackInformation> mediaTracks)
        {
            MediaTracks = mediaTracks;
        }

        public MediaTrackChangedEvent() { }
    }
}
