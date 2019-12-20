using System;

namespace AutoMapper.Extensions.EnumMapping
{
    /// <summary>
    /// Skip member mapping and use a EnumMapping converter convention to convert to the destination type
    /// </summary>
    /// <remarks>Not used for LINQ projection (ProjectTo)</remarks>
    /// <typeparam name="TSource">Source type for Enum mapping</typeparam>
    /// <typeparam name="TDestination">Destination type for Enum mapping</typeparam>
    public interface IEnumMappingExpression<in TSource, in TDestination>
        where TSource : struct, Enum
        where TDestination : struct, Enum
    {
        /// <summary>
        /// Create a type mapping from the <typeparamref name="TDestination"/> type to the <typeparamref name="TSource"/> type, using the existing configuration for <typeparamref name="TSource"/> type to <typeparamref name="TDestination"/> type.
        /// </summary>
        void ReverseMap();

        /// <summary>
        /// Create a type mapping from the <typeparamref name="TDestination"/> type to the <typeparamref name="TSource"/> type, using the existing configuration for <typeparamref name="TSource"/> type to <typeparamref name="TDestination"/> type.
        /// </summary>
        /// <param name="options">Callback for reverse mapping configuration options</param>
        void ReverseMap(Action<IEnumConfigurationExpression<TDestination, TSource>> options);
    }
}