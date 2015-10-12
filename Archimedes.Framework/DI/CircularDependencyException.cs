using System;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Thrown when autowiring (injecting dependencies) has failed due to a circular dependency loop.
    /// </summary>
    public class CircularDependencyException : AutowireException
    {

        public CircularDependencyException(Type a, Type b) : this(a.Name + " depends on " + b.Name + " and vice versa (circular dependency). Fix that!")
        {
            
        }

        public CircularDependencyException(object a, Type b)
            : this(a + " depends on " + b.Name + " and vice versa (circular dependency). Fix that!")
        {

        }

        public CircularDependencyException(string message) : base(message)
        {
            
        }
    }
}
