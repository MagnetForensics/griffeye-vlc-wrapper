using Griffeye.VideoPlayerContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Griffeye.VlcWrapper.Services
{
    public class MediaTrackService : IMediaTrackService
    {
        public void SetMediaTrack(LibVLCSharp.Shared.MediaPlayer mediaPlayer, VideoPlayerContract.Enums.TrackType trackType, int trackId)
        {
            switch (trackType)
            {
                case VideoPlayerContract.Enums.TrackType.Audio when mediaPlayer.AudioTrack == trackId:
                case VideoPlayerContract.Enums.TrackType.Video when mediaPlayer.VideoTrack == trackId:
                    return;
                case VideoPlayerContract.Enums.TrackType.Audio:
                    mediaPlayer.SetAudioTrack(trackId);
                    break;
                case VideoPlayerContract.Enums.TrackType.Video:
                    mediaPlayer.SetVideoTrack(trackId);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(trackType), trackType, null);
            }
        }

        public async Task<List<TrackInformation>> GetTrackInformation(LibVLCSharp.Shared.MediaPlayer mediaPlayer)
        {
            var list = new List<TrackInformation>();            
            await mediaPlayer.Media.Parse(LibVLCSharp.Shared.MediaParseOptions.ParseLocal);

            foreach (var track in mediaPlayer.Media.Tracks)
            {
                if (!IsSupportedTrackType(track.TrackType)) continue;

                var trackInformation = new TrackInformation
                {
                    TrackId = track.Id,
                    TrackType = GetTrackType(track.TrackType),
                    Description = GetTrackDescription(mediaPlayer, track.TrackType, track.Id),
                    Codec = mediaPlayer.Media.CodecDescription(track.TrackType, track.Codec),
                    Bitrate = track.TrackType == LibVLCSharp.Shared.TrackType.Audio ? track.Data.Audio.Rate : track.Data.Video.FrameRateNum,
                    IsActive = IsActiveTrack(mediaPlayer, track.TrackType, track.Id)
                };

                list.Add(trackInformation);
            }

            return list;
        }

        private string GetTrackDescription(LibVLCSharp.Shared.MediaPlayer mediaPlayer, LibVLCSharp.Shared.TrackType trackType, int trackId)
        {
            try
            {
                return trackType switch
                {
                    LibVLCSharp.Shared.TrackType.Audio => mediaPlayer.AudioTrackDescription.FirstOrDefault(x => x.Id == trackId).Name,
                    LibVLCSharp.Shared.TrackType.Video => mediaPlayer.VideoTrackDescription.FirstOrDefault(x => x.Id == trackId).Name,
                    _ => string.Empty
                };
            }
            catch (ArgumentNullException)
            {
                return string.Empty;
            }
        }

        private bool IsSupportedTrackType(LibVLCSharp.Shared.TrackType trackType)
        {
            return trackType switch
            {
                LibVLCSharp.Shared.TrackType.Audio or LibVLCSharp.Shared.TrackType.Video => true,
                _ => false
            };
        }

        private VideoPlayerContract.Enums.TrackType GetTrackType(LibVLCSharp.Shared.TrackType trackType)
        {
            return trackType switch
            {
                LibVLCSharp.Shared.TrackType.Audio => VideoPlayerContract.Enums.TrackType.Audio,
                LibVLCSharp.Shared.TrackType.Video => VideoPlayerContract.Enums.TrackType.Video,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private bool IsActiveTrack(LibVLCSharp.Shared.MediaPlayer mediaPlayer, LibVLCSharp.Shared.TrackType type, int trackId)
        {
            switch (type)
            {
                case LibVLCSharp.Shared.TrackType.Audio when trackId == mediaPlayer.AudioTrack:
                case LibVLCSharp.Shared.TrackType.Video when trackId == mediaPlayer.VideoTrack:
                    return true;
                default:
                    return false;
            }
        }
    }
}
