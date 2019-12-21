AutoMapper.Extensions.EnumMapping
===========
[![Build Status](https://ci.appveyor.com/api/projects/status/github/jbogard/AutoMapper.Extensions.EnumMapping?branch=master&svg=true)](https://ci.appveyor.com/project/jbogard/automapper-extensions-enummapping) 
[![NuGet](https://img.shields.io/nuget/dt/AutoMapper.Extensions.EnumMapping.svg)](https://www.nuget.org/packages/AutoMapper.Extensions.EnumMapping) 
[![NuGet](https://img.shields.io/nuget/vpre/AutoMapper.Extensions.EnumMapping.svg)](https://www.nuget.org/packages/AutoMapper.Extensions.EnumMapping)
[![MyGet (dev)](https://img.shields.io/myget/automapperdev/v/AutoMapper.Extensions.EnumMapping.svg)](http://myget.org/gallery/automapperdev)

### Summary

The AutoMapper.Extensions.EnumMapping library gives you control about your enum values mappings. Normally enums are automatically mapped bij AutoMapper as an integer, but you have no control about custom mappings. It is possible to create a custom type converter for every enum.

This library supports mapping enums values like properties.

This library is Cross-platform, supporting `netstandard2.0` and `net461`.

### Dependencies

- [AutoMapper](https://www.nuget.org/packages/AutoMapper/) (from version 9.1.0)

### Installing AutoMapper.Extensions.EnumMapping

You should install [AutoMapper.Extensions.EnumMapping with NuGet](https://www.nuget.org/packages/AutoMapper.Extensions.EnumMapping):

    Install-Package AutoMapper.Extensions.EnumMapping

Or via the .NET Core command line interface:

    dotnet add package AutoMapper.Extensions.EnumMapping

Either commands, from Package Manager Console or .NET Core CLI, will download and install AutoMapper.Extensions.EnumMapping. AutoMapper.Extensions.EnumMapping has no dependencies. 

### Usage
Install via NuGet first:
`Install-Package AutoMapper.Extensions.EnumMapping`

To use it:

For method `CreateMap` this library provide a `ConvertUsingEnumMapping` method. This method add all default mappings from source to destination enum values.

If you want to change some mappings, then you can use `MapValue` method. This is a chainable method.

Default the enum values are mapped by value, but it is possible to map by name calling  `MapByName()` or  `MapByValue()`.

```csharp
using AutoMapper.Extensions.EnumMapping;

public enum Source
{
    Default = 0,
    First = 1,
    Second = 2
}

public enum Destination
{
    Default = 0,
    Second = 2
}

internal class YourProfile : Profile
{
    public YourProfile()
    {
        CreateMap<Source, Destination>()
            .ConvertUsingEnumMapping(opt => opt
		// optional: .MapByValue() or MapByName(), without configuration MapByValue is used
		.MapValue(Source.First, Destination.Default))
            .ReverseMap(); // to support Destination to Source mapping, including custom mappings of ConvertUsingEnumMapping
    }
}
    ...
```

### Testing

[AutoMapper](https://www.nuget.org/packages/AutoMapper/) provides a nice tooling for validating typemaps. This library adds an extra `EnumMapperConfigurationExpressionExtensions.EnableEnumMappingValidation` extension method to extend the existing `AssertConfigurationIsValid()` method to validate also the enum mappings.

To enable testing the enum mapping configuration:

```csharp

public class MappingConfigurationsTests
{
    [Fact]
    public void WhenProfilesAreConfigured_ItShouldNotThrowException()
    {
        // Arrange
        var config = new MapperConfiguration(configuration =>
        {
            configuration.EnableEnumMappingValidation();

            configuration.AddMaps(typeof(AssemblyInfo).GetTypeInfo().Assembly);
        });
		
        // Assert
        config.AssertConfigurationIsValid();
    }
}
```
