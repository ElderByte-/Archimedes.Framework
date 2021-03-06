﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Archimedes.Patterns;
using Archimedes.Patterns.Utils;

namespace Archimedes.Framework.ContextEnvironment.Properties
{
    /// <summary>
    /// Represents properties as a simple key-value store.
    /// Provides several convinience methods to access and check property values.
    /// </summary>
    public class PropertyStore
    {
        private readonly Dictionary<string, string> _parameters = new Dictionary<string, string>();

        /// <summary>
        /// Creates an empty properties object.
        /// </summary>
        public PropertyStore()
        {
            
        }

        /// <summary>
        /// Adds a flag with a value (option)
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="value"></param>
        public void Set(string parameter, string value)
        {
            var param = parameter.ToLower();
            if (_parameters.ContainsKey(param))
            {
                _parameters[parameter] = value;
            }
            else
            {
                _parameters.Add(param, value);

            }
        }

        /// <summary>
        /// Returns the value of the given parameter.
        /// If the parameter is not present, throws a <see cref="UnknownParameterException"/> exception.
        /// 
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string Get(string parameter)
        {
            var value = GetOptional(parameter);

            if (value.IsPresent)
            {
                return value.Value;
            }
            throw new UnknownParameterException(parameter);
        }

        /// <summary>
        /// Returns the value of the given parameter, or null, if the parameter
        /// is not defined.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Optional<string> GetOptional(string parameter)
        {
            if (_parameters.ContainsKey(parameter.ToLower()))
            {
                return Optional.OfNullable(_parameters[parameter.ToLower()]);
            }
            return Optional.Empty<string>();
        }

        /// <summary>
        /// Returns the properties value parsed as T.
        /// If the parameter does not exists or can not be parsed to a T, returns an Empty Optional
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public Optional<T> GetOptional<T>(string parameter)
        {
            return GetOptional(parameter).FlatMap( x => ParseUtil.ParseSave<T>(x) );
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Optional<string> GetOptional(params string[] parameters)
        {
            foreach (string param in parameters)
            {
                var opt = GetOptional(param);
                if (opt.IsPresent) return opt; // return the first present parameter value
            }
            return Optional.Empty<string>();
        }


        /// <summary>
        /// A short hand to check if a parameter is set to "true".
        /// If the parameter is false, or not set at all, returns false.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public bool IsTrue(string parameter)
        {
            return GetOptional(parameter).Map(value => true.ToString().ToLower().Equals(value.ToLower())).OrElse(false);
        }


        /// <summary>
        /// Merges the given parameter set into this configuration
        /// </summary>
        /// <param name="parameters"></param>
        public PropertyStore Merge(IDictionary<string, string> parameters)
        {
            foreach (var kv in parameters)
            {
                Set(kv.Key, kv.Value);
            }
            return this;
        }

        public PropertyStore Merge(PropertyStore other)
        {
            return Merge(other._parameters);
        }

        public IDictionary<string, string> ToKeyValuePairs()
        {
            return new Dictionary<string, string>(_parameters);
        }

        public override string ToString()
        {
            return _parameters.Aggregate("", (current, kv) =>
                current + (kv.Key + "=" + kv.Value + Environment.NewLine)
                );
        }
    }
}
