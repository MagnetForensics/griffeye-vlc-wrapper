using System;
using System.Threading.Tasks;

namespace Griffeye.VideoPlayerContract.MediaPlayer.Server.Interfaces;

public interface IRpcServer : IDisposable
{
    event EventHandler<string> Exited;
    Task StartAsync();
}
