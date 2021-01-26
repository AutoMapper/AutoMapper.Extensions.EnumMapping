using System;

namespace AutoMapper.Extensions.EnumMapping
{
    /// <summary>
    /// Enum configuration options
    /// </summary>
    /// <typeparam name="TSource">Source type for Enum mapping</typeparam>
    /// <typeparam name="TDestination">Destination type for Enum mapping</typeparam>
    public interface IEnumConfigurationExpression<in TSource, in TDestination>
        where TSource : struct, Enum
        where TDestination : struct, Enum
    {
        /// <summary>
        /// Map enum values by name
        /// </summary>
        /// <returns>Enum configuration options</returns>
        IEnumConfigurationExpression<TSource, TDestination> MapByName(bool ignoreCase = false);

        /// <summary>
        /// (default) Map enum values by value (underlying value type)
        /// </summary>
        /// <returns>Enum configuration options</returns>
        IEnumConfigurationExpression<TSource, TDestination> MapByValue();

        /// <summary>
        /// Map enum value from source to destination value
        /// </summary>
        /// <returns>Enum configuration options</returns>
        IEnumConfigurationExpression<TSource, TDestination> MapValue(TSource source, TDestination destination);
    }
}
