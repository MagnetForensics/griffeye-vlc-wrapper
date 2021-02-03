using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Events
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class MediaInfoEvent : VideoPlayerEvent
    {
        public float AspectRatio { get; private set; }
        public string Orientation { get; private set; }

        /// <summary>
        /// The aspect ratio in width / height and the video orientation
        /// </summary>
        /// <param name="aspectRatio"></param>
        /// <param name="orientation"></param>
        public MediaInfoEvent(float aspectRatio, string orientation)
        {
            AspectRatio = aspectRatio;
            Orientation = orientation;
        }

        private MediaInfoEvent() { }
    }
}