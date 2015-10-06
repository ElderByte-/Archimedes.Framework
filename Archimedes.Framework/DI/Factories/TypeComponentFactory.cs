using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Archimedes.Framework.AOP;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.DI.Factories
{


    public class TypeComponentFactory : ComponentFactoryBase
    {
        private readonly Type _implementationType;

        public TypeComponentFactory(Type implementationType, string implementationName, bool isPrimary, Type[] primaryForTypes)
            :base(implementationName, isPrimary, primaryForTypes)
        {
            if(implementationType == null) throw new ArgumentNullException("implementationType");

            _implementationType = implementationType;
        }


        public override object CreateInstance(ElderBox ctx, ISet<Type> unresolvedDependencies, object[] providedParameters = null)
        {
            if (unresolvedDependencies == null) throw new ArgumentNullException("unresolvedDependencies");
            if (providedParameters == null) providedParameters = new object[0];


            // Try to create an instance of the given type.

            var allConstructors = _implementationType.GetConstructors(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic);

            var constructor = (from c in allConstructors
                               where c.GetCustomAttributes(typeof(InjectAttribute), false).Any()
                               select c).FirstOrDefault();

            if (constructor == null)
            {
                // There was no Constructor with the Inject Attribute.
                // Just get the first available one
                constructor = (from c in allConstructors
                               where c.IsPublic || !c.IsPrivate
                               select c).FirstOrDefault() ?? allConstructors.FirstOrDefault();
            }

            if (constructor != null)
            {
                // We have found a constructor

                var rawInstance = FormatterServices.GetUninitializedObject(_implementationType);

                ctx.Autowire(rawInstance, unresolvedDependencies);

                var parameters = ctx.AutowireParameters(_implementationType, constructor.GetParameters(), unresolvedDependencies, providedParameters);
                constructor.Invoke(rawInstance, parameters);
                return rawInstance;
            }

            throw new NotSupportedException("Can not create an instance for type " + _implementationType.Name + " - no viable (public/protected) constructor in the available " + allConstructors.Count() + " found!");
        }

        public override string ToString()
        {
            return _implementationType.ToString();
        }
    }

}
