using System;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class ReverseEnumValueMappingByCustom
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
                    .ConvertUsingEnumMapping(opt => opt
                        .MapByCustom()
                        .MapValue(Source.Default, Destination.Default)
                        .MapValue(Source.Foo, Destination.Foo)
                        .MapValue(Source.Bar, Destination.Bar)
                    )
                    .ReverseMap();
            });

            protected override void Because_of()
            {
                _result = Mapper.Map<Source, Destination>(Source.Bar);
            }

            [Fact]
            public void Should_map_enum_by_custom()
            {
                _result.ShouldBe(Destination.Bar);
                ((int)_result).ShouldBe((int)Source.Foo);

            }
        }

        public class ValidCustomMapping : AutoMapperSpecBase
        {
            Destination _result;
            public enum Source { Default, Bar }
            public enum Destination { Default, Bar }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                 .ConvertUsingEnumMapping(opt => opt
                        .MapByCustom()
                        .MapValue(Source.Default, Destination.Default)
                        .MapValue(Source.Bar, Destination.Bar)
                    )
                    .ReverseMap();
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
                 .ConvertUsingEnumMapping(opt => opt
                     .MapByCustom()
                     .MapValue(Source.Default, Destination.Default)
                     .MapValue(Source.Bar, Destination.Bar)
                )
                .ReverseMap();
            });

            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Source).FullName} to {typeof(Destination).FullName} based on Custom{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - Foo{Environment.NewLine}"));
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
                        .MapByCustom()
                        .MapValue(Source.Default, Destination.Default)
                        .MapException(Source.Foo, () => new NotSupportedException($"Foo is not valid value"))
                        .MapValue(Source.Bar, Destination.Bar))
                    .ReverseMap();
            });

            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Source).FullName} to {typeof(Destination).FullName} based on Custom{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - Error{Environment.NewLine}"));
        }

        public class ValidCustomReverseMapping : AutoMapperSpecBase
        {
            Source _resultDefault;
            Source _resultFoo;
            Source _resultBar;
            public enum Source { Default, Bar }
            public enum Destination { Default, Foo, Bar }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt
                        .MapByCustom()
                        .MapValue(Source.Default, Destination.Default)
                        .MapValue(Source.Bar, Destination.Bar))
                    .ReverseMap(optr => optr.MapByCustom().MapValue(Destination.Foo, Source.Bar));
            });

            protected override void Because_of()
            {
                _resultDefault = Mapper.Map<Source>(Destination.Default);
                _resultFoo = Mapper.Map<Source>(Destination.Foo);
                _resultBar = Mapper.Map<Source>(Destination.Bar);
            }

            [Fact]
            public void Should_map_using_reverse_custom_map()
            {
                _resultDefault.ShouldBe(Source.Default);
                _resultFoo.ShouldBe(Source.Bar);
                _resultBar.ShouldBe(Source.Bar);
            }
        }
    }
}
