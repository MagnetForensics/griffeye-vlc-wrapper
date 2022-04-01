using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.Messaging;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Griffeye.VlcWrapper.Models;
using Griffeye.VlcWrapper.Tests.AutoData;
using NSubstitute;
using NUnit.Framework;
using ProtoBuf;

namespace Griffeye.VlcWrapper.Tests.Tests
{
    public class MessageLoopTests
    {
        [TestFixture]
        public class StartShould
        {
            [Theory, MessageLoopData]
            public async Task SubscribeToMediaPlayerEventsUsingIEventService([Frozen]IEventService eventService, MessageLoop sut)
            {
                await sut.Start();
                eventService.Received(1).Subscribe(Arg.Any<Stream>());
            }

            [Theory, MessageLoopData]
            public async Task CreatesAnonymousPipes([Frozen]IStreamFactory streamFactory, InputData input, MessageLoop sut)
            {
                await sut.Start();
                streamFactory.Received(1).CreateAnonymousPipeClientStream(PipeDirection.In, input.PipeInName);
                streamFactory.Received(1).CreateAnonymousPipeClientStream(PipeDirection.Out, input.PipeOutName);
                streamFactory.Received(1).CreateAnonymousPipeClientStream(PipeDirection.Out, input.PipeEventName);
            }
            
            [Theory, MessageLoopData]
            public async Task UseIMessageSerializer([Frozen]IMessageSerializer messageSerializer,  MessageLoop sut)
            {
                await sut.Start();
                messageSerializer
                    .Received(1)
                    .DeserializeWithLengthPrefix<BaseRequest>(Arg.Any<Stream>(), PrefixStyle.Base128);
            }
            
            [Theory, MessageLoopData(true)]
            public void ThrowInvalidOperationExceptionIfMessageIsNull(MessageLoop sut)
            {
                sut.Invoking(x => x.Start())
                    .Should()
                    .ThrowAsync<InvalidOperationException>();
            }

            [Theory, MessageLoopData]
            public async Task ProcessMessages([Frozen]IMessageService messageService, [Frozen]BaseRequest message,
                MessageLoop sut)
            {
                await sut.Start();
                messageService.Received(1).Process(message, Arg.Any<Stream>(), Arg.Any<Stream>());
            }
        }
    }
}