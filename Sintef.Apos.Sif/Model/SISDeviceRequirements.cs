using Sintef.Apos.Sif.Model.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Boolean = Sintef.Apos.Sif.Model.Attributes.Boolean;
using String = Sintef.Apos.Sif.Model.Attributes.String;

namespace Sintef.Apos.Sif.Model
{
    public class SISDeviceRequirements : Node
    {
        public DurationHours MaximumTestIntervalForSILCompliance { get; protected set; } //1
        public TagName TagName { get; protected set; }
        public Percent PFDBudget { get; protected set; }
        public String TagInformation { get; protected set; }
        public String SafeStateOfDevice { get; protected set; } //5
        public TypeAB TypeAOrB { get; protected set; }
        public String EquipmentType { get; protected set; }
        public Percent PFHBudget { get; protected set; }
        //public String RequirementsForTesting { get; protected set; }
        public SCLevel SystematicCapabilityRequirement { get; protected set; } //10
        public Percent TestCoverage { get; protected set; }
        public DurationHours MeanTimeToRestoration { get; protected set; }
        public DurationHours MinimumTestIntervalOperatorSpecification { get; protected set; }
        public DurationSeconds MaximumAllowableSISDeviceResponseTime { get; protected set; }
        public DurationHours ExpectedRepairTime { get; protected set; } //15
        public String NumericFailCriterionValue { get; protected set; }
        public Comparison NumericFailCriterionOperator { get; protected set; }
        public String NumericFailCriterionDescriptionOfMeasurement { get; protected set; }
        public Boolean BypassAdministrativeControlRequired { get; protected set; }
        public DurationSeconds MaximumAllowableBypassTime { get; protected set; } //20
        public TripEnergyMode TripEnergyMode { get; protected set; }
        public FailurePhilosophy FailurePhilosophy { get; protected set; }
        public EnvironmentalExtremes EnvironmentalExtremes { get; protected set; }
        public DurationHours MaximumPermittedRepairTime { get; protected set; } //24

        public SISDeviceRequirements(Group parent, string name, int expectedNumberOfAttributes) : base(parent, name)
        {
            SetAttributes(Definition.GetAttributes(this, expectedNumberOfAttributes + 23));

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

    }
}
