using Griffeye.VideoPlayerContract.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Griffeye.VlcWrapper.Services
{
    public interface IMediaTrackService
    {
        void SetMediaTrack(LibVLCSharp.Shared.MediaPlayer mediaPlayer, VideoPlayerContract.Enums.TrackType trackType, int trackId);
        Task<List<TrackInformation>> GetTrackInformationAsync(LibVLCSharp.Shared.MediaPlayer mediaPlayer);
    }
}