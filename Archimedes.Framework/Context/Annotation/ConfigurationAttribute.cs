using System;

namespace Archimedes.Framework.Context.Annotation
{
    /// <summary>
    /// 
    /// Indicates that the class has some specific configuration annotations
    /// and / or specail component factory methods.
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ConfigurationAttribute : Attribute
    {

    }
}
