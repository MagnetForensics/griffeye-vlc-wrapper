namespace Griffeye.VlcWrapper.Models
{
    public class InputData
    {
        public long Handle { get; set; }
        public string PipeInName { get; set; }
        public string PipeOutName { get; set; }
        public string PipeEventName { get; set; }
        public Serilog Serilog { get; set; }
    }

    public class Serilog
    {
        public string MinimumLevel { get; set; }
    }
}