using Griffeye.VideoPlayerContract.Enums;
using Griffeye.VideoPlayerContract.Messages.Requests;
using Griffeye.VlcWrapper.Tests.AutoData;
using NUnit.Framework;
using System;

namespace Griffeye.VlcWrapper.Tests.Tests
{
    [TestFixture]
    public class SetImageOptionTests
    {
        [Theory, ImageOptionData(true)]
        public void ThrowIfInvalidInput(ImageOption option, float value)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new SetImageOption(option, value));
        }

        [Theory, ImageOptionData]
        public void DontThrowIfValidInput(ImageOption option, float value)
        {
            Assert.DoesNotThrow(() => new SetImageOption(option, value));
        }
    }
}