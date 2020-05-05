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

            logger.LogInformation($"Returning empty response with sequence number: {message.SequenceNumber}");
            messageSerializer.SerializeWithLengthPrefix(outStream, response, PrefixStyle.Base128);
        }

        public void ReturnResultResponse(Stream outStream, BaseRequest message, bool success)
        {
            var response = new ResultResponse(message.SequenceNumber, success);

            logger.LogInformation(
                $"Returning result response with sequence number: {message.SequenceNumber} and success code: {success}");
            messageSerializer.SerializeWithLengthPrefix(outStream, response, PrefixStyle.Base128);
        }

        public void ReturnResultResponse(Stream outStream, BaseRequest message, List<(int, string)> tracks)
        {
            var response = new TracksResponse(tracks, message.SequenceNumber);

            logger.LogInformation(
                $"Returning result response with sequence number: {message.SequenceNumber} and result: {tracks}");
            messageSerializer.SerializeWithLengthPrefix(outStream, response, PrefixStyle.Base128);
        }
    }
}