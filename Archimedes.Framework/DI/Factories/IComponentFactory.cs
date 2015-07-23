using System;
using System.Collections.Generic;

namespace Archimedes.Framework.DI.Factories
{
    /// <summary>
    /// Represents a factory which is able to create an instance
    /// </summary>
    public interface IComponentFactory
    {
        /// <summary>
        /// If this implementation is named, get the name of it.
        /// Otherwise, it will return null.
        /// </summary>
        string ImplementationName { get; }

        /// <summary>
        /// Returns true if this implementaiton is primary
        /// </summary>
        bool IsPrimary { get; }

        /// <summary>
        /// 
        /// </summary>
        Type[] PrimaryForTypes { get; }

        /// <summary>
        /// Creates a new instance of the type using the given context
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="unresolvedDependencies"></param>
        /// <param name="providedParameters"></param>
        /// <returns></returns>
        object CreateInstance(ElderBox ctx, HashSet<Type> unresolvedDependencies, object[] providedParameters = null);
    }


}
