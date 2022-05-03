using System.Diagnostics;

namespace Griffeye.VideoPlayerContract.Logging.Interfaces;

public interface IRpcLoggerFactory
{
    TraceListener Create();
    bool IsDebug();
}