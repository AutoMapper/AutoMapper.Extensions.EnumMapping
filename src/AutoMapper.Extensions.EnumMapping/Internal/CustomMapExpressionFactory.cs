using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace AutoMapper.Extensions.EnumMapping.Internal
{
    internal class CustomMapExpressionFactory<TSource,TDestination>
        where TDestination : struct, Enum
    {
        private readonly Dictionary<TSource, GetDestinationObject<TDestination>> _enumValueMappings;

        public CustomMapExpressionFactory(Dictionary<TSource, GetDestinationObject<TDestination>> enumValueMappings)
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
            if (!_enumValueMappings.TryGetValue(source, out var getDestinationObject))
            {
                throw new AutoMapperMappingException($"Value {source} of type {source.GetType().FullName} not supported");
            }

            return getDestinationObject.GetDestinationFunc.Invoke();
        }
    }
}
