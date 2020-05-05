using System.IO;
using System.IO.Pipes;
using AutoFixture;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using NSubstitute;

namespace Griffeye.VlcWrapper.Tests.AutoData.Customization
{
    public class StreamFactoryReturnsMemoryStreamCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var streamFactory = fixture.Freeze<IStreamFactory>();
            
                streamFactory.CreateAnonymousPipeClientStream(Arg.Any<PipeDirection>(), Arg.Any<string>())
                    .ReturnsForAnyArgs(new MemoryStream());
        }
    }
}