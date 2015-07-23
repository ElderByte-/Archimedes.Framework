using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Archimedes.Framework.AOP;
using Archimedes.Framework.DI.Factories;
using Archimedes.Framework.Stereotype;

namespace Archimedes.Framework.DI
{
    /// <summary>
    /// Holds all (named) implementation types of an interface.
    /// </summary>
    internal class ImplementationRegistry
    {
        #region Fields

        private readonly Type _iface; // Just used for better exception texts
        private readonly List<IComponentFactory> _anonymousImpls = new List<IComponentFactory>();
        private readonly Dictionary<string, IComponentFactory> _namedImpls = new Dictionary<string, IComponentFactory>();

        private IComponentFactory _primary = null;

        #endregion

        #region Constructor

        public ImplementationRegistry(Type iface)
        {
            if (iface == null) throw new ArgumentNullException("iface");

            _iface = iface;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Try to get the implementation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public IComponentFactory TryGetImplementation(string name = null)
        {
            // First priority is a named implementation
            if (name != null)
            {
                return FindImplementationByName(name);
            }

            // Second priority is a primary service
            if (_primary != null)
            {
                return _primary;
            }

            // Last priority are anonymous implementations
            if (_anonymousImpls.Count == 0)
            {
                if (_namedImpls.Count == 1)
                {
                    return _namedImpls.First().Value;
                }
                throw new NotSupportedException("Could not find an implemnetation for type " + _iface.Name);
            }
            else if (_anonymousImpls.Count == 1)
            {
                return _anonymousImpls.First();
            }
            else
            {
                string implsStr = string.Join(" ", _anonymousImpls);
                throw new AmbiguousMappingException("There is more than one ("+_anonymousImpls.Count+") implementation available for the same type "+_iface+": " + implsStr);
            }
        }

        /// <summary>
        /// Register a new implementation
        /// </summary>
        /// <param name="impl"></param>
        public void Register(IComponentFactory impl)
        {
            if(impl == null) throw new ArgumentNullException("impl");

            if (impl.ImplementationName != null)
            {
                if (!_namedImpls.ContainsKey(impl.ImplementationName))
                {
                    _namedImpls.Add(impl.ImplementationName, impl);
                    _anonymousImpls.Add(impl);
                }
                else
                {
                    var offendingImpl = _namedImpls[impl.ImplementationName];
                    throw new AmbiguousMappingException("A implementation of " + _iface.Name + " has ambiguous name " + impl.ImplementationName + " which already exists! Implementation " + offendingImpl + " collides with " + impl + "!");
                }
            }
            else
            {
                _anonymousImpls.Add(impl);
            }

            RegisterPrimary(impl);
        }

        #endregion

        #region Private methods

        private void RegisterPrimary(IComponentFactory impl)
        {
            if (impl.IsPrimary)
            {
                // The Implementation has the Primary Attribute
                if (IsPrimaryForThisIface(impl))
                {
                    if (_primary == null)
                    {
                        _primary = impl;
                    }
                    else
                    {
                        throw new AmbiguousMappingException("The [Primary] annotation is used on more than one available implementations: " + _primary + " and " + impl);
                    }
                }
            }
        }

        /// <summary>
        /// The primary attribute allows to specify specific interface types
        /// for which it overrules.
        /// </summary>
        /// <param name="impl"></param>
        /// <returns></returns>
        private bool IsPrimaryForThisIface(IComponentFactory impl)
        {
            if (impl.PrimaryForTypes == null || impl.PrimaryForTypes.Length == 0) return true;

            return impl.PrimaryForTypes.Any(primaryApplies => _iface == primaryApplies);
        }


        /// <summary>
        /// Try to find an implementation with the given name.
        /// Will throw an exception if no implementation was found.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private IComponentFactory FindImplementationByName(string name)
        {
            if (_namedImpls.ContainsKey(name))
            {
                return _namedImpls[name];
            }
            else
            {
                throw new NotSupportedException("Could not find a named implemnetation '" + name + "' of type " + _iface.Name);
            }
        }

        #endregion


        public override string ToString()
        {
            if (_primary != null) return _primary.ToString();

            string allImpl = "";
            foreach (var impl in _anonymousImpls)
            {
                allImpl += impl.ToString() + " | ";
            }
            return allImpl;
        }
    }
}
