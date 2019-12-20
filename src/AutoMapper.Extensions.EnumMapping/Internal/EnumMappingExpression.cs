using System;

namespace AutoMapper.Extensions.EnumMapping.Internal
{
    internal class EnumMappingExpression<TSource, TDestination> : IEnumMappingExpression<TSource, TDestination>
        where TSource : struct, Enum
        where TDestination : struct, Enum
    {
        protected IMappingExpression<TSource, TDestination> MappingExpression { get; }

        public EnumMappingExpression(IMappingExpression<TSource, TDestination> mappingExpression)
        {
            MappingExpression = mappingExpression;
        }

        public void ReverseMap()
        {
            ReverseMap(null);
        }

        public void ReverseMap(Action<IEnumConfigurationExpression<TDestination, TSource>> options)
        {
            var reversedMappingExpression = MappingExpression.ReverseMap();
            if (options != null)
            {
                reversedMappingExpression.ConvertUsingEnumMapping(options);
            }
        }
    }
}