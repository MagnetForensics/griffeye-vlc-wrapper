using Griffeye.VideoPlayerContract.Models;
using ProtoBuf;
using System.Collections.Generic;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class MediaInfoEvent : VideoPlayerEvent
    {
        public float AspectRatio { get; private set; }
        public string Orientation { get; private set; }
        public List<TrackInformation> MediaTracks { get; set; }

        /// <summary>
        /// The aspect ratio in width / height and the video orientation
        /// </summary>
        /// <param name="aspectRatio"></param>
        /// <param name="orientation"></param>
        public MediaInfoEvent(float aspectRatio, string orientation, List<TrackInformation> mediaTracks)
        {
            AspectRatio = aspectRatio;
            Orientation = orientation;
            MediaTracks = mediaTracks;
        }

        private MediaInfoEvent() { }
    }
}
