using System.Linq;
using AutoMapper.Extensions.EnumMapping.Internal;
using AutoMapper.Internal;

namespace AutoMapper.Extensions.EnumMapping
{
    /// <summary>
    /// Extension class to support validation for EnumMappings
    /// </summary>
    public static class EnumMapperConfigurationExpressionExtensions
    {
        /// <summary>
        /// Enable EnumMapping configuration validation
        /// </summary>
        /// <param name="mapperConfigurationExpression">Configuration object for AutoMapper</param>
        public static void EnableEnumMappingValidation(this IMapperConfigurationExpression mapperConfigurationExpression)
        {
            mapperConfigurationExpression.Internal().Validator(context =>
            {
                if (context.TypeMap != null)
                {
                    foreach (var feature in context.TypeMap.Features)
                    {
                        if (feature is IEnumMappingValidationRuntimeFeature validator)
                        {
                            validator.Validate(context.TypeMap.Types);
                        }
                    }
                }
            });
        }
    }
}
