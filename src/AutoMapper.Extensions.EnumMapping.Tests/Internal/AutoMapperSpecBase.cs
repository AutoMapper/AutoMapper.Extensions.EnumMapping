using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests.Internal
{
    public abstract class AutoMapperSpecBase : NonValidatingSpecBase
    {
        [Fact]
        public void Should_have_valid_configuration()
        {
            Configuration.AssertConfigurationIsValid();
        }

    }
}
