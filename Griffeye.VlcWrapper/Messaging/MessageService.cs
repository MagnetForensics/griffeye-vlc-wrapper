using System;
using System.IO;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Microsoft.Extensions.Logging;

namespace Griffeye.VlcWrapper.Messaging
{
    public class MessageService : IMessageService
    {
        private readonly ILogger<MessageService> logger;
        private readonly IResponseService responseService;
        private readonly IRequestService requestService;      
        private readonly IMediaPlayer mediaPlayer;

        public MessageService(ILogger<MessageService> logger, IResponseService responseService,
            IRequestService requestService, IMediaPlayer mediaPlayer)
        {
            this.logger = logger;
            this.responseService = responseService;
            this.requestService = requestService;
            this.mediaPlayer = mediaPlayer;
        }

        public bool Process(BaseRequest message, Stream eventStream, Stream outStream)
        {
            try
            {
                if (requestService.IsQuitMessage(message)) 
                {
                    responseService.ReturnEmptyResponse(outStream, message);
                    return true; 
                }
                if (requestService.CanHandleMessage(mediaPlayer, message, outStream)) { return false; }

                logger.LogWarning($"Invalid message type: {message.GetType()}");
                responseService.ReturnResultResponse(outStream, message, false);

                throw new InvalidCastException($"Invalid message type: {message.GetType()}");
            }
            catch (InvalidCastException) { }

            return false;
        }
    }
}