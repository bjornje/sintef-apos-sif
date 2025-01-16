using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public abstract class AttributeType
    {
        public string Name { get; }
        public string Description { get; }
        public string Value { get; set; }
        public string RefAttributeType { get; }


        public AttributeType(string name, string description, string refAttributeType)
        {
            Name = name;
            Description = description;
            RefAttributeType = refAttributeType;
        }

        public abstract AttributeType Clone();

        public abstract void Validate(Node node, Collection<ModelError> errors);
    }

    public class String : AttributeType
    {
        public String(string name, string description) : base(name, description, "Types/String")
        {

        }

        public override AttributeType Clone()
        {
            return new String(Name, Description);
        }

        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (Name == "SIFID" && node is SIF)
            {
                if (string.IsNullOrEmpty(Value)) errors.Add(new ModelError(node, $"{Name} must have a value."));
            }
        }
    }
    public class Frequecy : AttributeType
    {
        public Frequecy(string name, string description) : base(name, description, "Types/Frequecy")
        {

        }

        public override AttributeType Clone()
        {
            return new Frequecy(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (double.TryParse(Value, out var value)) return;
            errors.Add(new ModelError(node, $"The value {Value} provided for {Name} is not a decimal number."));
        }
    }
    public class Integer : AttributeType
    {
        public Integer(string name, string description) : base(name, description, "Types/Integer")
        {

        }

        public override AttributeType Clone()
        {
            return new Integer(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (long.TryParse(Value, out var value)) return;
            errors.Add(new ModelError(node, $"The value {Value} provided for {Name} is not an integer number."));
        }
    }
    public class Hours : AttributeType
    {
        public Hours(string name, string description) : base(name, description, "Types/Hours")
        {

        }

        public override AttributeType Clone()
        {
            return new Hours(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (double.TryParse(Value, out var value)) return;
            errors.Add(new ModelError(node, $"The value {Value} provided for {Name} is not a decimal number."));
        }
    }
    public class FITs : AttributeType
    {
        public FITs(string name, string description) : base(name, description, "Types/FITs")
        {

        }

        public override AttributeType Clone()
        {
            return new FITs(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
        }
    }
    public class SILLevel : AttributeType
    {
        public SILLevel(string name, string description) : base(name, description, "Types/SILLevel")
        {

        }

        public override AttributeType Clone()
        {
            return new SILLevel(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (!Definition.TryGetAttributeTypeValues(GetType().Name, out var values)) return;
            if (!values.Contains(Value)) errors.Add(new ModelError(node, $"The value {Value} is not valid for {Name}."));
        }
    }
    public class SIFMaximumAllowedResponseTime : AttributeType
    {
        public SIFMaximumAllowedResponseTime(string name, string description) : base(name, description, "Types/SIFMaximumAllowedResponseTime")
        {

        }

        public override AttributeType Clone()
        {
            return new SIFMaximumAllowedResponseTime(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
        }
    }
    public class InputDeviceTrigger : AttributeType
    {
        public InputDeviceTrigger(string name, string description) : base(name, description, "Types/InputDeviceTrigger")
        {

        }

        public override AttributeType Clone()
        {
            return new InputDeviceTrigger(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (!Definition.TryGetAttributeTypeValues(GetType().Name, out var values)) return;
            if (!values.Contains(Value)) errors.Add(new ModelError(node, $"The value {Value} is not valid for {Name}."));
        }
    }
    public class FinalElementFunction : AttributeType
    {
        public FinalElementFunction(string name, string description) : base(name, description, "Types/FinalElementFunction")
        {

        }

        public override AttributeType Clone()
        {
            return new FinalElementFunction(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (!Definition.TryGetAttributeTypeValues(GetType().Name, out var values)) return;
            if (!values.Contains(Value)) errors.Add(new ModelError(node, $"The value {Value} is not valid for {Name}."));
        }
    }
    public class FailSafePosition : AttributeType
    {
        public FailSafePosition(string name, string description) : base(name, description, "Types/FailSafePosition")
        {

        }

        public override AttributeType Clone()
        {
            return new FailSafePosition(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (!Definition.TryGetAttributeTypeValues(GetType().Name, out var values)) return;
            if (!values.Contains(Value)) errors.Add(new ModelError(node, $"The value {Value} is not valid for {Name}."));
        }
    }
    public class Percent : AttributeType
    {
        public Percent(string name, string description) : base(name, description, "Types/Percent")
        {

        }

        public override AttributeType Clone()
        {
            return new Percent(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (double.TryParse(Value, out var value)) return;
            errors.Add(new ModelError(node, $"The value {Value} provided for {Name} is not a decimal number."));
        }
    }
    public class Seconds : AttributeType
    {
        public Seconds(string name, string description) : base(name, description, "Types/Seconds")
        {

        }

        public override AttributeType Clone()
        {
            return new Seconds(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (double.TryParse(Value, out var value)) return;
            errors.Add(new ModelError(node, $"The value {Value} provided for {Name} is not a decimal number."));
        }
    }

    public class E_DEToTrip : AttributeType
    {
        public E_DEToTrip(string name, string description) : base(name, description, "Types/E_DEToTrip")
        {

        }

        public override AttributeType Clone()
        {
            return new E_DEToTrip(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
            if (string.IsNullOrEmpty(Value)) return;
            if (!Definition.TryGetAttributeTypeValues(GetType().Name, out var values)) return;
            if (!values.Contains(Value)) errors.Add(new ModelError(node, $"The value {Value} is not valid for {Name}."));
        }
    }

    public class ProofTestInterval : AttributeType
    {
        public ProofTestInterval(string name, string description) : base(name, description, "Types/ProofTestInterval")
        {

        }

        public override AttributeType Clone()
        {
            return new ProofTestInterval(Name, Description);
        }


        public override void Validate(Node node, Collection<ModelError> errors)
        {
        }
    }
}
