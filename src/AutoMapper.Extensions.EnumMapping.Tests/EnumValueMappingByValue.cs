using System;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class EnumValueMappingByValue
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
                    .ConvertUsingEnumMapping(opt => opt.MapByValue());
            });

            protected override void Because_of()
            {
                _result = Mapper.Map<Source, Destination>(Source.Bar);
            }

            [Fact]
            public void Should_map_enum_by_value()
            {
                _result.ShouldBe(Destination.Foo);
                ((int)_result).ShouldBe((int)Source.Bar);

            }
        }

        public class ValidCustomMapping : AutoMapperSpecBase
        {
            Destination _result;
            public enum Source { Default, Foo, Bar }
            public enum Destination { Default, Bar }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt
                        .MapByValue()
                        .MapValue(Source.Bar, Destination.Bar));
            });

            protected override void Because_of()
            {
                _result = Mapper.Map<Source, Destination>(Source.Bar);
            }

            [Fact]
            public void Should_map_using_custom_map()
            {
                _result.ShouldBe(Destination.Bar);
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
                    .ConvertUsingEnumMapping(opt => opt.MapByValue());
            });

            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Source).FullName} to {typeof(Destination).FullName} based on Value{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - Bar{Environment.NewLine}"));
        }

        public class CustomMappingWithValidationErrors : NonValidatingSpecBase
        {
            public enum Source { Default, Foo, Bar, Error }
            public enum Destination { Default, Bar }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt
                        .MapByValue()
                        .MapValue(Source.Bar, Destination.Bar));
            });

            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Source).FullName} to {typeof(Destination).FullName} based on Value{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - Error{Environment.NewLine}"));
        }
    }
}
