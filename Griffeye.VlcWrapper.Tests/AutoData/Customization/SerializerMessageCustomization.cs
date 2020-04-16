using System.IO;
using AutoFixture;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using NSubstitute;
using ProtoBuf;

namespace Griffeye.VlcWrapper.Tests.AutoData.Customization
{
    public class SerializerMessageCustomization : ICustomization
    {
        public bool ReturnsNull { get; set; }
        public void Customize(IFixture fixture)
        {
            if (!ReturnsNull) return;
            
            var serializer = fixture.Freeze<IMessageSerializer>();

            serializer
                .DeserializeWithLengthPrefix<BaseRequest>(Arg.Any<Stream>(), Arg.Any<PrefixStyle>())
                .ReturnsForAnyArgs((BaseRequest)null);
        }
    }
}