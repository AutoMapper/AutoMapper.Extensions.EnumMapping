using System;

#nullable enable

namespace AutoMapper.Extensions.EnumMapping.Internal;

internal struct GetDestinationObject<TDestination>
    where TDestination : struct, Enum
{
    public GetDestinationObject(GetDestinationType getDestinationType, Func<TDestination> getDestinationFunc)
    {
        GetDestinationType = getDestinationType;
        GetDestinationFunc = getDestinationFunc;
    }

    public GetDestinationType GetDestinationType { get; }
    public Func<TDestination> GetDestinationFunc { get; }
}

internal static class GetDestinationObject
{
    public static GetDestinationObject<TDestination> Value<TDestination>(TDestination value)
        where TDestination : struct, Enum
    {
        return new GetDestinationObject<TDestination>(GetDestinationType.Value, () => value);
    }
    
    public static GetDestinationObject<TDestination> Exception<TDestination>(Func<Exception> throwExceptionFunc)
        where TDestination : struct, Enum
    {
        return new GetDestinationObject<TDestination>(GetDestinationType.Exception, () => throw throwExceptionFunc());
    }
}