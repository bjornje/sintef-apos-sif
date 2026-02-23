using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Sintef.Apos.Sif.Model.Attributes
{
    public abstract class AttributeType
    {
        public string Name { get; }
        public string Description { get; }
        public string StringValue { get; set; }
        public string RefAttributeType { get; }
        public bool IsMandatory { get; }
        public Type DataType { get; }
        public Node Owner { get; }
        public virtual object ObjectValue { get => GetValueAsObject(StringValue, DataType); set => StringValue = GetValueAsString(value); }

        protected AttributeType(string name, string description, string refAttributeType, bool isMandatory, Type dataType, Node owner)
        {
            Name = name;
            Description = description;
            RefAttributeType = refAttributeType;
            IsMandatory = isMandatory;
            DataType = dataType;
            Owner = owner;
        }

        public abstract AttributeType Clone(Node owner);

        public virtual bool IsValid(out Collection<ModelError> errors)
        {
            errors = new Collection<ModelError>();

            Validate(Owner, errors);

            return errors.Count == 0;
        }

        public virtual void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(StringValue))
            {
                if (IsMandatory)
                {
                    errors.Add(new ModelError(node, this, $"{Name} must have a value."));
                }

                return;
            }

            if (DataType == typeof(decimal))
            {
                var valueAsObject = GetValueAsObject(StringValue, DataType);
                var valueAsString = GetValueAsString(valueAsObject);

                if (StringValue == valueAsString)
                {
                    return;
                }

                if (!StringValue.Contains('E'))
                {
                    valueAsString = GetValueAsStringNotScientific(valueAsObject);

                    if (StringValue == valueAsString)
                    {
                        return;
                    }
                }

                errors.Add(new ModelError(node, this, $"The value {StringValue} provided for {Name} is not a valid decimal number."));
            }
            else if (DataType == typeof(long))
            {
                var valueAsObject = GetValueAsObject(StringValue, DataType);
                var valueAsString = GetValueAsString(valueAsObject);

                if (StringValue == valueAsString)
                {
                    return;
                }

                errors.Add(new ModelError(node, this, $"The value {StringValue} provided for {Name} is not a valid integer number."));
            }
            else if (DataType == typeof(bool))
            {
                var valueAsObject = GetValueAsObject(StringValue, DataType);
                var valueAsString = GetValueAsString(valueAsObject);

                if (StringValue == valueAsString)
                {
                    return;
                }

                errors.Add(new ModelError(node, this, $"The value {StringValue} provided for {Name} is not a valid boolean."));
            }
            else if (DataType == typeof(string))
            {
                if (string.IsNullOrEmpty(StringValue))
                {
                    return;
                }

                if (!Definition.TryGetAttributeTypeValues(RefAttributeType, out var values))
                {
                    return;
                }

                if (!values.Contains(StringValue))
                {
                    errors.Add(new ModelError(node, this, $"The value {StringValue} is not valid for {Name}."));
                }
            }
        }

        private static object GetValueAsObject(string value, Type dataType)
        {
            if (dataType == typeof(decimal))
            {
                if (HasExponent(value, out var significand, out var exponent))
                {
                    if (double.TryParse(significand, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleSignificand) &&
                        int.TryParse(exponent, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intExponent))
                    {
                        return doubleSignificand * Math.Pow(10, intExponent);
                    }
                }
                else
                {
                    if (double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue)) return doubleValue;
                    if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var decimalValue)) return decimalValue;
                }
            }
            else if (dataType == typeof(long))
            {
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue)) return intValue;
                if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue)) return longValue;
            }
            else if (dataType == typeof(bool))
            {
                if (bool.TryParse(value, out var boolValue)) return boolValue;
            }
            else if (dataType == typeof(string)) return value;

            return null;
        }

        private static bool HasExponent(string value, out string significand, out string exponent)
        {
            significand = null;
            exponent = null;

            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            var split = value.Split('E');

            if (split.Length < 2)
            {
                return false;
            }

            significand = split[0];
            exponent = split[1];

            return true;
        }

        private static string GetValueAsString(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is double doubleValue)
            {
                return doubleValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is decimal decimalValue)
            {
                return decimalValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is int intValue)
            {
                return intValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is long longValue)
            {
                return longValue.ToString(CultureInfo.InvariantCulture);
            }
            else if (value is bool boolValue)
            {
                return boolValue.ToString(CultureInfo.InvariantCulture).ToLower();
            }
            else
            {
                return value.ToString();
            }
        }

        private static string GetValueAsStringNotScientific(object value)
        {
            if (value == null)
            {
                return null;
            }

            if (value is double doubleValue)
            {
                return doubleValue.ToString("0." + new string('#', 339));
            }
            else
            {
                return value.ToString();
            }
        }

    }
}
