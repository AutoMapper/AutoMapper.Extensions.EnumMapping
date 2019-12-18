using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class EnumValueWithOtherUnderlyingTypeMapping : AutoMapperSpecBase
    {
        Destination _destination;
        public enum Source : byte { Default, Foo, Bar }
        public enum Destination : byte { Default, Bar, Foo }

        protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
        {
            cfg.EnableEnumMappingValidation();
            cfg.CreateMap<Source, Destination>()
                .ConvertUsingEnumMapping(opt => opt.MapByValue());
        });

        protected override void Because_of()
        {
            _destination = Mapper.Map<Source, Destination>(Source.Bar);
        }

        [Fact]
        public void Should_map_by_underlying_type()
        {
            _destination.ShouldBe(Destination.Foo);
        }
    }
}
