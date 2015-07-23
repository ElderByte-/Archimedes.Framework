using System;
using System.Linq;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.AOP
{
    internal static class AOPUitl
    {
        private static readonly Type[] ComponentAttributs = { typeof(ServiceAttribute), typeof(ControllerAttribute), typeof(ComponentAttribute) };
        private static readonly Type PrimaryAttribute = typeof(PrimaryAttribute);


        public static bool IsTypeComponent(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

           return GetComponentAttribute(type) != null;
        }

        public static ComponentAttribute GetComponentAttribute(Type impl)
        {
            if (impl == null) throw new ArgumentNullException("impl");

            foreach (var componentAttribut in ComponentAttributs)
            {
                var attr = impl.GetCustomAttributes(componentAttribut, false);
                if (attr.Length > 0)
                {
                    return (ComponentAttribute)attr[0];
                }
            }
            return null;
        }


        public static PrimaryAttribute GetPrimaryAttribute(Type impl)
        {
            if (impl == null) throw new ArgumentNullException("impl");

            return (PrimaryAttribute) impl.GetCustomAttributes(PrimaryAttribute, false).FirstOrDefault();
        }
    }
}
