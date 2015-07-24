using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Archimedes.Framework.SampleApp.Util
{
    public static class AppUtil
    {
        /// <summary>
        /// Gets the applications binary folder, i.e. the folder where the exe resides.
        /// </summary>
        /// <exception cref="NotSupportedException">Thrown when the binary folder could not be retrived.</exception>
        public static string ApplicaitonBinaryFolder
        {
            get
            {
                var assembly = Assembly.GetEntryAssembly();
                if (assembly != null)
                {
                    return Path.GetDirectoryName(assembly.Location);
                }
                throw new NotSupportedException("Entry Assembly not available! This may occur in specail circumstances, such as when running as Unit Test.");
            }
        }

    }
}
