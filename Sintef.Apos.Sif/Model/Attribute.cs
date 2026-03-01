using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class Attribute<T> : IAttribute
    {
        public string Name { get; }
        public string Description { get; }
        public string StringValue { get; set; }
        public string RefAttributeType { get; }
        public bool IsMandatory { get; }
        public bool IsPartOfModel { get; }
        public bool IsOrderedList => false;
        public Collection<IAttribute> Items { get; }
        public Node Owner { get; }
        public string[] ValueOptions { get; set; }
        public object ObjectValue { get => GetValueAsObject(StringValue); set => StringValue = GetValueAsString(value); }

        public T Value { get => (T)ObjectValue; set => ObjectValue = value; }
        public Attribute(string name, string description, string refAttributeType, bool isMandatory, bool isPartOfModel, Node owner)
        {
            Name = name;
            Description = description;
            RefAttributeType = refAttributeType;
            IsMandatory = isMandatory;
            IsPartOfModel = isPartOfModel;
            Owner = owner;
        }

        public IAttribute Clone(Node owner)
        {
            return new Attribute<T>(Name, Description, RefAttributeType, IsMandatory, IsPartOfModel, owner);
        }

        public IAttribute CreateItem()
        {
            return null;
        }

        public bool TryGetValueOptions(out string[] valueOptions)
        {
            if (ValueOptions == null)
            {
                valueOptions = null;
                return false;
            }

            valueOptions = ValueOptions;

            return true;
        }

        public bool IsValid(out Collection<ModelError> errors)
        {
            errors = new Collection<ModelError>();

            Validate(Owner, errors);

            return errors.Count == 0;
        }

        public void Validate(Node node, Collection<ModelError> errors)
        {

            if (string.IsNullOrEmpty(StringValue))
            {
                if (IsMandatory)
                {
                    errors.Add(new ModelError(node, this, $"{Name} must have a value."));
                }

                return;
            }

            if (typeof(T) == typeof(double?))
            {
                var valueAsObject = GetValueAsObject(StringValue);
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
            else if (typeof(T) == typeof(int?))
            {
                var valueAsObject = GetValueAsObject(StringValue);
                var valueAsString = GetValueAsString(valueAsObject);

                if (StringValue == valueAsString)
                {
                    return;
                }

                errors.Add(new ModelError(node, this, $"The value {StringValue} provided for {Name} is not a valid integer number."));
            }
            else if (typeof(T) == typeof(bool?))
            {
                var valueAsObject = GetValueAsObject(StringValue);
                var valueAsString = GetValueAsString(valueAsObject);

                if (StringValue == valueAsString)
                {
                    return;
                }

                errors.Add(new ModelError(node, this, $"The value {StringValue} provided for {Name} is not a valid boolean."));
            }
            else if (typeof(T) == typeof(string))
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

        private static object GetValueAsObject(string value)
        {
            if (typeof(T) == typeof(double?))
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
                    if (double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue))
                    {
                        return doubleValue;
                    }

                    if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var decimalValue))
                    {
                        return decimalValue;
                    }
                }
            }
            else if (typeof(T) == typeof(int?))
            {
                if (int.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intValue))
                {
                    return intValue;
                }

                if (long.TryParse(value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var longValue))
                {
                    return longValue;
                }
            }
            else if (typeof(T) == typeof(bool?))
            {
                if (bool.TryParse(value, out var boolValue))
                {
                    return boolValue;
                }
            }
            else if (typeof(T) == typeof(string))
            {
                return value;
            }

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
