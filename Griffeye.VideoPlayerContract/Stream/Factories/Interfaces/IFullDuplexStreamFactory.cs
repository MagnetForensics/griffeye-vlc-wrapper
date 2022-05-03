namespace Griffeye.VideoPlayerContract.Stream.Factories.Interfaces;

public interface IFullDuplexStreamFactory
{
    System.IO.Stream CreateInStream();
    System.IO.Stream CreateOutStream();
}