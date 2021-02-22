using System.IO;
using AutoFixture.NUnit3;
using FluentAssertions;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Griffeye.VlcWrapper.Tests.AutoData;
using Griffeye.VlcWrapper.Tests.AutoData.TestModels;
using NSubstitute;
using NUnit.Framework;

namespace Griffeye.VlcWrapper.Tests.Tests
{
    public class MessageServiceTests
    {
        public class ProcessShould
        {            
            [Theory, AutoNSubstituteData]
            public void CheckIfQuitMessageUsingIRequestService([Frozen]IRequestService requestService, [Frozen]Stream eventStream, [Frozen]BaseRequest message, Stream outStream, MessageService sut)
            {
                sut.Process(message, eventStream, outStream);
                requestService.Received(1).IsQuitMessage(message);
            }
            
            [Theory, AutoNSubstituteData]
            public void ReturnTrueIfQuitMessage([Frozen]Stream eventStream, [Frozen]Quit message, Stream outStream, MessageService sut)
            {
                sut.Process(message, eventStream, outStream).Should()
                    .BeTrue("Because we should stop process messages after a quit message.");
            }
            
            [Theory, MessageServiceData]
            public void CheckIfValidMessageUsingIRequestService([Frozen]IRequestService requestService,
                Stream eventStream, BaseRequest message, Stream outStream, MessageService sut)
            {
                sut.Process(message, eventStream, outStream);
                requestService.Received(1)
                    .CanHandleMessage(Arg.Any<IMediaPlayer>(), Arg.Any<BaseRequest>(), Arg.Any<Stream>());
            }
            
            [Theory, MessageServiceData(canHandleMessage: true)]
            public void ReturnFalseIfValidMessage(Stream eventStream, BaseRequest message, Stream outStream,
                MessageService sut)
            {
                sut.Process(message, eventStream, outStream)
                    .Should().BeFalse("Because we want to process more messages.");
            }
            
            [Theory, MessageServiceData]
            public void CallReturnResultResponseOnIResponseService([Frozen]IResponseService responseService,
                [Frozen]Stream eventStream, [Frozen]BaseRequest message, [Frozen]Stream outStream, MessageService sut)
            {
                sut.Process(message, eventStream, outStream);
                responseService.Received(1).ReturnResultResponse(outStream, message, false);
            }
            
            [Theory, MessageServiceData]
            public void ReturnFalseIfInvalidMessage([Frozen]Stream eventStream, [Frozen]InvalidCastMessage message, 
                [Frozen]Stream outStream, MessageService sut)
            {
                sut.Process(message, eventStream, outStream).Should().BeFalse();
            }
        }
    }
}