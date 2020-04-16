using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class AspectRatioEvent : VideoPlayerEvent
    {
        /// <summary>
        /// The sapect ratio in width / height
        /// </summary>
        /// <param name="aspectRatio"></param>
        public AspectRatioEvent(float aspectRatio)
        {
            AspectRatio = aspectRatio;
        }

        private AspectRatioEvent() { }

        public float AspectRatio { get; private set; }
    }
}
