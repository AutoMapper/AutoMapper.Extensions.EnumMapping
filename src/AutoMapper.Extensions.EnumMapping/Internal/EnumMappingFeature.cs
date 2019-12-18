using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper.Features;

namespace AutoMapper.Extensions.EnumMapping.Internal
{
    internal class EnumMappingFeature<TSource, TDestination> : IMappingFeature, IEnumConfigurationExpression<TSource, TDestination>
        where TSource : struct, Enum
        where TDestination : struct, Enum
    {
        protected EnumMappingType EnumMappingType = EnumMappingType.Value;
        protected readonly Dictionary<TSource, TDestination> EnumValueMappingsOverride = new Dictionary<TSource, TDestination>();

        public void Configure(TypeMap typeMap)
        {
            if (!typeMap.SourceType.IsEnum)
            {
                throw new ArgumentException($"The type {typeMap.SourceType.FullName} can not be configured as an Enum, because it is not an Enum");
            }

            if (!typeMap.DestinationTypeToUse.IsEnum)
            {
                throw new ArgumentException($"The type {typeMap.DestinationTypeToUse.FullName} can not be configured as an Enum, because it is not an Enum");
            }

            var enumValueMappings = CreateOverridedEnumValueMappings(typeMap.SourceType, typeMap.DestinationTypeToUse);

            typeMap.CustomMapExpression = new CustomMapExpressionFactory<TSource, TDestination>(enumValueMappings).Create();
            typeMap.Features.Set(new EnumMappingValidationRuntimeFeature<TSource, TDestination>(enumValueMappings, EnumMappingType));
        }

        private Dictionary<TSource, TDestination> CreateOverridedEnumValueMappings(Type sourceType, Type destinationType)
        {
            var enumValueMappings = CreateDefaultEnumValueMappings(sourceType, destinationType);

            foreach (var enumValueMappingOverride in EnumValueMappingsOverride)
            {
                enumValueMappings[enumValueMappingOverride.Key] = enumValueMappingOverride.Value;
            }

            return enumValueMappings;
        }

        private Dictionary<TSource, TDestination> CreateDefaultEnumValueMappings(Type sourceType, Type destinationType)
        {
            var enumValueMappings = new Dictionary<TSource, TDestination>();

            var destinationEnumValues = Enum.GetValues(destinationType);

            if (EnumMappingType == EnumMappingType.Name)
            {
                const bool ignoreCase = false;

                foreach (TDestination destinationEnumValue in destinationEnumValues)
                {
                    var destinationEnumName = Enum.GetName(destinationType, destinationEnumValue);

                    if (!string.IsNullOrWhiteSpace(destinationEnumName))
                    {
                        if (Enum.TryParse(destinationEnumName, ignoreCase, out TSource sourceEnumValue))
                        {
                            enumValueMappings.Add(sourceEnumValue, destinationEnumValue);
                        }
                    }
                }
            }
            else
            {
                var sourceEnumValueType = Enum.GetUnderlyingType(sourceType);
                var destinationEnumValueType = Enum.GetUnderlyingType(destinationType);

                foreach (TDestination destinationEnumValue in destinationEnumValues)
                {
                    var sourceEnumValues = Enum.GetValues(sourceType);
                    foreach (TSource sourceEnumValue in sourceEnumValues)
                    {
                        var compareSource = Convert.ChangeType(sourceEnumValue, sourceEnumValueType);
                        var compareDestination = Convert.ChangeType(destinationEnumValue, destinationEnumValueType);

                        if (compareSource.Equals(compareDestination))
                        {
                            enumValueMappings.Add(sourceEnumValue, destinationEnumValue);
                        }
                    }
                }
            }

            return enumValueMappings;
        }

        public IEnumConfigurationExpression<TSource, TDestination> MapByName()
        {
            EnumMappingType = EnumMappingType.Name;
            return this;
        }

        public IEnumConfigurationExpression<TSource, TDestination> MapByValue()
        {
            EnumMappingType = EnumMappingType.Value;
            return this;
        }

        public IEnumConfigurationExpression<TSource, TDestination> MapValue(TSource source, TDestination destination)
        {
            EnumValueMappingsOverride[source] = destination;
            return this;
        }

        public IMappingFeature Reverse()
        {
            var reverseEnumConfigurationExpression = new EnumMappingFeature<TDestination, TSource>();

            if (EnumMappingType == EnumMappingType.Name)
            {
                reverseEnumConfigurationExpression.MapByName();
            }
            else
            {
                reverseEnumConfigurationExpression.MapByValue();
            }

            var reverseEnumValueMappingsOverride = new Dictionary<TDestination, TSource>();
            var sourceEnumValueType = Enum.GetUnderlyingType(typeof(TSource));
            var destinationEnumValueType = Enum.GetUnderlyingType(typeof(TDestination));

            var enumValueMappings = CreateOverridedEnumValueMappings(typeof(TSource), typeof(TDestination));

            var destinationsPerSourceMappings = enumValueMappings.GroupBy(g => g.Value).ToList();
            foreach (var destinationsPerSourceMapping in destinationsPerSourceMappings)
            {
                var destinationValue = destinationsPerSourceMapping.Key;
                var destinationValueAsSourceType = GetDestinationValueAsSourceType(destinationValue, sourceEnumValueType);

                var sourceValues = destinationsPerSourceMapping.Select(x => x.Key).ToList();
                
                var hasDestinationValueSameValueInSource = destinationValueAsSourceType.HasValue && sourceValues.Contains(destinationValueAsSourceType.Value);
                if (hasDestinationValueSameValueInSource)
                {
                    // if there is a matching source and destination value, then that mapping is preferred and no override is needed
                    continue;
                }

                foreach (var sourceValue in sourceValues)
                {
                    var hasDestinationSameValueAsSource = HasDestinationSameValueAsSource(sourceValue, destinationEnumValueType, out TDestination? sourceValueAsDestinationType);

                    if (!hasDestinationSameValueAsSource)
                    {
                        continue;
                    }

                    var isSourceValueUsedInDestinationPartOfEnumMapping = sourceValueAsDestinationType.HasValue && enumValueMappings.ContainsValue(sourceValueAsDestinationType.Value);
                    if (!isSourceValueUsedInDestinationPartOfEnumMapping)
                    {
                        // if there is a source which is not a destination part of a mapping, then that mapping cannot reversed
                        continue;
                    }

                    reverseEnumValueMappingsOverride.Add(destinationValue, sourceValue);
                }
            }

            foreach (var reverseEnumValueMappingOverride in reverseEnumValueMappingsOverride)
            {
                reverseEnumConfigurationExpression.MapValue(reverseEnumValueMappingOverride.Key, reverseEnumValueMappingOverride.Value);
            }

            return reverseEnumConfigurationExpression;
        }

        private bool HasDestinationSameValueAsSource(TSource sourceValue, Type destinationEnumValueType, out TDestination? sourceValueAsDestinationType)
        {
            var hasDestinationSameValueAsSource = false;
            sourceValueAsDestinationType = null;

            if (EnumMappingType == EnumMappingType.Name)
            {
                var destinationEnumName = Enum.GetName(typeof(TDestination), sourceValue);
                if (!string.IsNullOrWhiteSpace(destinationEnumName))
                {
                    if (Enum.TryParse(destinationEnumName, ignoreCase: true,
                            out TDestination parsedSourceValueAsDestinationType))
                    {
                        hasDestinationSameValueAsSource = true;
                        sourceValueAsDestinationType = parsedSourceValueAsDestinationType;
                    }
                }
            }
            else
            {
                var localSourceValueAsDestinationType = (TDestination) Convert.ChangeType(sourceValue, destinationEnumValueType);
                hasDestinationSameValueAsSource = Enum.GetValues(typeof(TDestination)).OfType<TDestination>()
                    .Any(x => Equals(x, localSourceValueAsDestinationType));
                sourceValueAsDestinationType = localSourceValueAsDestinationType;
            }

            return hasDestinationSameValueAsSource;
        }

        private TSource? GetDestinationValueAsSourceType(TDestination destinationValue, Type sourceEnumValueType)
        {
            TSource? destinationValueAsSourceType = null;
            if (EnumMappingType == EnumMappingType.Name)
            {
                var destinationEnumName = Enum.GetName(typeof(TDestination), destinationValue);

                if (!string.IsNullOrWhiteSpace(destinationEnumName))
                {
                    if (Enum.TryParse(destinationEnumName, ignoreCase: true, out TSource parsedSourceValue))
                    {
                        destinationValueAsSourceType = parsedSourceValue;
                    }
                }
            }
            else
            {
                destinationValueAsSourceType = (TSource)Convert.ChangeType(destinationValue, sourceEnumValueType);
            }

            return destinationValueAsSourceType;
        }
    }
}

