using Griffeye.VideoPlayerContract.Models;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Griffeye.VideoPlayerContract.Messages.Events;

[DataContract]
public class MediaTrackChangedEvent
{
    [DataMember]
    public List<TrackInformation> MediaTracks { get; }

    public MediaTrackChangedEvent(List<TrackInformation> mediaTracks)
    {
        MediaTracks = mediaTracks;
    }
}