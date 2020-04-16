using AutoFixture;
using AutoFixture.AutoNSubstitute;
using AutoFixture.NUnit3;
using System;


namespace Griffeye.VlcWrapper.Tests.AutoData
{
    public class AutoNSubstituteData : AutoDataAttribute
    {
        public AutoNSubstituteData(Action<IFixture> initialize) : base(() =>
        {
            var fixture = new Fixture();

            fixture.Customize(new AutoNSubstituteCustomization { GenerateDelegates = true, ConfigureMembers = true });
            initialize(fixture);

            return fixture;
        })
        {
        }

        public AutoNSubstituteData() : this(fixture => { })
        {
        }
    }
}