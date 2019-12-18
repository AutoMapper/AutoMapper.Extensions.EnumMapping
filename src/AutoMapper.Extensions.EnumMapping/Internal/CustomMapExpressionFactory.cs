using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutoMapper.Extensions.EnumMapping.Internal
{
    internal class CustomMapExpressionFactory<TSource,TDestination>
    {
        private readonly Dictionary<TSource, TDestination> _enumValueMappings;

        public CustomMapExpressionFactory(Dictionary<TSource, TDestination> enumValueMappings)
        {
            _enumValueMappings = enumValueMappings;
        }

        public LambdaExpression Create()
        {
            Expression<Func<TSource, TDestination, ResolutionContext, TDestination>> method = (s, d, c) => ConvertEnumValue(s);
            return method;
        }

        private TDestination ConvertEnumValue(TSource source)
        {
            if (!_enumValueMappings.TryGetValue(source, out TDestination result))
            {
                throw new AutoMapperMappingException($"Value {source} of type {source.GetType().FullName} not supported");
            }

            return result;
        }
    }
}
