using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public abstract class AttributeType
    {
        public string Name { get; }
        public string Description { get; }
        public string StringValue { get; set; }
        public string RefAttributeType { get; }
        public Type DataType { get; }
        public Node Owner { get; }
        public virtual object ObjectValue { get => GetValueAsObject(StringValue, DataType); set => StringValue = GetValueAsString(value); }

        protected AttributeType(string name, string description, string refAttributeType, Type dataType, Node owner)
        {
            Name = name;
            Description = description;
            RefAttributeType = refAttributeType;
            DataType = dataType;
            Owner = owner;
        }

        public abstract AttributeType Clone(Node owner);

        public virtual void Validate(Node node, Collection<ModelError> errors)
        {
            if (DataType == typeof(decimal))
            {
                var valueAsObject = GetValueAsObject(StringValue, DataType);
                var valueAsString = GetValueAsString(valueAsObject);
                if (StringValue == valueAsString) return;

                errors.Add(new ModelError(node, $"The value {StringValue} provided for {Name} is not a valid decimal number."));
            }
            else if (DataType == typeof(long))
            {
                var valueAsObject = GetValueAsObject(StringValue, DataType);
                var valueAsString = GetValueAsString(valueAsObject);
                if (StringValue == valueAsString) return;

                errors.Add(new ModelError(node, $"The value {StringValue} provided for {Name} is not a valid integer number."));
            }
            else if (DataType == typeof(bool))
            {
                var valueAsObject = GetValueAsObject(StringValue, DataType);
                var valueAsString = GetValueAsString(valueAsObject);
                if (StringValue == valueAsString) return;

                errors.Add(new ModelError(node, $"The value {StringValue} provided for {Name} is not a valid boolean."));
            }
            else if (DataType == typeof(string))
            {
                if (string.IsNullOrEmpty(StringValue)) return;
                if (!Definition.TryGetAttributeTypeValues(GetType().Name, out var values)) return;
                if (!values.Contains(StringValue)) errors.Add(new ModelError(node, $"The value {StringValue} is not valid for {Name}."));
            }
        }

        private static object GetValueAsObject(string value, Type dataType)
        {
            if (string.IsNullOrEmpty(value)) return null;

            if (dataType == typeof(decimal))
            {
                if (double.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var doubleValue)) return doubleValue;
                if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var decimalValue)) return decimalValue;
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

        private static string GetValueAsString(object value)
        {
            if (value == null) return null;

            if (value is double doubleValue) return doubleValue.ToString(CultureInfo.InvariantCulture);
            else if (value is decimal decimalValue) return decimalValue.ToString(CultureInfo.InvariantCulture);
            else if (value is int intValue) return intValue.ToString(CultureInfo.InvariantCulture);
            else if (value is long longValue) return longValue.ToString(CultureInfo.InvariantCulture);
            else if (value is bool boolValue) return boolValue.ToString(CultureInfo.InvariantCulture).ToLower();
            else return value.ToString();
        }

    }

    public class AssetIntegrityLevel : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public AssetIntegrityLevel(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new AssetIntegrityLevel(Name, Description, RefAttributeType, owner);
        }
    }

    public class Accuracy : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public Accuracy(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Accuracy(Name, Description, RefAttributeType, owner);
        }
    }

    public class Boolean : AttributeType
    {
        public bool? Value { get => ObjectValue as bool?; set => ObjectValue = value; }
        public Boolean(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(bool), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Boolean(Name, Description, RefAttributeType, owner);
        }
    }

    public class Comparison : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public Comparison(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Comparison(Name, Description, RefAttributeType, owner);
        }
    }

    public class DurationHours : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public DurationHours(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new DurationHours(Name, Description, RefAttributeType, owner);
        }
    }

    public class DurationSeconds : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public DurationSeconds(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new DurationSeconds(Name, Description, RefAttributeType, owner);
        }
    }

    public class EnvironmentalExtremes : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public EnvironmentalExtremes(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {
        }

        public override AttributeType Clone(Node owner)
        {
            return new EnvironmentalExtremes(Name, Description, RefAttributeType, owner);
        }
    }

    public class EnvironmentalIntegrityLevel : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public EnvironmentalIntegrityLevel(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {
        }

        public override AttributeType Clone(Node owner)
        {
            return new EnvironmentalIntegrityLevel(Name, Description, RefAttributeType, owner);
        }
    }

    public class FailurePhilosophy : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public FailurePhilosophy(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {
        }

        public override AttributeType Clone(Node owner)
        {
            return new FailurePhilosophy(Name, Description, RefAttributeType, owner);
        }
    }

    public class FrequecyPerHour : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public FrequecyPerHour(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new FrequecyPerHour(Name, Description, RefAttributeType, owner);
        }
    }

    public class FrequecyPerYear : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public FrequecyPerYear(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new FrequecyPerYear(Name, Description, RefAttributeType, owner);
        }
    }

    public class Integer : AttributeType
    {
        public int? Value { get => ObjectValue as int?; set => ObjectValue = value; }
        public Integer(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(long), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Integer(Name, Description, RefAttributeType, owner);
        }
    }

    public class LeakageRate_kg_s : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public LeakageRate_kg_s(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new LeakageRate_kg_s(Name, Description, RefAttributeType, owner);
        }
    }

    public class ModeOfOperation : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public ModeOfOperation(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new ModeOfOperation(Name, Description, RefAttributeType, owner);
        }
    }

    public class Percent : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public Percent(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Percent(Name, Description, RefAttributeType, owner);
        }
    }

    public class Probability : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public Probability(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Probability(Name, Description, RefAttributeType, owner);
        }
    }

    public class RangeMax : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public RangeMax(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new RangeMax(Name, Description, RefAttributeType, owner);
        }
    }

    public class RangeMin : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public RangeMin(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new RangeMin(Name, Description, RefAttributeType, owner);
        }
    }

    public class ResetAfterShutdown_FinalElement : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public ResetAfterShutdown_FinalElement(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new ResetAfterShutdown_FinalElement(Name, Description, RefAttributeType, owner);
        }
    }

    public class SCLevel : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public SCLevel(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new SCLevel(Name, Description, RefAttributeType, owner);
        }
    }

    public class SIFType : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public SIFType(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new SIFType(Name, Description, RefAttributeType, owner);
        }
    }

    public class SILLevel : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public SILLevel(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new SILLevel(Name, Description, RefAttributeType, owner);
        }
    }

    public class String : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public String(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {
        }

        public override AttributeType Clone(Node owner)
        {
            return new String(Name, Description, RefAttributeType, owner);
        }

        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (Name == "SIFID" && node is SIF && string.IsNullOrEmpty(StringValue))
            {
                errors.Add(new ModelError(node, $"{Name} must have a value."));
            }
        }
    }

    public class TagName : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public TagName(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new TagName(Name, Description, RefAttributeType, owner);
        }
    }

    public class TripEnergyMode : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public TripEnergyMode(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new TripEnergyMode(Name, Description, RefAttributeType, owner);
        }
    }

    public class TypeAB : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public TypeAB(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new TypeAB(Name, Description, RefAttributeType, owner);
        }
    }

    public class UnitOfMeasure : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public UnitOfMeasure(string name, string description, string refAttributeType, Node owner) : base(name, description, refAttributeType, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new UnitOfMeasure(Name, Description, RefAttributeType, owner);
        }
    }

}
