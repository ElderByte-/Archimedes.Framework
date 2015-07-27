using System;
using System.Collections.Generic;
using System.Reflection;
using Archimedes.Framework.Context.Annotation;
using Archimedes.Framework.DI.Factories;
using Archimedes.Framework.Stereotype;
using Archimedes.Framework.Util;

namespace Archimedes.Framework.DI
{
    public class ComponentRegisterer
    {
        private readonly IModuleConfiguration _configuration;

        public ComponentRegisterer(IModuleConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void RegisterComponents(IEnumerable<Type> componentTypes)
        {
            foreach (var componentType in componentTypes)
            {
                RegisterComponent(componentType);
            }
        }

        public void RegisterComponent(Type componentType)
        {
            if (ComponentUtil.IsComponent(componentType) || ComponentUtil.IsConfiguration(componentType))
            {
                RegisterInterfaceImpl(componentType);
                RegisterInheritanceImpl(componentType);

                RegisterFactoryMethods(componentType);
            }
            else
            {
                throw new NotSupportedException("All component types must have a Component derived attribute!");
            }

        }

        private void RegisterFactoryMethods(Type componentType)
        {
            var allMethods = componentType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var methodInfo in allMethods)
            {
                if (methodInfo.IsDefined(typeof(ComponentFactoryAttribute), false))
                {
                    RegisterFactoryMethod(_configuration, componentType, methodInfo);
                }
            }
        }

        private void RegisterFactoryMethod(IModuleConfiguration configuration, Type componentType, MethodInfo method)
        {
            var iface = method.ReturnType;
            var factory = new FactoryMethodReference(componentType, method);

            configuration.RegisterFactoryMethod(iface, factory);
        }


        private void RegisterInheritanceImpl(Type componentType)
        {
            var baseTypes = ReflectionUtil.FindCustomBaseTypes(componentType);

            foreach (var baseType in baseTypes)
            {
                _configuration.RegisterSingleton(baseType, componentType);
            }
        }

        private void RegisterInterfaceImpl(Type componentType)
        {
            var baseTypes = ReflectionUtil.FindCustomInterfaces(componentType);

            foreach (var baseType in baseTypes)
            {
                _configuration.RegisterSingleton(baseType, componentType);
            }
        }

    }
}
