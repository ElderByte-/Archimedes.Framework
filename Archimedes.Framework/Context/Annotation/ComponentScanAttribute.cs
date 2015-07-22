using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archimedes.Framework.Context.Annotation
{
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
        /// </summary>
        public string IncludeFilterRegex
        {
            get { return _includeFilterRegex; }
        }
    }
}
