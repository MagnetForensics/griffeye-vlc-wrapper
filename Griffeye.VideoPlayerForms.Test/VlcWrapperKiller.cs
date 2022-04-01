using System.Diagnostics;

namespace Griffeye.VideoPlayerForms.Test;

public class VlcWrapperKiller
{
    private const string ProcessName = "Griffeye.VlcWrapper";
    
    public static void KillAll()
    {
        try
        {
            var workers = Process.GetProcessesByName(ProcessName);
            foreach (var worker in workers)
            {
                worker.Kill();
                worker.WaitForExit();
                worker.Dispose();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine($@"Failed to kill process {e.Message}");
        }
    }
}