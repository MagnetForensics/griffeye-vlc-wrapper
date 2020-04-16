using System.IO;
using AutoFixture;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.MediaPlayer;
using Griffeye.VlcWrapper.Messaging.Interfaces;
using NSubstitute;

namespace Griffeye.VlcWrapper.Tests.AutoData.Customization
{
    public class RequestServiceCustomization : ICustomization
    {
        public bool IsQuitMessage { get; set; }
        public bool CanHandleMessage { get; set; }
        public void Customize(IFixture fixture)
        {
            var requestService = fixture.Freeze<IRequestService>();
            
            requestService.IsQuitMessage(Arg.Any<BaseRequest>()).ReturnsForAnyArgs(IsQuitMessage);
            requestService.CanHandleMessage(Arg.Any<IMediaPlayer>(), Arg.Any<BaseRequest>(), Arg.Any<Stream>())
                .ReturnsForAnyArgs(CanHandleMessage);
        }
    }
}