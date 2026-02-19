using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class SISComponent : Node
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

        public const string RefBaseSystemUnitPath = "SIF Unit Classes/SISComponent";

        public String Name { get; }

        public SISComponent(Group parent, string name, int expectedNumberOfAttributes) : base(parent, name)
        {
            Name = new String(nameof(Name), "", "", this);
            Name.StringValue = name;

            SetAttributes(Definition.GetAttributes(this, expectedNumberOfAttributes + 23));
        }

        public bool IsSameAs(SISComponent component)
        {
            if (string.IsNullOrEmpty(Name.StringValue) && !string.IsNullOrEmpty(component.Name.StringValue)) return false;
            if (!string.IsNullOrEmpty(Name.StringValue) && string.IsNullOrEmpty(component.Name.StringValue)) return false;
            if (!string.IsNullOrEmpty(Name.StringValue) && !string.IsNullOrEmpty(component.Name.StringValue) && Name.StringValue != component.Name.StringValue) return false;

            if (!HaveSameAttributeValues(component)) return false;

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var property in Attributes) property.Validate(this, errors);

            if (string.IsNullOrWhiteSpace(Name.StringValue)) errors.Add(new ModelError(this, "Name must have a value."));
        }

    }

    public class SISComponents : IEnumerable<SISComponent>
    {
        private readonly Collection<SISComponent> _items = new Collection<SISComponent>();
        private readonly Group _parent;
        public SISComponents(Group parent)
        {
            _parent = parent;
        }

        public SISComponent Append(string name = null)
        {
            SISComponent component;

            var parent = _parent as Node;

            while(parent is Group)
            {
                parent = parent.Parent;
            }



            if (parent is InputDeviceSubsystem)
            {
                name = name ?? "New input component";
                component = new InputDeviceComponent(_parent, name);
            }
            else if (parent is LogicSolverSubsystem)
            {
                name = name ?? "New solver component";
                component = new LogicSolverComponent(_parent, name);
            }
            else if (parent is FinalElementSubsystem)
            {
                name = name ?? "New final component";
                component = new FinalElementComponent(_parent, name);
            }
            else throw new Exception($"Unexpeced type for having a group: {_parent.GetType()}");

            _items.Add(component); 
            return component;
        }

        public bool Remove(SISComponent item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<SISComponent> GetEnumerator()
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

            var alreadyMatchedComponents = new List<SISComponent>();

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
                var duplicates = _items.Where(x => x.Name.StringValue == component.Name.StringValue);
                if (duplicates.Count() > 1) errors.Add(new ModelError(component, "Name must be unique."));
                component.Validate(errors);
            }
        }

    }
}
