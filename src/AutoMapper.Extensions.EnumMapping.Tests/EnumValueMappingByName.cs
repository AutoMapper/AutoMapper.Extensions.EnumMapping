using System;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class EnumValueMappingByName
    {
        public class Valid : AutoMapperSpecBase
        {
            Destination _result;
            public enum Source { Default, Foo, Bar }
            public enum Destination { Default, Bar, Foo }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByName());
            });

            protected override void Because_of()
            {
                _result = Mapper.Map<Source, Destination>(Source.Bar);
            }

            [Fact]
            public void Should_map_enum_by_name()
            {
                _result.ShouldBe(Destination.Bar);
                ((int)_result).ShouldNotBe((int)Source.Bar);
            }
        }

        public class ValidIgnoreCase : AutoMapperSpecBase
        {
            Destination _result;
            public enum Source { Default, FOO, Bar, FooBar }
            public enum Destination { fOObAR, DefaulT, Bar, Foo }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByName(true));
            });

            protected override void Because_of()
            {
                _result = Mapper.Map<Source, Destination>(Source.FooBar);
            }

            [Fact]
            public void Should_map_enum_by_name()
            {
                _result.ShouldBe(Destination.fOObAR);
                ((int)_result).ShouldNotBe((int)Source.FooBar);
            }

        }

        public class ValidCustomMapping : AutoMapperSpecBase
        {
            Destination _result;
            public enum Source { Default, Foo, Bar }
            public enum Destination { Default, Foo }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt
                        .MapByName()
                        .MapValue(Source.Bar, Destination.Foo));
            });

            protected override void Because_of()
            {
                _result = Mapper.Map<Source, Destination>(Source.Bar);
            }

            [Fact]
            public void Should_map_using_custom_map()
            {
                _result.ShouldBe(Destination.Foo);
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
                    .ConvertUsingEnumMapping(opt => opt.MapByName());
            });

            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Source).FullName} to {typeof(Destination).FullName} based on Name{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - Foo{Environment.NewLine}"));
        }

        public class CustomMappingWithValidationErrors : NonValidatingSpecBase
        {
            public enum Source { Default, Foo, Bar, Error }
            public enum Destination { Default, Foo }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt
                        .MapByName()
                        .MapValue(Source.Bar, Destination.Foo));
            });
            
            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Source).FullName} to {typeof(Destination).FullName} based on Name{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - Error{Environment.NewLine}"));
        }
    }
}
