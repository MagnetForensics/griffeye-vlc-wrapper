using System;
using AutoFixture;
using Griffeye.VideoPlayerContract.Enums;

namespace Griffeye.VlcWrapper.Tests.AutoData.Customization
{
    public class ImageOptionCustomization : ICustomization
    {
        private static readonly Random random = new Random();
        public bool ShouldThrowOutOfRange { get; set; }

        public void Customize(IFixture fixture)
        {
            var imageOption = fixture.Create<ImageOption>();
            var value = GetRandomImageOptionValue(ShouldThrowOutOfRange, imageOption);
            fixture.Inject(value);
            fixture.Inject(imageOption);
        }

        private static double NextInRange(double min, double max) => random.NextDouble() * (max - min) + min;

        private static double NextOutOfRange(double start, double stop)
            => random.Next(0, 1) == 1 ? NextInRange(double.MinValue, start) : NextInRange(stop, double.MaxValue);

        private static float GetRandomImageOptionValue(bool shouldThrowOutOfRange, ImageOption imageOption)
            => imageOption switch
            {
                ImageOption.Contrast => shouldThrowOutOfRange ? (float)NextOutOfRange(0, 2) : (float)NextInRange(0, 2),
                ImageOption.Brightness => shouldThrowOutOfRange ? (float)NextOutOfRange(0, 2) : (float)NextInRange(0, 2),
                ImageOption.Hue => shouldThrowOutOfRange ? (float)NextOutOfRange(-180, 180) : random.Next(-180, 180),
                ImageOption.Saturation => shouldThrowOutOfRange ? (float)NextOutOfRange(0, 3) : (float)NextInRange(0, 3),
                ImageOption.Gamma => shouldThrowOutOfRange ? (float)NextOutOfRange(0.01, 10.0) : (float)NextInRange(0.01, 10.0),
                _ => 0f
            };
    }

}
