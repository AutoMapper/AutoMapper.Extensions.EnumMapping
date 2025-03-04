using System;
using System.Reflection;
using AutoMapper.Extensions.EnumMapping.Tests.Internal;
using Shouldly;
using Xunit;

namespace AutoMapper.Extensions.EnumMapping.Tests
{
    public class ReverseCustomEnumMappingByCustom
    {
        public class Valid : AutoMapperSpecBase
        {
            Destination _result;

            // Assume, as is the case for my use case, that both the source
            // and destination enumerations are created via code generation
            // from sources outside our control, and thus I cannot make updates
            // or changes to them to work around the issue.

            // This is idiomatic of how a Protobuf enum is generated, with the Unspecified
            // value acting as a stand-in for when the field is not set by the client or server
            public enum Source
            {
                Unspecified = 0,
                Bar = 1,
                Baz = 2,
            }

            // In our case, this is generated from an OpenAPI spec via Kiota
            public enum Destination
            {
                BAR_ALT_NAME, // we can't map by name because the names don't match, even with case insensitivity turned on
                BAZ_ALT_NAME,
            }

            public class TestEnumProfile : Profile
            {
                public TestEnumProfile()
                {
                    CreateMap<Source, Destination>()
                        .ConvertUsingEnumMapping(
                            opts =>
                            {
                                opts
                                    .MapByCustom()
                                    .MapValue(Source.Bar, Destination.BAR_ALT_NAME)
                                    .MapValue(Source.Baz, Destination.BAZ_ALT_NAME)
                                    .MapException(Source.Unspecified, () => new InvalidOperationException($"Unspecified values are not supported"));
                            })
                        .ReverseMap();
                }
            }

            protected override MapperConfiguration Configuration { get; } = new MapperConfiguration(cfg =>
            {
                cfg.EnableEnumMappingValidation();
                cfg.AddMaps(typeof(ReverseCustomEnumMappingByCustom).GetTypeInfo().Assembly);
            });

            protected override void Because_of()
            {
                _result = Mapper.Map<Source, Destination>(Source.Bar);
            }

            [Fact]
            public void Should_map_enum_by_value()
            {
                _result.ShouldBe(Destination.BAR_ALT_NAME);
            }

            [Fact]
            public void TestBarMapping()
            {
                // Passes
                var res = Mapper.Map<Destination>(Source.Bar);
                res.ShouldBe(Destination.BAR_ALT_NAME);
                //Assert.That(_mapper.Map<Destination>(Source.Bar), Is.EqualTo(Destination.BAR_ALT_NAME));
            }

            [Fact]
            public void TestBazMapping()
            {
                // Passes
                var res = Mapper.Map<Destination>(Source.Baz);
                res.ShouldBe(Destination.BAZ_ALT_NAME);
                //Assert.That(_mapper.Map<Destination>(Source.Baz), Is.EqualTo(Destination.BAZ_ALT_NAME));
            }

            [Fact]
            public void TestUnspecifiedMapping()
            {
                // Passes
                Assert.Throws<InvalidOperationException>(() =>
                {
                    Mapper.Map<Destination>(Source.Unspecified);
                    //_mapper.Map<Destination>(Source.Unspecified);
                });
            }

            [Fact]
            public void TestReverseBarMapping()
            {
                // Passes
                var res = Mapper.Map<Source>(Destination.BAR_ALT_NAME);
                res.ShouldBe(Source.Bar);
                //Assert.That(_mapper.Map<Source>(Destination.BAR_ALT_NAME), Is.EqualTo(Source.Bar));
            }

            [Fact]
            public void TestReverseBazMapping()
            {
                // Failure: Expected: Baz  But was:  Bar
                var res = Mapper.Map<Source>(Destination.BAZ_ALT_NAME);
                res.ShouldBe(Source.Baz);
                //Assert.That(_mapper.Map<Source>(Destination.BAZ_ALT_NAME), Is.EqualTo(Source.Baz));
            }
        } 
    }
}
