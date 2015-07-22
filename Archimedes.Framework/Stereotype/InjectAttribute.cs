using System;

namespace Archimedes.Framework.Stereotype
{
    /// <summary>
    /// Marks a Field or Constructor as Injection target
    /// </summary>
    [AttributeUsage(AttributeTargets.Constructor | AttributeTargets.Field)]
    public class InjectAttribute : Attribute
    {

    }
}
