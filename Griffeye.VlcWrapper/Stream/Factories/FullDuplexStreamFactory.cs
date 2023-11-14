using System.IO.Pipes;
using Griffeye.VideoPlayerContract.Stream.Factories;
using Griffeye.VideoPlayerContract.Stream.Factories.Interfaces;
using Griffeye.VlcWrapper.Models;

namespace Griffeye.VlcWrapper.Stream.Factories;

public class FullDuplexStreamFactory : IFullDuplexStreamFactory
{
    private readonly IStreamFactory streamFactory;
    private readonly string pipeInName;
    private readonly string pipeOutName;

    public FullDuplexStreamFactory(InputData data)
    {
        streamFactory = new StreamFactory();
        pipeInName = data.PipeInName;
        pipeOutName = data.PipeOutName;
    }

    public System.IO.Stream CreateInStream()
    {
        return streamFactory.CreateAnonymousPipeClientStream(PipeDirection.In, pipeInName);
    }

    public System.IO.Stream CreateOutStream()
    {
        return streamFactory.CreateAnonymousPipeClientStream(PipeDirection.Out, pipeOutName);
    }
}