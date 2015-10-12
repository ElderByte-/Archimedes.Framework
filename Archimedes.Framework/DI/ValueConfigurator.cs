using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Archimedes.Framework.ContextEnvironment.Properties;
using Archimedes.Framework.Util;
using Archimedes.Patterns.Utils;
using log4net;

namespace Archimedes.Framework.DI
{
    internal class ValueConfigurator
    {
        #region Fields

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Regex _variableReference = new Regex(@"\$\{(.*)\}");
        private readonly PropertyStore _configuration;

        #endregion


        public ValueConfigurator(PropertyStore configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Set the value of the given field to the value of the evaluated expression.
        /// If this fails, no exception is thrown.
        /// </summary>
        /// <param name="field">The field to set</param>
        /// <param name="instance">The instance which field is set</param>
        /// <param name="valueExpression">An expression to evaluate</param>
        /// <returns>Returns true upon success</returns>
        /// <exception cref="ValueConfigurationException"></exception>
        public bool SetValueSave(FieldInfo field, object instance, string valueExpression)
        {
            try
            {
                SetValue(field, instance, valueExpression);
                return true;
            }catch(ValueConfigurationException e)
            {
                if (Log.IsDebugEnabled)
                {
                    Log.Debug(e);
                }
                else
                {
                    // Avoid too much log noise, since this happens when properteis just are not set
                    Log.Warn(e.Message);
                }
            }
            return false;
        }

        /// <summary>
        /// Set the value of the given field to the value of the evaluated expression
        /// </summary>
        /// <param name="field">The field to set</param>
        /// <param name="instance">The instance which field is set</param>
        /// <param name="valueExpression">An expression to evaluate</param>
        /// <exception cref="ValueConfigurationException"></exception>
        public void SetValue(FieldInfo field, object instance, string valueExpression)
        {
            try
            {
                var value = InterpretValueExpression(valueExpression);
                SetValueAutoConvert(field, instance, value);
            }
            catch (Exception e)
            {
                throw new ValueConfigurationException("Failed to set value " + valueExpression + " to field " + field + " at class " + instance.GetType(), e);
            }
        }

        private string InterpretValueExpression(string expression)
        {
            return _variableReference.Replace(expression, match =>
            {
                var variable = match.Groups[1].Value;

                var valueOpt = _configuration.GetOptional(variable);

                if (!valueOpt.IsPresent)
                {
                    Log.Warn("Could not find configuration key '" + variable + "'!");
                }

                return valueOpt.OrDefault();
            });
        }

        /// <summary>
        /// Set the given value to the given field of the given instance
        /// </summary>
        /// <param name="field"></param>
        /// <param name="instance"></param>
        /// <param name="value"></param>
        private static void SetValueAutoConvert(FieldInfo field, object instance, string value)
        {
            if (field.FieldType == typeof(string))
            {
                field.SetValue(instance, value);
            }
            else
            {
                var targetType = field.FieldType;
                var propValue = Convert(value, targetType, CultureInfo.InvariantCulture);
                try
                {
                    field.SetValue(instance, propValue);
                }
                catch (Exception e)
                {
                    throw new NotSupportedException(string.Format("Failed to set field '{0}' to value '{1}'!", field, propValue), e);
                }
            }
        }

        /// <summary>
        /// Converts the given string value
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        private static object Convert(string value, Type targetType, CultureInfo culture)
        {
            try
            {
                if (targetType.IsGenericType && targetType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    if (string.IsNullOrEmpty(value)) return null;
                    var valueType = Nullable.GetUnderlyingType(targetType);
                    return Convert(value, valueType, culture);
                }

                return ParseUtil.Parse(value, targetType, culture);
            }
            catch (FormatException e)
            {
                throw new NotSupportedException("The value configurator does not support converting a string '" + value + "' to your '" + targetType + "' type!", e);
            }

        }
       
    }
}
