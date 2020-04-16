using Griffeye.VlcWrapper.Tests.AutoData.Customization;

namespace Griffeye.VlcWrapper.Tests.AutoData
{
    public class MessageServiceData : AutoNSubstituteData
    {
        public MessageServiceData(bool isQuitMessage = false, bool canHandleMessage = false) : base(fixture =>
        {
            fixture.Customize(new RequestServiceCustomization
            {
                IsQuitMessage = isQuitMessage,
                CanHandleMessage = canHandleMessage
            });
        })
        {
        }
    }
}