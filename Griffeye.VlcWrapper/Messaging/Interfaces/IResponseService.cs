using System.Collections.Generic;
using System.IO;
using Griffeye.VideoPlayerContract.Messages.Requests;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IResponseService
    {
        public void ReturnEmptyResponse(Stream outStream, BaseRequest message);
        public void ReturnResultResponse(Stream outStream, BaseRequest message, bool success);
        public void ReturnResultResponse(Stream outStream, BaseRequest message, List<(int, string)> tracks);
    }
}