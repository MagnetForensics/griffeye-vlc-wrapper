using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Griffeye.VlcWrapper.Models;
using Microsoft.Extensions.Logging;
using ProtoBuf;
using System;
using System.IO;
using System.IO.Pipes;

namespace Griffeye.VlcWrapper.Messaging
{
    public class MessageLoop : IMessageLoop
    {
        private readonly IMessageService messageService;
        private readonly ILogger<MessageLoop> logger;
        private readonly IStreamFactory streamFactory;
        private readonly IMessageSerializer messageSerializer;
        private readonly InputData inputData;

        public MessageLoop(IMessageService messageService, ILogger<MessageLoop> logger,
            IStreamFactory streamFactory, IMessageSerializer messageSerializer, InputData inputData)
        {
         
            this.messageService = messageService;
            this.logger = logger;
            this.streamFactory = streamFactory;
            this.messageSerializer = messageSerializer;
            this.inputData = inputData;
        }

        public void Start()
        {
            using var pipeInStream = streamFactory.CreateAnonymousPipeClientStream(PipeDirection.In, inputData.PipeInName);
            using var pipeOutStream = streamFactory.CreateAnonymousPipeClientStream(PipeDirection.Out, inputData.PipeOutName);
            using var pipeEventStream = streamFactory.CreateAnonymousPipeClientStream(PipeDirection.Out, inputData.PipeEventName);
            
            try
            {
                var done = false;

                while (!done)
                {
                    var message = messageSerializer
                        .DeserializeWithLengthPrefix<BaseRequest>(pipeInStream, PrefixStyle.Base128);

                    if (message == null)
                    {
                        throw new InvalidOperationException("Unknown command from video player sub process.");
                    }

                    done = messageService.Process(message, pipeEventStream, pipeOutStream);
                }
            }
            catch (EndOfStreamException) { logger.LogInformation("End of stream"); }
        }
    }
}