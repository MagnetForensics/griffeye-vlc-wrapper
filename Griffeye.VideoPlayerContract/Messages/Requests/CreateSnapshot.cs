using ProtoBuf;

namespace Griffeye.VideoPlayerContract.Messages.Requests
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public class CreateSnapshot : BaseRequest
    {
        public int NumberOfVideoOutput { get; set; }
        public int Width { get; private set; }
        public int Height { get; private set; }
        public string FilePath { get; private set; }

        public CreateSnapshot(int numberOfVideoOutput, int width, int height, string filePath) : this()
        {
            NumberOfVideoOutput = numberOfVideoOutput;
            Width = width;
            Height = height;
            FilePath = filePath;
        }

        private CreateSnapshot() {}
    }
}