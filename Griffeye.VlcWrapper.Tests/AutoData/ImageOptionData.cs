using Griffeye.VlcWrapper.Tests.AutoData.Customization;
using System;

namespace Griffeye.VlcWrapper.Tests.AutoData
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ImageOptionData : AutoNSubstituteData
    {
        public ImageOptionData(bool shouldThrowOutOfRange = false) : base(fixture =>
        {
            fixture.Customize(new ImageOptionCustomization { ShouldThrowOutOfRange = shouldThrowOutOfRange });
        })
        {
        }
    }
}
