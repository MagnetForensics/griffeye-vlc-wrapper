using Griffeye.VideoPlayerContract.Enums;
using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class LogEvent : VideoPlayerEvent
    {
        public LogEvent(LogLevel loglevel, int lineNumber, string file, string message)
        {
            Loglevel = loglevel;
            LineNumber = lineNumber;
            File = file;
            Message = message;
        }

        private LogEvent() { }

        public LogLevel Loglevel { get; private set; }
        public int LineNumber { get; private set; }
        public string File { get; private set; }
        public string Message { get; private set; }
    }
}