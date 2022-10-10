using AutoMapper.Extensions.EnumMapping.Tests.Internal;

namespace AutoMapper.Extensions.EnumMapping.Tests;

public class EnumMappingValidation
{
    public class Default : AutoMapperSpecBase
    {
        protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
        {
            cfg.EnableEnumMappingValidation();
            cfg.CreateMap<object, object>();
        });
    }
}
