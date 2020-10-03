using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    [ProtoInclude(200, typeof(CreateSnapshot))]
    [ProtoInclude(300, typeof(SetAudioTrack))]
    [ProtoInclude(400, typeof(SetVideoTrack))]
    [ProtoInclude(500, typeof(Load))]
    [ProtoInclude(600, typeof(LocalFileStreamConnect))]
    [ProtoInclude(700, typeof(GetAudioTracks))]
    [ProtoInclude(800, typeof(GetVideoTracks))]
    [ProtoInclude(900, typeof(Seek))]
    [ProtoInclude(1100, typeof(SetMute))]
    [ProtoInclude(1200, typeof(SetPlaybackSpeed))]
    [ProtoInclude(1400, typeof(SetVolume))]
    [ProtoInclude(1600, typeof(Play))]
    [ProtoInclude(1700, typeof(Pause))]
    [ProtoInclude(2000, typeof(LocalFileStreamDisconnect))]
    [ProtoInclude(2100, typeof(Quit))]
    [ProtoInclude(2200, typeof(StepForward))]
    [ProtoInclude(2600, typeof(StepBack))]
    [ProtoInclude(2700, typeof(SetImageOption))]
    [ProtoInclude(2800, typeof(EnableImageOptions))]
    [ProtoInclude(2900, typeof(EnableHardwareDecoding))]
    [ProtoInclude(3000, typeof(AddMediaOption))]

    public class BaseRequest
    {
        public int SequenceNumber { get; set; }
        public BaseRequest() { /*needed for proto-buf in base class and all classes that inherit BaseRequest*/}
    }
}