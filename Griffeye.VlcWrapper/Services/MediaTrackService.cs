using Griffeye.VideoPlayerContract.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibVLCSharp.Shared;

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

        public async Task<List<TrackInformation>> GetTrackInformationAsync(LibVLCSharp.Shared.MediaPlayer mediaPlayer)
        {
            var list = new List<TrackInformation>();

            if (mediaPlayer.Media == null)
            {
                throw new NullReferenceException();
            }

            if (!mediaPlayer.Media.IsParsed)
            {
                await mediaPlayer.Media.Parse();
            }

            foreach (var track in mediaPlayer.Media.Tracks)
            {
                if (!IsSupportedTrackType(track.TrackType)) continue;

                var trackInformation = new TrackInformation
                {
                    TrackId = track.Id,
                    TrackType = GetTrackType(track.TrackType),
                    Description = GetTrackDescription(mediaPlayer, track.TrackType, track.Id),
                    Codec = mediaPlayer.Media.CodecDescription(track.TrackType, track.Codec),
                    Bitrate = track.TrackType == TrackType.Audio ? track.Data.Audio.Rate : track.Data.Video.FrameRateNum,
                    IsActive = IsActiveTrack(mediaPlayer, track.TrackType, track.Id)
                };

                list.Add(trackInformation);
            }

            return list;
        }

        private static string GetTrackDescription(LibVLCSharp.Shared.MediaPlayer mediaPlayer, TrackType trackType, int trackId)
        {
            try
            {
                return trackType switch
                {
                    TrackType.Audio => mediaPlayer.AudioTrackDescription.FirstOrDefault(x => x.Id == trackId).Name,
                    TrackType.Video => mediaPlayer.VideoTrackDescription.FirstOrDefault(x => x.Id == trackId).Name,
                    _ => string.Empty
                };
            }
            catch (ArgumentNullException)
            {
                return string.Empty;
            }
        }

        private static bool IsSupportedTrackType(TrackType trackType)
        {
            return trackType switch
            {
                TrackType.Audio or TrackType.Video => true,
                _ => false
            };
        }

        private VideoPlayerContract.Enums.TrackType GetTrackType(TrackType trackType)
        {
            return trackType switch
            {
                TrackType.Audio => VideoPlayerContract.Enums.TrackType.Audio,
                TrackType.Video => VideoPlayerContract.Enums.TrackType.Video,
                _ => throw new ArgumentOutOfRangeException($"Unsupported track type")
            };
        }

        private static bool IsActiveTrack(LibVLCSharp.Shared.MediaPlayer mediaPlayer, TrackType type, int trackId)
        {
            switch (type)
            {
                case TrackType.Audio when trackId == mediaPlayer.AudioTrack:
                case TrackType.Video when trackId == mediaPlayer.VideoTrack:
                    return true;
                default:
                    return false;
            }
        }
    }
}
