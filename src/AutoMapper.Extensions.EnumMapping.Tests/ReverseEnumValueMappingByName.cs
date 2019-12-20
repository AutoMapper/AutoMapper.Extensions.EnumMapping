using System;
using System.Collections.Generic;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class ReverseEnumValueMappingByName
    {
        public class Valid : AutoMapperSpecBase
        {
            readonly List<Source> _results = new List<Source>();
            public enum Source { Bar, Foo, Default }
            public enum Destination { Default, Bar, Foo }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByName())
                    .ReverseMap();
            });

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.Default));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Bar));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Foo));
            }

            [Fact]
            public void Should_map_enum_by_name()
            {
                _results[0].ShouldBe(Source.Default);
                _results[1].ShouldBe(Source.Bar);
                _results[2].ShouldBe(Source.Foo);
                ((int)_results[0]).ShouldNotBe((int)Destination.Default);
                ((int)_results[1]).ShouldNotBe((int)Destination.Bar);
                ((int)_results[2]).ShouldNotBe((int)Destination.Foo);
            }
        }

        public class ValidCustomMapping : AutoMapperSpecBase
        {
            readonly List<Source> _results = new List<Source>();
            public enum Source { Default, Foo }
            public enum Destination { Default, Foo, Bar }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByName())
                    .ReverseMap(opt => opt.
                        MapValue(Destination.Bar, Source.Foo));
            });

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.Default));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Foo));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Bar));
            }

            [Fact]
            public void Should_map_using_custom_map()
            {
                _results[0].ShouldBe(Source.Default);
                _results[1].ShouldBe(Source.Foo);
                _results[2].ShouldBe(Source.Foo);
            }
        }

        public class ValidationErrors : NonValidatingSpecBase
        {
            public enum Source { Default, Foo, Bar }
            public enum Destination { Default, Bar }

            protected override MapperConfiguration Configuration => new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByName())
                    .ReverseMap();
            });

            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Source).FullName} to {typeof(Destination).FullName} based on Name{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - Foo{Environment.NewLine}"));
        }

        public class CustomMappingWithValidationErrors : NonValidatingSpecBase
        {
            public enum Source { Default, Foo }
            public enum Destination { Default, Foo, Bar, Error }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByName())
                    .ReverseMap(opt => opt.MapValue(Destination.Bar, Source.Foo));
            });

            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Destination).FullName} to {typeof(Source).FullName} based on Name{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - Error{Environment.NewLine}"));
        }
    }
}
