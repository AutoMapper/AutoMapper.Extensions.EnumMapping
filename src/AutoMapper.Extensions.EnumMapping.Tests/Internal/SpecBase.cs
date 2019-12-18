using System;

namespace AutoMapper.Extensions.EnumMapping.Tests.Internal
{
    public abstract class SpecBase : SpecBaseBase, IDisposable
    {
        protected SpecBase()
        {
            Establish_context();
            Because_of();
        }

        public void Dispose()
        {
            Cleanup();
        }
    }
}
