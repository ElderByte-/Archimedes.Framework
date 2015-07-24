using System;
using Archimedes.Framework.AOP;
using Archimedes.Framework.Context.Annotation;

namespace Archimedes.Framework.Stereotype
{
    public static class ComponentUtil
    {
        public static readonly Type[] ComponentAttributes = { typeof(ServiceAttribute), typeof(ComponentAttribute), typeof(ControllerAttribute) };


        /// <summary>
        /// Checks if the given type has a component attribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsComponent(Type type)
        {
            foreach (var attribute in ComponentAttributes)
            {
                if (type.IsDefined(attribute, false))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks if the given type has a configuration attribute
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsConfiguration(Type type)
        {
            return type.IsDefined(typeof (ConfigurationAttribute), false);
        }

        public static AttributeScanner BuildComponentScanner()
        {
            return new AttributeScanner(ComponentAttributes);
        }
    }
}
