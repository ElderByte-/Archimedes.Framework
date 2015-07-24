using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using log4net;

namespace Archimedes.Framework.AOP
{
    /// <summary>
    /// Scans for types which have one of the specified attributes.
    /// </summary>
    public class AttributeScanner
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly Type[] _matchAttributes;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new AttributeScanner which scans for the given attributes.
        /// </summary>
        /// <param name="matchAttributes"></param>
        public AttributeScanner(params Type[] matchAttributes)
        {
            _matchAttributes = matchAttributes;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Scans for all types in all assemblis matching the assembly filters
        /// </summary>
        /// <param name="regexAssemblyFilters">The assembly filter regular expression (pattern)</param>
        /// <returns></returns>
        public ISet<Type> ScanByAttribute(params string[] regexAssemblyFilters)
        {
            var foundTypes = new HashSet<Type>();

            if (regexAssemblyFilters == null || regexAssemblyFilters.Length == 0)
            {
                Log.Warn("Assembly Attribute-Scanning is NOT restrictend and all Assemblies will therefore be scanned. For performance reasons, you should limit the scan to your libraries.");
            }
            else
            {
                Log.Info("Assembly Attribute-Scanning restricted to: " + string.Join(", ", regexAssemblyFilters));
            }


            EnsureAssembliesAreLoadedForComponentScan();

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var ignoredAssemblies = new List<string>();

            foreach (var assembly in assemblies)
            {
                if (ScanAssembly(assembly, regexAssemblyFilters))
                {
                    Log.Debug("==  Scanning assembly " + assembly.GetName().Name + "  ==");
                    var components = FindTypesWithAttribute(assembly, _matchAttributes);

                    foreach (var component in components)
                    {
                        Log.Debug("      * " + component.Name);
                        foundTypes.Add(component);
                    }
                    Log.Debug(" == == ");
                }
                else
                {
                    ignoredAssemblies.Add(assembly.GetName().Name);
                }
            }

            Log.Debug(string.Format("Ignored assemblies: {0}", Environment.NewLine + string.Join(Environment.NewLine, ignoredAssemblies)));

            return foundTypes;
        }

        #endregion

        #region Private methods


        private bool ScanAssembly(Assembly assembly, string[] regexAssemblyFilters)
        {
            if (regexAssemblyFilters.Length == 0) return true;

            foreach (var regexAssemblyFilter in regexAssemblyFilters)
            {
                if (regexAssemblyFilter != null && Regex.IsMatch(assembly.GetName().Name, regexAssemblyFilter, RegexOptions.IgnoreCase))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Finds all types which have one of the given attribute types
        /// </summary>
        /// <param name="assembly"></param>
        /// <param name="attributeTypes"></param>
        /// <returns></returns>
        private IEnumerable<Type> FindTypesWithAttribute(Assembly assembly, params Type[] attributeTypes)
        {
            foreach (var t in assembly.GetTypes())
            {
                foreach (var attribute in attributeTypes)
                {
                    if (t.IsDefined(attribute, false))
                    {
                        yield return t;
                        break;
                    }
                }
            }
        }

        private void EnsureAssembliesAreLoadedForComponentScan()
        {
            var loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();

            loadedAssemblies
                .SelectMany(x => x.GetReferencedAssemblies())
                .Distinct()
                .Where(y => loadedAssemblies.Any((a) => a.FullName == y.FullName) == false)
                .ToList()
                .ForEach(x => loadedAssemblies.Add(AppDomain.CurrentDomain.Load(x)));
        }

        #endregion
    }
}
