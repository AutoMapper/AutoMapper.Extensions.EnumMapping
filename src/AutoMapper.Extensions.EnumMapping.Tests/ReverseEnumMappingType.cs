using System.Collections.Generic;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class ReverseEnumMappingType
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
            readonly List<Source> _results = new List<Source>();

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping()
                    .ReverseMap();
            });

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.Default));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Foo));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Bar));
            }

            [Fact]
            public void Should_map_with_default_mappings()
            {
                _results[0].ShouldBe(Source.Default);
                _results[1].ShouldBe(Source.Foo);
                _results[2].ShouldBe(Source.Bar);
            }
        }

        public class ByName : AutoMapperSpecBase
        {
            readonly List<Source> _results = new List<Source>();

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByName());
            });

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.Default));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Foo));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Bar));
            }

            [Fact]
            public void Should_map_with_default_mappings()
            {
                _results[0].ShouldBe(Source.Default);
                _results[1].ShouldBe(Source.Foo);
                _results[2].ShouldBe(Source.Bar);
            }
        }

        public class ByValue : AutoMapperSpecBase
        {
            readonly List<Source> _results = new List<Source>();

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByValue());
            });

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.Default));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Foo));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Bar));
            }

            [Fact]
            public void Should_map_with_default_mappings()
            {
                _results[0].ShouldBe(Source.Default);
                _results[1].ShouldBe(Source.Foo);
                _results[2].ShouldBe(Source.Bar);
            }
        }
    }
}
