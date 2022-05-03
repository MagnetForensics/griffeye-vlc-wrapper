using System.Diagnostics;
using Griffeye.VideoPlayerContract.Logging.Interfaces;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Griffeye.VlcWrapper.Logging.Factories;

public class RpcLoggerFactory : IRpcLoggerFactory
{
    private readonly ILogger logger;

    public RpcLoggerFactory(ILogger<RpcLoggerFactory> logger)
    {
        this.logger = logger;
    }

    public TraceListener Create()
    {
        return new LogTraceSource(logger);
    }

    public bool IsDebug() => Log.IsEnabled(LogEventLevel.Debug);
}