using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Griffeye.VlcWrapper.Models;
using Microsoft.Extensions.Logging;
using ProtoBuf;
using System;
using System.IO;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace Griffeye.VlcWrapper.Messaging
{
    public class MessageLoop : IMessageLoop
    {
        private readonly IMessageService messageService;
        private readonly ILogger<MessageLoop> logger;
        private readonly IStreamFactory streamFactory;
        private readonly IMessageSerializer messageSerializer;
        private readonly InputData inputData;
        private readonly IEventService eventService;

        public MessageLoop(IMessageService messageService, ILogger<MessageLoop> logger,
            IStreamFactory streamFactory, IMessageSerializer messageSerializer, InputData inputData, IEventService eventService)
        {
         
            this.messageService = messageService;
            this.logger = logger;
            this.streamFactory = streamFactory;
            this.messageSerializer = messageSerializer;
            this.inputData = inputData;
            this.eventService = eventService;
        }

        public async Task Start()
        {
            using var pipeInStream = streamFactory.CreateAnonymousPipeClientStream(PipeDirection.In, inputData.PipeInName);
            using var pipeOutStream = streamFactory.CreateAnonymousPipeClientStream(PipeDirection.Out, inputData.PipeOutName);
            using var pipeEventStream = streamFactory.CreateAnonymousPipeClientStream(PipeDirection.Out, inputData.PipeEventName);

            eventService.Subscribe(pipeEventStream);

            try
            {
                var done = false;

                while (!done)
                {
                    await pipeInStream.ReadAsync(new byte[0], 0, 0);
                    var message = messageSerializer.DeserializeWithLengthPrefix<BaseRequest>(pipeInStream, PrefixStyle.Base128);

                    if (message == null)
                    {
                        throw new InvalidOperationException($"No data in pipe.");
                    }

                    logger.LogDebug("Got message with {Type} and {Sequence}", message.GetType(), message.SequenceNumber);
                    done = messageService.Process(message, pipeEventStream, pipeOutStream);
                }
                logger.LogInformation($"Handled message of Type Quit");
            }
            catch (EndOfStreamException) { logger.LogInformation("End of stream"); }
        }
    }
}