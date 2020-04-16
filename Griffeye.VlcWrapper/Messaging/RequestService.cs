using System.IO;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging.Interfaces;

namespace Griffeye.VlcWrapper.Messaging
{
    public class RequestService : IRequestService
    {
        private readonly IResponseService responseService;

        public RequestService(IResponseService responseService)
        {
            this.responseService = responseService;
        }

        public bool IsQuitMessage(BaseRequest message)
        {
            return message is Quit _;
        }

        public bool CanHandleMessage(IMediaPlayer mediaPlayer, BaseRequest message, Stream outStream)
        {
            return ExecuteNoResponseMessage(mediaPlayer, message) ||
                   ExecuteResultResponse(mediaPlayer, message, outStream) ||
                   ExecuteEmptyResponse(mediaPlayer, message, outStream);
        }

        private static bool ExecuteNoResponseMessage(IMediaPlayer mediaPlayer, BaseRequest message)
        {
            switch (message)
            {
                case SetVolume m: mediaPlayer.SetVolume(m.Volume); break;
                case SetMute m: mediaPlayer.SetMute(m.IsMuted); break;
                case SetPlaybackSpeed m: mediaPlayer.SetPlaybackSpeed(m.Speed); break;
                case SetAudioTrack m: mediaPlayer.SetAudioTrack(m.TrackId); break;
                case SetVideoTrack m: mediaPlayer.SetVideoTrack(m.TrackId); break;
                default: return false;
            }

            return true;
        }

        private bool ExecuteResultResponse(IMediaPlayer mediaPlayer, BaseRequest message, Stream outStream)
        {
            var success = true;

            switch (message)
            {
                case Load m: mediaPlayer.LoadMedia(m.Type, m.FileToLoad); break;
                case LocalFileStreamConnect m: mediaPlayer.ConnectLocalFileStream(m.PipeName); break;
                case CreateSnapshot m: 
                    success = mediaPlayer.CreateSnapshot(m.NumberOfVideoOutput, m.Width, m.Height, m.FilePath); break;
                case GetAudioTracks _: 
                    responseService.ReturnResultResponse(outStream, message, mediaPlayer.GetAudioTracks());
                    return true;
                case GetVideoTracks _:
                    responseService.ReturnResultResponse(outStream, message, mediaPlayer.GetVideoTracks()); 
                    return true;
                default: return false;
            }

            responseService.ReturnResultResponse(outStream, message, success);

            return true;
        }

        private bool ExecuteEmptyResponse(IMediaPlayer mediaPlayer, BaseRequest message, Stream outStream)
        {
            switch (message)
            {
                case Play _: mediaPlayer.Play(); break;
                case Pause _: mediaPlayer.Pause(); break;
                case LocalFileStreamDisconnect _: mediaPlayer.DisconnectLocalFileStream(); break;
                case StepForward _: mediaPlayer.StepForward(); break;
                case StepBack _: mediaPlayer.StepBack(); break;
                case Seek m: mediaPlayer.Seek(m.Position); break;
                default: return false;
            }

            responseService.ReturnEmptyResponse(outStream, message);

            return true;
        }
    }
}