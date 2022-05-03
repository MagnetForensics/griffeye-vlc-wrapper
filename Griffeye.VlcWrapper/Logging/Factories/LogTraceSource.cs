using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Griffeye.VlcWrapper.Logging.Factories;

public class LogTraceSource : TraceListener
{
    private readonly ILogger logger;

    public LogTraceSource(ILogger logger)
    {
        this.logger = logger;
    }
    
    public override void Write(string message)
    {
        logger.LogInformation("RPC - {Message}", message);
    }

    public override void WriteLine(string message)
    {
        logger.LogInformation("RPC - {Message}", message);
    }
}