using Griffeye.VideoPlayerContract.Enums;
using ProtoBuf;
using System;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class SetImageOption : BaseRequest
    {
        public ImageOption Option { get; private set; }

        public float Value { get; private set; }

        public SetImageOption(ImageOption option, float value)
        {
            switch (option)
            {
                case ImageOption.Contrast when value < 0 || value > 2:
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be between [0.0 ... 2.0] for contrast.");
                case ImageOption.Brightness when value < 0 || value > 2:
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be between [0.0 ... 2.0] for brightness.");
                case ImageOption.Hue when value < -180 || value > 180:
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be between [-180 ... 180] for hue.");
                case ImageOption.Saturation when value < 0 || value > 3:
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be between [0.0 ... 3.0] for saturation.");
                case ImageOption.Gamma when value < 0.01 || value > 10:
                    throw new ArgumentOutOfRangeException(nameof(value), "Must be between [0.01 ... 10.0] for gamma.");
                default:
                    break;
            }

            Option = option;
            Value = value;
        }

        private SetImageOption() { }
    }
}