using System.IO.Pipes;
using AutoFixture;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using NSubstitute;

namespace Griffeye.VlcWrapper.Tests.AutoData.Customization
{
    public class StreamFactoryReturnsNullCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var streamFactory = fixture.Freeze<IStreamFactory>();
            
                streamFactory.CreateAnonymousPipeClientStream(Arg.Any<PipeDirection>(), Arg.Any<string>())
                    .ReturnsForAnyArgs((AnonymousPipeClientStream) null);
        }
    }
}