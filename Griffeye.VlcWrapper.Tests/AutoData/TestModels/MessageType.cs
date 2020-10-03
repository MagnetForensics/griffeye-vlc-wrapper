namespace Griffeye.VlcWrapper.Tests.AutoData.TestModels
{
    public enum MessageType
    {
        Play, Pause, LocalFileStreamDisconnect, PlaybackSpeed, StepForward, StepBack, Seek,
        Load, LocalFileStreamConnect, CreateSnapshot, Volume, Mute, InvalidCast,
        SetAudioTrack, SetVideoTrack, GetVideoTracks, GetAudioTracks,
        EnableImageOption, EnableHardwareDecoding, SetImageOption, AddMediaOption
    }
}