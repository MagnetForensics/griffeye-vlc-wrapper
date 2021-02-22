using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Griffeye.VlcWrapper.Messaging
{
    public class RequestService : IRequestService
    {
        private readonly IResponseService responseService;
        private readonly ILogger<RequestService> logger;

        public RequestService(IResponseService responseService, ILogger<RequestService> logger)
        {
            this.responseService = responseService;
            this.logger = logger;
        }

        public bool IsQuitMessage(BaseRequest message) => message is Quit _;

        public bool CanHandleMessage(IMediaPlayer mediaPlayer, BaseRequest message, Stream outStream)
        {
            var result = ExecuteResultResponse(mediaPlayer, message, outStream) ||
                   ExecuteEmptyResponse(mediaPlayer, message, outStream);

            logger.LogDebug("Handled message of {Type} with {Sequence}", message.GetType(), message.SequenceNumber);

            return result;
        }

        private bool ExecuteResultResponse(IMediaPlayer mediaPlayer, BaseRequest message, Stream outStream)
        {
            var success = true;

            switch (message)
            {
                case Load m: mediaPlayer.LoadMedia(m.Type, m.FileToLoad, m.StartPosition, m.StopPosition); break;
                case LocalFileStreamConnect m: mediaPlayer.ConnectLocalFileStream(m.PipeName); break;
                case CreateSnapshot m: success = mediaPlayer.CreateSnapshot(m.NumberOfVideoOutput, m.Width, m.Height, m.FilePath); break;
                default: return false;
            }

            responseService.ReturnResultResponse(outStream, message, success);

            return true;
        }

        private bool ExecuteEmptyResponse(IMediaPlayer mediaPlayer, BaseRequest message, Stream outStream)
        {
            switch (message)
            {
                case SetVolume m: mediaPlayer.SetVolume(m.Volume); break;
                case SetMute m: mediaPlayer.SetMute(m.IsMuted); break;
                case SetPlaybackSpeed m: mediaPlayer.SetPlaybackSpeed(m.Speed); break;
                case Play _: mediaPlayer.Play(); break;
                case Pause _: mediaPlayer.Pause(); break;
                case LocalFileStreamDisconnect _: mediaPlayer.DisconnectLocalFileStream(); break;
                case StepForward _: mediaPlayer.StepForward(); break;
                case StepBack _: mediaPlayer.StepBack(); break;
                case Seek m: mediaPlayer.Seek(m.Position); break;
                case SetImageOption m: mediaPlayer.SetImageOption(m.Option, m.Value); break;
                case EnableImageOptions m: mediaPlayer.EnableImageOptions(m.Enable); break;
                case EnableHardwareDecoding m: mediaPlayer.EnableHardwareDecoding(m.Enable); break;
                case AddMediaOption m: mediaPlayer.AddMediaOption(m.Option); break;
                case SetMediaTrack m: mediaPlayer.SetMediaTrack(m.TrackType, m.TrackId); break;
                case UnloadMedia _: mediaPlayer.UnloadMedia(); break;
                default: return false;
            }

            responseService.ReturnEmptyResponse(outStream, message);

            return true;
        }
    }
}