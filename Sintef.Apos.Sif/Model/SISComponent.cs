using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class SISComponent : Node
    {
        public String APOSL2Group { get; protected set; } //1
        public Boolean Bypass_Approvement { get; protected set; }
        public BypassControl Bypass_Control { get; protected set; }
        public Seconds Bypass_MaxAllowableBypassTime { get; protected set; }
        public String DiagnosticRequired { get; protected set; } //5
        public String DiagnosticRequirementsForImplementation { get; protected set; }
        public E_DEToTrip E_DEToTrip { get; protected set; }
        public String EnvironmentalExtremes { get; protected set; }
        public Comparison FailPass_ComparisonToPass { get; protected set; }
        public String FailPass_Masurement { get; protected set; } //10
        public String FailPass_Requirement { get; protected set; }
        public String FailPass_Unit { get; protected set; }
        public Seconds MaxAllowableResponseTime { get; protected set; }
        public Hours MTTR { get; protected set; }
        public Percent PFDBudget { get; protected set; } //15
        public Percent PFHBudget { get; protected set; }
        public Percent ProofTestCoverage { get; protected set; }
        public Hours ProofTestIntervalOperatorSpec { get; protected set; }
        public Hours ProofTestIntervalSILCompliance { get; protected set; }
        public String SafeState { get; protected set; } //20
        public String SurvaibabilityRequirement { get; protected set; }
        public SILLevel SystematicCapability { get; protected set; } 
        public String TagDescription { get; protected set; }
        public String TagName { get; protected set; }
        public TagNumber TagNumber { get; protected set; } //25
        public Seconds TimeDelay { get; protected set; }
        public String TripAction { get; protected set; }
        public TypeAB TypeAB { get; protected set; } //28

        public const string RefBaseSystemUnitPath = "SIF Unit Classes/SISComponent";

        public String Name { get; }

        public SISComponent(Group parent, string name, int expectedNumberOfAttributes) : base(parent, name)
        {
            Name = new String(nameof(Name), "", this);
            Name.StringValue = name;

            SetAttributes(Definition.GetAttributes(this, expectedNumberOfAttributes + 28));
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
