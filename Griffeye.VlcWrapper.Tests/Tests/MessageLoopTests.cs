using System;
using System.IO;
using System.IO.Pipes;
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
            public void CreatesAnonymousPipes([Frozen]IStreamFactory streamFactory, InputData input, MessageLoop sut)
            {
                sut.Start();
                streamFactory.Received(1).CreateAnonymousPipeClientStream(PipeDirection.In, input.PipeInName);
                streamFactory.Received(1).CreateAnonymousPipeClientStream(PipeDirection.Out, input.PipeOutName);
                streamFactory.Received(1).CreateAnonymousPipeClientStream(PipeDirection.Out, input.PipeEventName);
            }
            
            [Theory, MessageLoopData]
            public void UseIMessageSerializer([Frozen]IMessageSerializer messageSerializer,  MessageLoop sut)
            {
                sut.Start();
                messageSerializer
                    .Received(1)
                    .DeserializeWithLengthPrefix<BaseRequest>(Arg.Any<Stream>(), PrefixStyle.Base128);
            }
            
            [Theory, MessageLoopData(true)]
            public void ThrowInvalidOperationExceptionIfMessageIsNull([Frozen] IMessageService messageService,
                [Frozen] BaseRequest message, MessageLoop sut)
            {
                sut.Invoking(x => x.Start())
                    .Should()
                    .Throw<InvalidOperationException>();
            }

            [Theory, MessageLoopData]
            public void ProcessMessages([Frozen]IMessageService messageService, [Frozen]BaseRequest message,
                MessageLoop sut)
            {
                sut.Start();
                messageService.Received(1).Process(message, Arg.Any<Stream>(), Arg.Any<Stream>());
            }
        }
    }
}