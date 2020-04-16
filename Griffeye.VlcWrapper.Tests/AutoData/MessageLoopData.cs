using Griffeye.VlcWrapper.Tests.AutoData.Customization;

namespace Griffeye.VlcWrapper.Tests.AutoData
{
    public class MessageLoopData : AutoNSubstituteData
    {
        public MessageLoopData(bool messageIsNull = false, bool messageServiceReturns = true) : base(fixture =>
        {
            fixture
                .Customize(new InputDataCustomization())
                .Customize(new StreamFactoryReturnsNullCustomization())
                .Customize(new SerializerMessageCustomization {ReturnsNull = messageIsNull})
                .Customize(new MessageServiceCustomization {Returns = messageServiceReturns});
        })
        {
        }
    }
}