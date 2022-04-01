using System.ComponentModel;
using System.Diagnostics;
using System.IO.Pipes;
using System.Reflection;

namespace Griffeye.VideoPlayerForms.Test
{
    public sealed class SubProcessHost : IDisposable
    {
        public event EventHandler<EventArgs>? Exited;
        
        public AnonymousPipeServerStream PipeSend { get; }
        public AnonymousPipeServerStream PipeReceive { get; }
        public AnonymousPipeServerStream PipeEvent { get; }
        public bool IsRunning => !hasDisposed && process is { HasExited: false };
        
        private readonly Process process;
        private bool hasDisposed;

        public SubProcessHost()
        {
            PipeSend = new AnonymousPipeServerStream(PipeDirection.Out, HandleInheritability.Inheritable);
            PipeReceive = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);
            PipeEvent = new AnonymousPipeServerStream(PipeDirection.In, HandleInheritability.Inheritable);
            process = new Process { EnableRaisingEvents = true };
            process.Exited += (_, _) => Exited?.Invoke(this, EventArgs.Empty);
        }

        public void Start(IntPtr windowHandle)
        {
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.WorkingDirectory = AssemblyDirectory;
            process.StartInfo.FileName = GetVideoSubProcessPath();

            const string logLevel = "Debug";
            var logPath = Path.Combine(AssemblyDirectory!, "VideoPlayer.txt");
            var waitForDebugger = "false";

            if (Debugger.IsAttached)
            {
                waitForDebugger = "true";
            }
            
            process.StartInfo.Arguments =
                $"Handle={windowHandle} " +
                $"PipeInName={PipeSend.GetClientHandleAsString()} " +
                $"PipeOutName={PipeReceive.GetClientHandleAsString()} " +
                $"PipeEventName={PipeEvent.GetClientHandleAsString()} " +
                $"Serilog:MinimumLevel={logLevel} " +
                $"Serilog:WriteTo:0:Args:configure:0:Args:path=\"{logPath}\" " +
                $"WaitForDebugger={waitForDebugger}";

            process.Start();
            
            if (process.HasExited)
            {
                throw new Exception("Video sub process failed to start");
            }
        }

        private static string GetVideoSubProcessPath()
        {
            return Path.Combine(AssemblyDirectory!, "Griffeye.VlcWrapper.exe");
        }

        private static string? AssemblyDirectory
        {
            get
            {
                var codeBase = Assembly.GetExecutingAssembly().Location;
                var uri = new UriBuilder(codeBase);
                var path = Uri.UnescapeDataString(uri.Path);

                return Path.GetDirectoryName(path);
            }
        }

        public void Dispose()
        {
            hasDisposed = true;
            PipeSend.Dispose();
            PipeReceive.Dispose();
            PipeEvent.Dispose();

            try
            {
                process.Kill();
            }
            catch (InvalidOperationException) { /*Process has exited. */ }
            catch (Win32Exception) { /*Process is terminating or could not be terminated.*/ }

            process.Dispose();
        }
    }
}