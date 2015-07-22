using System;
using Archimedes.Framework.AOP;

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

        public static AttributeScanner BuildComponentScanner()
        {
            return new AttributeScanner(ComponentAttributes);
        }
    }
}
