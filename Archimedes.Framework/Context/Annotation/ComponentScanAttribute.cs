using System;

namespace Archimedes.Framework.Context.Annotation
{

    /// <summary>
    /// Indicates that the component scan should be limited to the given include-filter
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentScanAttribute : Attribute
    {
        private readonly string _includeFilterRegex;

        public ComponentScanAttribute(string includeFilterRegex)
        {
            _includeFilterRegex = includeFilterRegex;
        }

        /// <summary>
        /// Gets the include filter regex pattern for assemblies to scan.
        /// Use ',' as delemiter if you need multiple patterns.
        /// </summary>
        public string IncludeFilterRegex
        {
            get { return _includeFilterRegex; }
        }
    }
}
