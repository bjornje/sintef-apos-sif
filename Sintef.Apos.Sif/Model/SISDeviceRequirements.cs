using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class SISDeviceRequirements : Node
    {
        public AttributeList<string> AdditionalDiagnosticRequirementForImplementation { get; protected set; } //1
        public Attribute<bool?> BypassAdministrativeControlRequired { get; protected set; }
        public AttributeList<string> DiagnosticRequired { get; protected set; }
        public Attribute<string> EnvironmentalExtremes { get; protected set; }
        public Attribute<string> EquipmentType { get; protected set; } //5
        public Attribute<double?> ExpectedRepairTime { get; protected set; }
        public AttributeList<string> FailCriterion { get; protected set; }
        public AttributeList<string> FailureModeResponses { get; protected set; }
        public Attribute<string> FailurePhilosophy { get; protected set; }
        public Attribute<double?> MaximumAllowableBypassTime { get; protected set; } //10
        public Attribute<double?> MaximumAllowableSISDeviceResponseTime { get; protected set; }
        public Attribute<double?> MaximumPermittedRepairTime { get; protected set; }
        public Attribute<double?> MaximumTestIntervalForSILCompliance { get; protected set; }
        public Attribute<double?> MeanTimeToRestoration { get; protected set; }
        public Attribute<double?> MinimumTestIntervalOperatorSpecification { get; protected set; } //15
        public Attribute<string> NumericFailCriterionDescriptionOfMeasurement { get; protected set; }
        public Attribute<string> NumericFailCriterionOperator { get; protected set; }
        public Attribute<string> NumericFailCriterionValue { get; protected set; }
        public Attribute<double?> PFDBudget { get; protected set; }
        public Attribute<double?> PFHBudget { get; protected set; } //20
        public AttributeList<string> RequirementsForTesting { get; protected set; }
        public Attribute<string> SafeStateOfDevice { get; protected set; }
        public Attribute<string> SISDeviceRequirementsVersion { get; protected set; }
        public AttributeList<string> SurvivabilityRequirement { get; protected set; }
        public Attribute<string> SystematicCapabilityRequirement { get; protected set; } //25
        public Attribute<string> TagInformation { get; protected set; }
        public Attribute<string> TagName { get; protected set; }
        public Attribute<double?> TestCoverage { get; protected set; }
        public AttributeList<double?> TimeDelayOfAction { get; protected set; }
        public Attribute<string> TripEnergyMode { get; protected set; } //30
        public Attribute<string> TypeAOrB { get; protected set; } //31

        public SISDeviceRequirements(Group parent, string name, int expectedNumberOfAttributes) : base(parent, name)
        {
            SetAttributes(Definition.GetAttributes(this, expectedNumberOfAttributes + 31));

            TagName.StringValue = name;
        }

        public bool IsSameAs(SISDeviceRequirements component)
        {
            if (string.IsNullOrEmpty(TagName.StringValue) && !string.IsNullOrEmpty(component.TagName.StringValue)) return false;
            if (!string.IsNullOrEmpty(TagName.StringValue) && string.IsNullOrEmpty(component.TagName.StringValue)) return false;
            if (!string.IsNullOrEmpty(TagName.StringValue) && !string.IsNullOrEmpty(component.TagName.StringValue) && TagName.StringValue != component.TagName.StringValue) return false;

            if (!HaveSameAttributeValues(component)) return false;

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var property in Attributes) property.Validate(this, errors);
        }


        public override void PushAttributes()
        {
            foreach (var attribute in Definition.GetAdditionalAttributes(GetType().Name))
            {
                TryAddAttribute(attribute.Clone(this));
            }
        }

    }

    public class SISComponents : IEnumerable<SISDeviceRequirements>
    {
        private readonly Collection<SISDeviceRequirements> _items = new Collection<SISDeviceRequirements>();
        private readonly Group _parent;
        public SISComponents(Group parent)
        {
            _parent = parent;
        }

        public SISDeviceRequirements Append(string name = null)
        {
            SISDeviceRequirements component;

            var parent = _parent as Node;

            while(parent is Group)
            {
                parent = parent.Parent;
            }



            if (parent is InputDeviceSubsystem)
            {
                component = new InputDeviceRequirements(_parent, name);
                component.TagName.StringValue = name ?? "New input component";
            }
            else if (parent is LogicSolverSubsystem)
            {
                component = new LogicSolverRequirements(_parent, name);
                component.TagName.StringValue = name ?? "New solver component";
            }
            else if (parent is FinalElementSubsystem)
            {
                component = new FinalElementRequirements(_parent, name);
                component.TagName.StringValue = name ?? "New final component";
            }
            else
            {
                throw new Exception($"Unexpeced type for having a group: {_parent.GetType()}");
            }

            _items.Add(component); 
            return component;
        }

        public bool Remove(SISDeviceRequirements item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<SISDeviceRequirements> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool IsSameAs(SISComponents components)
        {
            if (_items.Count != components.Count()) return false;

            var alreadyMatchedComponents = new List<SISDeviceRequirements>();

            foreach (var component in components)
            {
                var myComponent = _items.FirstOrDefault(x => !alreadyMatchedComponents.Contains(x) && x.IsSameAs(component));
                if (myComponent == null) return false;
                alreadyMatchedComponents.Add(myComponent);
            }

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var component in _items)
            {
                var duplicates = _items.Where(x => x.TagName.StringValue == component.TagName.StringValue);
                if (duplicates.Count() > 1) errors.Add(new ModelError(component, component.TagName, $"{component.TagName.Name} must be unique."));
                component.Validate(errors);
            }
        }

        public void PushAttributes()
        {
            foreach (var component in _items)
            {
                component.PushAttributes();
            }
        }

    }
}
