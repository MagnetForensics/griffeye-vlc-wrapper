using System.IO;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using ProtoBuf;

namespace Griffeye.VlcWrapper.Messaging
{
    public class MessageSerializer : IMessageSerializer
    {
        public T DeserializeWithLengthPrefix<T>(Stream stream, PrefixStyle prefixStyle)
        {
            return Serializer.DeserializeWithLengthPrefix<T>(stream, prefixStyle);
        }

        public void SerializeWithLengthPrefix<T>(Stream stream, T message, PrefixStyle prefixStyle)
        {
            Serializer.SerializeWithLengthPrefix(stream, message, prefixStyle);
        }
    }
}