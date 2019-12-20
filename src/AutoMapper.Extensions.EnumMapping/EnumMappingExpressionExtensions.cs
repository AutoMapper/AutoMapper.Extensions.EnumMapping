using System;
using AutoMapper.Extensions.EnumMapping.Internal;

namespace AutoMapper.Extensions.EnumMapping
{
    /// <summary>
    /// Extension class to support EnumMapping
    /// </summary>
    public static class EnumMappingExpressionExtensions
    {
        /// <summary>
        /// Skip member mapping and use a EnumMapping converter convention to convert to the destination type
        /// </summary>
        /// <remarks>Not used for LINQ projection (ProjectTo)</remarks>
        /// <param name="mappingExpression">Mapping configuration options</param>
        /// <typeparam name="TSource">Source enum type</typeparam>
        /// <typeparam name="TDestination">Destination enum type</typeparam>
        public static IEnumMappingExpression<TSource, TDestination> ConvertUsingEnumMapping<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
            where TSource : struct, Enum
            where TDestination : struct, Enum
            => mappingExpression.ConvertUsingEnumMapping(null);

        /// <summary>
        /// Skip member mapping and use a EnumMapping converter convention to convert to the destination type
        /// </summary>
        /// <remarks>Not used for LINQ projection (ProjectTo)</remarks>
        /// <param name="mappingExpression">Mapping configuration options</param>
        /// <param name="options">Callback for Mapping configuration options</param>
        /// <typeparam name="TSource">Source enum type</typeparam>
        /// <typeparam name="TDestination">Destination enum type</typeparam>
        public static IEnumMappingExpression<TSource, TDestination> ConvertUsingEnumMapping<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Action<IEnumConfigurationExpression<TSource, TDestination>> options)
            where TSource : struct, Enum
            where TDestination : struct, Enum
        {
            var feature = mappingExpression.Features.Get<EnumMappingFeature<TSource, TDestination>>();
            if (feature is null)
            {
                mappingExpression.Features.Set(feature = new EnumMappingFeature<TSource, TDestination>());
            }
            options?.Invoke(feature);

            return new EnumMappingExpression<TSource, TDestination>(mappingExpression);
        }
    }
}
