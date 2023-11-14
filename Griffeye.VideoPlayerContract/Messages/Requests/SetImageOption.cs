using Griffeye.VideoPlayerContract.Enums;
using System;
using System.Runtime.Serialization;

namespace Griffeye.VideoPlayerContract.Messages.Requests;

[DataContract]
public class SetImageOption
{
    [DataMember]
    public ImageOption Option { get; }

    [DataMember]
    public float Value { get; }

    public SetImageOption(ImageOption option, float value)
    {
        switch (option)
        {
            case ImageOption.Contrast when value is < 0 or > 2:
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between [0.0 ... 2.0] for contrast.");
            case ImageOption.Brightness when value is < 0 or > 2:
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between [0.0 ... 2.0] for brightness.");
            case ImageOption.Hue when value is < -180 or > 180:
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between [-180 ... 180] for hue.");
            case ImageOption.Saturation when value is < 0 or > 3:
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between [0.0 ... 3.0] for saturation.");
            case ImageOption.Gamma when value < 0.01 || value > 10:
                throw new ArgumentOutOfRangeException(nameof(value), "Must be between [0.01 ... 10.0] for gamma.");
            default:
                break;
        }

        Option = option;
        Value = value;
    }
}