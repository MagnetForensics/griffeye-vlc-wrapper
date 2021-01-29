using Griffeye.VideoPlayerContract.Models;
using System.Collections.Generic;

namespace Griffeye.VlcWrapper.Models
{
    public class VideoInfo
    {
        public float AspectRatio { get; set; }
        public string VideoOrientation { get; set; }
        public List<TrackInformation> MediaTracks { get; set; }
    }
}