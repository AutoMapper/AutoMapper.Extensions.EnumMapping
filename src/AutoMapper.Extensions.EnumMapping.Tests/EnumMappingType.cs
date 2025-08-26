using System.Collections.Generic;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class EnumMappingType
    {
        public enum Source
        {
            Default,
            Foo,
            Bar
        }

        public enum Destination
        {
            Default,
            Foo,
            Bar
        }

        public class Default : AutoMapperSpecBase
        {
            readonly List<Destination> _results = new List<Destination>();

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping();
            }, _loggerFactory);

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Source, Destination>(Source.Default));
                _results.Add(Mapper.Map<Source, Destination>(Source.Foo));
                _results.Add(Mapper.Map<Source, Destination>(Source.Bar));
            }

            [Fact]
            public void Should_map_with_default_mappings()
            {
                _results[0].ShouldBe(Destination.Default);
                _results[1].ShouldBe(Destination.Foo);
                _results[2].ShouldBe(Destination.Bar);
            }
        }

        public class ByName : AutoMapperSpecBase
        {
            readonly List<Destination> _results = new List<Destination>();

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByName());
            }, _loggerFactory);

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Source, Destination>(Source.Default));
                _results.Add(Mapper.Map<Source, Destination>(Source.Foo));
                _results.Add(Mapper.Map<Source, Destination>(Source.Bar));
            }

            [Fact]
            public void Should_map_with_default_mappings()
            {
                _results[0].ShouldBe(Destination.Default);
                _results[1].ShouldBe(Destination.Foo);
                _results[2].ShouldBe(Destination.Bar);
            }
        }

        public class ByValue : AutoMapperSpecBase
        {
            readonly List<Destination> _results = new List<Destination>();

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByValue());
            }, _loggerFactory);

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Source, Destination>(Source.Default));
                _results.Add(Mapper.Map<Source, Destination>(Source.Foo));
                _results.Add(Mapper.Map<Source, Destination>(Source.Bar));
            }

            [Fact]
            public void Should_map_with_default_mappings()
            {
                _results[0].ShouldBe(Destination.Default);
                _results[1].ShouldBe(Destination.Foo);
                _results[2].ShouldBe(Destination.Bar);
            }
        }
    }
}
