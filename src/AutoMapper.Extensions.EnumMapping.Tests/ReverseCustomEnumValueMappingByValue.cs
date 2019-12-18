using System;
using System.Collections.Generic;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class ReverseCustomEnumValueMappingByValue
    {
        public class MissingDestinationValue : AutoMapperSpecBase
        {
            readonly List<Source> _results = new List<Source>();
            public enum Source { Default, Foo, OnlyInSource }
            public enum Destination { Default, Foo }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByValue()
                        .MapValue(Source.OnlyInSource, Destination.Foo))
                    .ReverseMap();
            });

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.Default));
                _results.Add(Mapper.Map<Destination, Source>(Destination.Foo));
            }

            [Fact]
            public void Should_map_using_custom_map()
            {
                _results[0].ShouldBe(Source.Default);
                _results[1].ShouldBe(Source.Foo);
            }
        }

        public class MissingSourceValue : NonValidatingSpecBase
        {
            public enum Source { Default, Foo }
            public enum Destination { Default, Foo, OnlyInDestination }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByValue())
                    .ReverseMap();
            });

            [Fact]
            public void Should_fail_validation() =>
                new Action(() => Configuration.AssertConfigurationIsValid()).ShouldThrowException<AutoMapperConfigurationException>(
                    ex => ex.Message.ShouldBe(
                        $@"Missing enum mapping from {typeof(Destination).FullName} to {typeof(Source).FullName} based on Value{Environment.NewLine}The following source values are not mapped:{Environment.NewLine} - OnlyInDestination{Environment.NewLine}"));
        }

        public class OnlyInSourceValueIsMappedToOnlyInDestinationValue : AutoMapperSpecBase
        {
            readonly List<Source> _results = new List<Source>();

            public enum Source { Default, OnlyInSource }
            public enum Destination { Default, OnlyInDestination }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByValue()
                        .MapValue(Source.OnlyInSource, Destination.OnlyInDestination))
                    .ReverseMap();
            });
            
            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.Default));
                _results.Add(Mapper.Map<Destination, Source>(Destination.OnlyInDestination));
            }

            [Fact]
            public void Should_map_using_custom_map()
            {
                _results[0].ShouldBe(Source.Default);
                _results[1].ShouldBe(Source.OnlyInSource);
            }
        }

        public class OnlyInSourceValueIsMappedToDestinationValueButDestinationValueHasAlsoSameSourceValueMapping : AutoMapperSpecBase
        {
            readonly List<Source> _results = new List<Source>();
            public enum Source { A, B, C, D, InSourceAndDestionation, OnyInSource }
            public enum Destination { A, B, C, D, InSourceAndDestionation }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByValue()
                        .MapValue(Source.A, Destination.B)
                        .MapValue(Source.D, Destination.InSourceAndDestionation)
                        .MapValue(Source.InSourceAndDestionation, Destination.D)
                        .MapValue(Source.OnyInSource, Destination.InSourceAndDestionation))
                    .ReverseMap();
            });

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.A));
                _results.Add(Mapper.Map<Destination, Source>(Destination.B));
                _results.Add(Mapper.Map<Destination, Source>(Destination.C));
                _results.Add(Mapper.Map<Destination, Source>(Destination.D));
                _results.Add(Mapper.Map<Destination, Source>(Destination.InSourceAndDestionation));
            }

            [Fact]
            public void Should_map_using_custom_map()
            {
                _results[0].ShouldBe(Source.A);
                _results[1].ShouldBe(Source.B);
                _results[2].ShouldBe(Source.C);
                _results[3].ShouldBe(Source.InSourceAndDestionation);
                _results[4].ShouldBe(Source.D);
            }
        }

        public class AllSourceValuesAreMappedToOtherDestinationValues : AutoMapperSpecBase
        {
            readonly List<Source> _results = new List<Source>();
            public enum Source { A, B, C, D, E, F }
            public enum Destination { A, B, C, D, E, F }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByValue()
                        .MapValue(Source.F, Destination.A)
                        .MapValue(Source.A, Destination.B)
                        .MapValue(Source.B, Destination.C)
                        .MapValue(Source.C, Destination.D)
                        .MapValue(Source.D, Destination.E)
                        .MapValue(Source.E, Destination.A)
                    )
                    .ReverseMap();
            });

            protected override void Because_of()
            {
                _results.Add(Mapper.Map<Destination, Source>(Destination.A));
                _results.Add(Mapper.Map<Destination, Source>(Destination.B));
                _results.Add(Mapper.Map<Destination, Source>(Destination.C));
                _results.Add(Mapper.Map<Destination, Source>(Destination.D));
                _results.Add(Mapper.Map<Destination, Source>(Destination.E));
                _results.Add(Mapper.Map<Destination, Source>(Destination.F));
            }

            [Fact]
            public void Should_map_using_custom_map()
            {
                _results[0].ShouldBe(Source.E);
                _results[1].ShouldBe(Source.A);
                _results[2].ShouldBe(Source.B);
                _results[3].ShouldBe(Source.C);
                _results[4].ShouldBe(Source.D);
                _results[5].ShouldBe(Source.F);
            }
        }

        public class AllSourceValuesAreMappedToOneDestinationValue : AutoMapperSpecBase
        {
            Source _result;
            public enum Source { A, B, C, D, E, F }
            public enum Destination { C = 2 }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.CreateMap<Source, Destination>()
                    .ConvertUsingEnumMapping(opt => opt.MapByValue()
                        .MapValue(Source.A, Destination.C)
                        .MapValue(Source.B, Destination.C)
                        .MapValue(Source.C, Destination.C)
                        .MapValue(Source.D, Destination.C)
                        .MapValue(Source.E, Destination.C)
                        .MapValue(Source.F, Destination.C)
                    )
                    .ReverseMap();
            });

            protected override void Because_of()
            {
                _result = Mapper.Map<Destination, Source>(Destination.C);
            }

            [Fact]
            public void Should_map_using_custom_map()
            {
                _result.ShouldBe(Source.C);
            }
        }
    }
}
