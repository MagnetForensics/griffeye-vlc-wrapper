using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Griffeye.VlcWrapper.Factories;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using Griffeye.VlcWrapper.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace Griffeye.VlcWrapper
{
    class Program
    {
        private static async Task<int> Main(string[] args)
        {            
            var config = SetupConfiguration(args);
            var serviceProvider = RegisterServices(config);
            var input = serviceProvider.GetService<InputData>();
            
            if (input.AttachDebugger) {Debugger.Launch(); }
            
           Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            var logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            logger.LogInformation("Video player started.");

            try
            {
                var messageLoop = serviceProvider.GetService<IMessageLoop>();
                await messageLoop.Start();

                return 0;
            }
            catch (Exception ex) { logger.LogError(ex, ex.Message); }
            finally 
            {
                logger.LogInformation("Video player exited.");
                Log.CloseAndFlush();
                DisposeServices(serviceProvider);
            }
            return -1;
        }

        private static IConfiguration SetupConfiguration(string[] args)
        {
            return new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddCommandLine(args)
                .Build();
        }

        private static IServiceProvider RegisterServices(IConfiguration config)
        {
            return new ServiceCollection()
                .AddLogging(configure => configure.AddSerilog())
                .Configure<InputData>(config)
                .AddSingleton(r => r.GetRequiredService<IOptions<InputData>>().Value)
                .AddSingleton<IMediaPlayer, VLCMediaPlayer>()
                .AddSingleton<IMessageLoop, MessageLoop>()
                .AddSingleton<IMessageService, MessageService>()
                .AddSingleton<IResponseService, ResponseService>()
                .AddSingleton<IRequestService, RequestService>()
                .AddSingleton<IEventService, EventService>()
                .AddSingleton<IStreamFactory, StreamFactory>()
                .AddSingleton<IMessageSerializer, MessageSerializer>()
                .BuildServiceProvider();
        }

        private static void DisposeServices(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) { return; }
            if (serviceProvider is IDisposable disposable) { disposable.Dispose(); }
        }
    }
}