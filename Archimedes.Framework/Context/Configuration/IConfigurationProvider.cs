using System;

namespace Archimedes.Framework.Context.Configuration
{
    public interface IConfigurationProvider
    {
        void HandleConfiguration(ApplicationContext ctx, Type configurationType);
    }
}
