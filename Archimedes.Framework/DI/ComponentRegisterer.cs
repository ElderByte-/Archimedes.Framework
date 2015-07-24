using System;
using System.Collections.Generic;
using System.Reflection;
using Archimedes.Framework.Context.Annotation;
using Archimedes.Framework.DI.Factories;
using Archimedes.Framework.Stereotype;

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
            Type t = componentType;
            while (t != null && !(t == typeof(Object)))
            {
                _configuration.RegisterSingleton(t, componentType);
                t = t.BaseType;
            } 
        }

        private void RegisterInterfaceImpl(Type componentType)
        {
            foreach (var @interface in componentType.GetInterfaces())
            {
                _configuration.RegisterSingleton(@interface, componentType);
            }
        }

    }
}
