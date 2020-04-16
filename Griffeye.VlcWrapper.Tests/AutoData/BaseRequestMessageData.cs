using System;
using Griffeye.VlcWrapper.Tests.AutoData.Customization;
using Griffeye.VlcWrapper.Tests.AutoData.TestModels;

namespace Griffeye.VlcWrapper.Tests.AutoData
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class BaseRequestMessageData : AutoNSubstituteData
    {
        public BaseRequestMessageData(MessageType messageType) : base(fixture =>
        {
            fixture.Customize(new BaseRequestMessageTypeCustomization {MessageType = messageType});
        })
        {
        }
    }
}