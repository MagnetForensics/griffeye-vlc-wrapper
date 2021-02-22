using AutoFixture;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.Tests.AutoData.TestModels;
using System;

namespace Griffeye.VlcWrapper.Tests.AutoData.Customization
{
    public class BaseRequestMessageTypeCustomization : ICustomization
    {
        public MessageType MessageType { get; set; }

        public void Customize(IFixture fixture)
        {
            switch (MessageType)
            {
                case MessageType.Play: fixture.Register<BaseRequest>(fixture.Create<Play>); break;
                case MessageType.Pause: fixture.Register<BaseRequest>(fixture.Create<Pause>); break;
                case MessageType.LocalFileStreamDisconnect:
                    fixture.Register<BaseRequest>(fixture.Create<LocalFileStreamDisconnect>); break;
                case MessageType.PlaybackSpeed: fixture.Register<BaseRequest>(fixture.Create<SetPlaybackSpeed>); break;
                case MessageType.StepForward: fixture.Register<BaseRequest>(fixture.Create<StepForward>); break;
                case MessageType.StepBack: fixture.Register<BaseRequest>(fixture.Create<StepBack>); break;
                case MessageType.Seek: fixture.Register<BaseRequest>(fixture.Create<Seek>); break;
                case MessageType.Load: fixture.Register<BaseRequest>(fixture.Create<Load>); break;
                case MessageType.LocalFileStreamConnect:
                    fixture.Register<BaseRequest>(fixture.Create<LocalFileStreamConnect>); break;
                case MessageType.CreateSnapshot: fixture.Register<BaseRequest>(fixture.Create<CreateSnapshot>); break;
                case MessageType.Volume: fixture.Register<BaseRequest>(fixture.Create<SetVolume>); break;
                case MessageType.Mute: fixture.Register<BaseRequest>(fixture.Create<SetMute>); break;
                case MessageType.InvalidCast: fixture.Register<BaseRequest>(fixture.Create<InvalidCastMessage>); break;
                case MessageType.EnableImageOption: fixture.Register<BaseRequest>(fixture.Create<EnableImageOptions>); break;
                case MessageType.EnableHardwareDecoding: fixture.Register<BaseRequest>(fixture.Create<EnableHardwareDecoding>); break;
                case MessageType.SetImageOption:
                    fixture.Customize(new ImageOptionCustomization());
                    fixture.Register<BaseRequest>(fixture.Create<SetImageOption>);
                    break;
                case MessageType.AddMediaOption: fixture.Register<BaseRequest>(fixture.Create<AddMediaOption>); break;
                case MessageType.SetMediaTrack: fixture.Register<BaseRequest>(fixture.Create<SetMediaTrack>); break;
                case MessageType.UnloadMedia: fixture.Register<BaseRequest>(fixture.Create<UnloadMedia>); break;
                default: throw new ArgumentOutOfRangeException();
            }

        }
    }
}