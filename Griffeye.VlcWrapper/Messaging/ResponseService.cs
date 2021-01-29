using System.Collections.Generic;
using System.IO;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VideoPlayerContract.Messages.Responses;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Microsoft.Extensions.Logging;
using ProtoBuf;

namespace Griffeye.VlcWrapper.Messaging
{
    public class ResponseService : IResponseService
    {
        private readonly IMessageSerializer messageSerializer;
        private readonly ILogger<ResponseService> logger;

        public ResponseService(IMessageSerializer messageSerializer, ILogger<ResponseService> logger)
        {
            this.messageSerializer = messageSerializer;
            this.logger = logger;
        }

        public void ReturnEmptyResponse(Stream outStream, BaseRequest message)
        {
            var response = new BaseResponse(message.SequenceNumber);

            logger.LogDebug("Returning empty response with {Sequence}", message.SequenceNumber);
            messageSerializer.SerializeWithLengthPrefix(outStream, response, PrefixStyle.Base128);
        }

        public void ReturnResultResponse(Stream outStream, BaseRequest message, bool success)
        {
            var response = new ResultResponse(message.SequenceNumber, success);

            logger.LogDebug(
                "Returning result response with {Sequence} and {Result}", message.SequenceNumber, success);
            messageSerializer.SerializeWithLengthPrefix(outStream, response, PrefixStyle.Base128);
        }
    }
}