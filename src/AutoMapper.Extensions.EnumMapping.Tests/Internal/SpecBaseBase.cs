using Microsoft.Extensions.Logging;

namespace AutoMapper.Extensions.EnumMapping.Tests.Internal
{
    public abstract class SpecBaseBase
    {
        protected static ILoggerFactory _loggerFactory;

        static SpecBaseBase()
        {
            _loggerFactory = new LoggerFactory();
        }

        protected virtual void MainSetup()
        {
            Establish_context();
            Because_of();
        }

        protected virtual void MainTeardown()
        {
            Cleanup();
        }

        protected virtual void Establish_context()
        {
        }

        protected virtual void Because_of()
        {
        }

        protected virtual void Cleanup()
        {
        }
    }
}
