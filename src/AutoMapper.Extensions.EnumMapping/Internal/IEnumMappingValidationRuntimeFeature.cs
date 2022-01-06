using AutoMapper.Features;
using AutoMapper.Internal;

namespace AutoMapper.Extensions.EnumMapping.Internal
{
    internal interface IEnumMappingValidationRuntimeFeature : IRuntimeFeature
    {
        void Validate(TypePair typePair);
    }
}