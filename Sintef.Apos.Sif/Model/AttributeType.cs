﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sintef.Apos.Sif.Model
{
    public abstract class AttributeType
    {
        public string Name { get; }
        public string Description { get; }
        public string StringValue { get; set; }
        public string RefAttributeType { get; }
        public Type DataType { get; }
        public virtual object ObjectValue { get => GetValueAsObject(StringValue, DataType); set => StringValue = GetValueAsString(value); }

        protected AttributeType(string name, string description, string refAttributeType, Type dataType)
        {
            Name = name;
            Description = description;
            RefAttributeType = refAttributeType;
            DataType = dataType;
        }

        public abstract AttributeType Clone();

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
            else if (value is bool boolValue) return boolValue.ToString(CultureInfo.InvariantCulture);
            else return value.ToString();
        }

    }

    public class String : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public String(string name, string description) : base(name, description, "Types/String", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new String(Name, Description);
        }

        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (Name == "SIFID" && node is SIF && string.IsNullOrEmpty(StringValue))
            {
                errors.Add(new ModelError(node, $"{Name} must have a value."));
            }
        }
    }
    public class Frequecy : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public Frequecy(string name, string description) : base(name, description, "Types/Frequecy", typeof(decimal))
        {

        }

        public override AttributeType Clone()
        {
            return new Frequecy(Name, Description);
        }
    }
    public class Integer : AttributeType
    {
        public int? Value { get => ObjectValue as int?; set => ObjectValue = value; }
        public Integer(string name, string description) : base(name, description, "Types/Integer", typeof(long))
        {

        }

        public override AttributeType Clone()
        {
            return new Integer(Name, Description);
        }
    }
    public class Boolean : AttributeType
    {
        public bool? Value { get => ObjectValue as bool?; set => ObjectValue = value; }
        public Boolean(string name, string description) : base(name, description, "Types/Boolean", typeof(bool))
        {

        }

        public override AttributeType Clone()
        {
            return new Boolean(Name, Description);
        }
    }
    public class Hours : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public Hours(string name, string description) : base(name, description, "Types/Hours", typeof(decimal))
        {

        }

        public override AttributeType Clone()
        {
            return new Hours(Name, Description);
        }
    }
    public class FITs : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public FITs(string name, string description) : base(name, description, "Types/FITs", typeof(decimal))
        {

        }

        public override AttributeType Clone()
        {
            return new FITs(Name, Description);
        }
    }
    public class SILLevel : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public SILLevel(string name, string description) : base(name, description, "Types/SILLevel", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new SILLevel(Name, Description);
        }
    }
    public class InputDeviceTrigger : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public InputDeviceTrigger(string name, string description) : base(name, description, "Types/InputDeviceTrigger", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new InputDeviceTrigger(Name, Description);
        }
    }
    public class FinalElementFunction : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public FinalElementFunction(string name, string description) : base(name, description, "Types/FinalElementFunction", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new FinalElementFunction(Name, Description);
        }
    }
    public class FailSafePosition : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public FailSafePosition(string name, string description) : base(name, description, "Types/FailSafePosition", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new FailSafePosition(Name, Description);
        }
    }
    public class Percent : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public Percent(string name, string description) : base(name, description, "Types/Percent", typeof(decimal))
        {

        }

        public override AttributeType Clone()
        {
            return new Percent(Name, Description);
        }
    }
    public class Seconds : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public Seconds(string name, string description) : base(name, description, "Types/Seconds", typeof(decimal))
        {

        }

        public override AttributeType Clone()
        {
            return new Seconds(Name, Description);
        }

    }

    public class E_DEToTrip : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public E_DEToTrip(string name, string description) : base(name, description, "Types/E_DEToTrip", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new E_DEToTrip(Name, Description);
        }
    }

    public class PerYear : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public PerYear(string name, string description) : base(name, description, "Types/PerYear", typeof(decimal))
        {

        }

        public override AttributeType Clone()
        {
            return new PerYear(Name, Description);
        }
    }
    public class SIFType : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public SIFType(string name, string description) : base(name, description, "Types/SIFType", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new SIFType(Name, Description);
        }
    }
    public class ModeOfOperation : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public ModeOfOperation(string name, string description) : base(name, description, "Types/ModeOfOperation", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new ModeOfOperation(Name, Description);
        }
    }

    public class ILLevel : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public ILLevel(string name, string description) : base(name, description, "Types/ILLevel", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new ILLevel(Name, Description);
        }
    }

    public class PerHour : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public PerHour(string name, string description) : base(name, description, "Types/PerHour", typeof(decimal))
        {

        }

        public override AttributeType Clone()
        {
            return new PerHour(Name, Description);
        }
    }

    public class ManualActivation : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public ManualActivation(string name, string description) : base(name, description, "Types/ManualActivation", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new ManualActivation(Name, Description);
        }
    }

    public class ResetAfterShutdown_FinalElement : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public ResetAfterShutdown_FinalElement(string name, string description) : base(name, description, "Types/ResetAfterShutdown_FinalElement", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new ResetAfterShutdown_FinalElement(Name, Description);
        }
    }

    public class AlarmOrWarning : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public AlarmOrWarning(string name, string description) : base(name, description, "Types/AlarmOrWarning", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new AlarmOrWarning(Name, Description);
        }
    }


    public class TagNumber : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public TagNumber(string name, string description) : base(name, description, "Types/TagNumber", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new TagNumber(Name, Description);
        }
    }

    public class TypeAB : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public TypeAB(string name, string description) : base(name, description, "Types/TypeAB", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new TypeAB(Name, Description);
        }
    }

    public class Comparison : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public Comparison(string name, string description) : base(name, description, "Types/Comparison", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new Comparison(Name, Description);
        }
    }


    public class BypassControl : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public BypassControl(string name, string description) : base(name, description, "Types/BypassControl", typeof(string))
        {

        }

        public override AttributeType Clone()
        {
            return new BypassControl(Name, Description);
        }
    }


    public class kg_s : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public kg_s(string name, string description) : base(name, description, "Types/kg_s", typeof(decimal))
        {

        }

        public override AttributeType Clone()
        {
            return new kg_s(Name, Description);
        }
    }


}
