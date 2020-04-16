using System.Collections.Generic;
using System.IO;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VideoPlayerContract.Messages.Responses;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using ProtoBuf;

namespace Griffeye.VlcWrapper.Messaging
{
    public class ResponseService : IResponseService
    {
        private readonly IMessageSerializer messageSerializer;

        public ResponseService(IMessageSerializer messageSerializer)
        {
            this.messageSerializer = messageSerializer;
        }
        public void ReturnEmptyResponse(Stream outStream, BaseRequest message)
        {
            var response = new BaseResponse(message.SequenceNumber);
            
            messageSerializer.SerializeWithLengthPrefix(outStream, response, PrefixStyle.Base128);
        }

        public void ReturnResultResponse(Stream outStream, BaseRequest message, bool success)
        {
            var response = new ResultResponse(message.SequenceNumber, success);

            messageSerializer.SerializeWithLengthPrefix(outStream, response, PrefixStyle.Base128);
        }
        
        public void ReturnResultResponse(Stream outStream, BaseRequest message, List<(int, string)> tracks)
        {
            var response = new TracksResponse(tracks, message.SequenceNumber);

            messageSerializer.SerializeWithLengthPrefix(outStream, response, PrefixStyle.Base128);
        }
    }
}