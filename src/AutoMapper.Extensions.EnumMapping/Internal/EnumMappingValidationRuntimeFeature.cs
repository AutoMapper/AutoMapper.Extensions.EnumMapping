using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper.Configuration;
using AutoMapper.Internal;

namespace AutoMapper.Extensions.EnumMapping.Internal
{
    internal class EnumMappingValidationRuntimeFeature<TSource, TDestination> : IEnumMappingValidationRuntimeFeature
        where TSource : struct, Enum
        where TDestination : struct, Enum
    {
        private readonly Dictionary<TSource, GetDestinationObject<TDestination>> _enumValueMappings;
        private readonly EnumMappingType _enumMappingType;

        public EnumMappingValidationRuntimeFeature(Dictionary<TSource, GetDestinationObject<TDestination>> enumValueMappings, EnumMappingType enumMappingType)
        {
            _enumValueMappings = enumValueMappings;
            _enumMappingType = enumMappingType;
        }

        public void Seal(IGlobalConfiguration configurationProvider)
        {
        }

        void IEnumMappingValidationRuntimeFeature.Validate(ValidationContext validationContext)
        {
            var hasMappingError = false;
            var typePair = validationContext.TypeMap.Types;
            var sourceEnumMappings = Enum.GetValues(typePair.SourceType);

            var messageBuilder = new StringBuilder($"Missing enum mapping from {typePair.SourceType.FullName} to {typePair.DestinationType.FullName} based on {_enumMappingType}");

            messageBuilder.AppendLine();
            messageBuilder.AppendLine("The following source values are not mapped:");

            foreach (TSource sourceEnumMapping in sourceEnumMappings)
            {
                if (!_enumValueMappings.ContainsKey(sourceEnumMapping))
                {
                    hasMappingError = true;
                    messageBuilder.AppendLine($" - {sourceEnumMapping}");
                }
            }

            if (hasMappingError)
            {
                validationContext.Exceptions.Add(new AutoMapperConfigurationException(messageBuilder.ToString()));
            }
        }
    }
}
