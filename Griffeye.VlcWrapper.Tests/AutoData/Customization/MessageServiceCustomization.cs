using System.IO;
using AutoFixture;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using NSubstitute;

namespace Griffeye.VlcWrapper.Tests.AutoData.Customization
{
    public class MessageServiceCustomization : ICustomization
    {
        public bool Returns { get; set; }
        public void Customize(IFixture fixture)
        {
            var messageProcessor = fixture.Create<IMessageService>();

            messageProcessor.Process(Arg.Any<BaseRequest>(), Arg.Any<Stream>(),
                Arg.Any<Stream>()).ReturnsForAnyArgs(Returns);
        }
    }
}