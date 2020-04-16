using AutoFixture;
using Griffeye.VlcWrapper.Models;

namespace Griffeye.VlcWrapper.Tests.AutoData.Customization
{
    internal class InputDataCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            var inputData = new InputData
            {
                Handle = fixture.Create<long>(),
                PipeEventName = fixture.Create<string>(),
                PipeInName = fixture.Create<string>(),
                PipeOutName = fixture.Create<string>()
            };

            fixture.Inject(inputData);
        }
    }
}