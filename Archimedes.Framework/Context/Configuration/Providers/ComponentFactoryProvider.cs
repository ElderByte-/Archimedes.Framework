using System;
using System.Reflection;
using Archimedes.Framework.Context.Annotation;

namespace Archimedes.Framework.Context.Configuration.Providers
{
    internal class ComponentFactoryProvider : IConfigurationProvider
    {

        public void HandleConfiguration(ApplicationContext ctx, Type configurationType)
        {
            var allMethods = configurationType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var methodInfo in allMethods)
            {
                if (methodInfo.IsDefined(typeof (ComponentFactoryAttribute), false))
                {
                    RegisterFactoryMethod(methodInfo);
                }
            }
        }

        private void RegisterFactoryMethod(MethodInfo method)
        {
            // TODO 
        }
    }
}
