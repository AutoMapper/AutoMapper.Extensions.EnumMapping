using AutoMapper.Internal;

namespace AutoMapper.Extensions.EnumMapping.Internal;

internal class EnumMappingValidationRuntimeFeatureProxy : IEnumMappingValidationRuntimeFeature
{
    private readonly IEnumMappingValidationRuntimeFeature _innerValidationRuntimeFeature;

    public EnumMappingValidationRuntimeFeatureProxy(IEnumMappingValidationRuntimeFeature innerValidationRuntimeFeature)
    {
        _innerValidationRuntimeFeature = innerValidationRuntimeFeature;
    }

    public void Seal(IGlobalConfiguration configurationProvider) => _innerValidationRuntimeFeature.Seal(configurationProvider);

    public void Validate(TypePair typePair) => _innerValidationRuntimeFeature.Validate(typePair);
}
