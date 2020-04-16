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
        private readonly IEventService eventService;
        private readonly IMediaPlayer mediaPlayer;

        public MessageService(ILogger<MessageService> logger, IResponseService responseService,
            IRequestService requestService, IEventService eventService, IMediaPlayer mediaPlayer)
        {
            this.logger = logger;
            this.responseService = responseService;
            this.requestService = requestService;
            this.eventService = eventService;
            this.mediaPlayer = mediaPlayer;
        }

        public bool Process(BaseRequest message, Stream eventStream, Stream outStream)
        {
            eventService.Subscribe(mediaPlayer, eventStream);
            
            try
            {
                if (requestService.IsQuitMessage(message)) { return true; }
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