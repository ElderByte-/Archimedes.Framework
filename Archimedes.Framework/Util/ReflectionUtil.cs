using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;

namespace Archimedes.Framework.Util
{
    public static class ReflectionUtil
    {
        private static readonly ISet<Type> _defaultInterfaces = new HashSet<Type>();

        static ReflectionUtil()
        {
            _defaultInterfaces.Add(typeof (_MemberInfo));
            _defaultInterfaces.Add(typeof(_Type));
            _defaultInterfaces.Add(typeof(IReflect));
            _defaultInterfaces.Add(typeof(ISerializable));
            _defaultInterfaces.Add(typeof(ICloneable));
        }


        public static bool IsDefaultInterface(Type iface)
        {
            return _defaultInterfaces.Contains(iface);
        }

        public static IEnumerable<Type> FindCustomInterfaces(Type clazz)
        {
            return clazz.GetInterfaces().Where(iface => !_defaultInterfaces.Contains(iface)).ToList();
        }

        public static IEnumerable<Type> FindCustomBaseTypes(Type clazz)
        {
            Type t = clazz;
            while (t != null && !(t == typeof(Object)))
            {
                yield return t;
                t = t.BaseType;
            }
        }
    }
}
