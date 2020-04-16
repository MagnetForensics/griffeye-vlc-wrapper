using System.IO;
using Griffeye.VideoPlayerContract.Messages.Events;
using Griffeye.VideoPlayerContract.Messages.Responses;
using ProtoBuf;

namespace Griffeye.VlcWrapper.Messaging.Interfaces
{
    public interface IMessageSerializer
    {
        public T DeserializeWithLengthPrefix<T>(Stream stream, PrefixStyle prefixStyle);
        public void SerializeWithLengthPrefix<T>(Stream stream, T message, PrefixStyle prefixStyle);
    }
}