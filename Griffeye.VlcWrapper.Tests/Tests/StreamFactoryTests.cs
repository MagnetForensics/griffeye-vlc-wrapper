using System.IO;
using System.IO.Pipes;
using FluentAssertions;
using Griffeye.VideoPlayerContract.Stream.Factories;
using Griffeye.VlcWrapper.Tests.AutoData;
using NUnit.Framework;

namespace Griffeye.VlcWrapper.Tests.Tests
{
    [TestFixture]
    public class StreamFactoryTests
    {
        public class CreateAnonymousPipeClientStreamShould
        {
            [Theory, AutoNSubstituteData]
            public void CreateNewAnonymousPipeClientStream(StreamFactory sut, PipeDirection clientDirection,
                PipeDirection serverDirection)
            {
                var handle = new AnonymousPipeServerStream(serverDirection, HandleInheritability.Inheritable);
                
                sut.CreateAnonymousPipeClientStream(clientDirection, handle.GetClientHandleAsString())
                .Should().NotBeNull().And.BeAssignableTo<AnonymousPipeClientStream>();
            }
        }
    }
}