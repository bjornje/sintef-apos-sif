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
        public Hours ProofTestInterval { get; }
        public SILLevel SIL { get; }

        public const string RefBaseSystemUnitPath = "SIF Unit Classes/SISComponent";
        public GroupVoter GroupVoter { get; set; }

        public String Name { get; } = new String(nameof(Name), "");

        public SISComponent(Group parent, string name) : base(parent, name)
        {
            Name.Value = name;

            var attributes = Definition.GetAttributes(this);

            foreach (var attribute in attributes)
            {
                AddAttribute(attribute);
            }

            ProofTestInterval = Attributes.Single(x => x.Name == nameof(ProofTestInterval)) as Hours; // 1
            SIL = Attributes.Single(x => x.Name == nameof(SIL)) as SILLevel; // 2
        }

        public bool IsSameAs(SISComponent component)
        {
            if (string.IsNullOrEmpty(Name.Value) && !string.IsNullOrEmpty(component.Name.Value)) return false;
            if (!string.IsNullOrEmpty(Name.Value) && string.IsNullOrEmpty(component.Name.Value)) return false;
            if (!string.IsNullOrEmpty(Name.Value) && !string.IsNullOrEmpty(component.Name.Value) && Name.Value != component.Name.Value) return false;

            if (!HaveSameAttributeValues(component)) return false;

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var property in Attributes) property.Validate(this, errors);

            if (string.IsNullOrWhiteSpace(Name.Value)) errors.Add(new ModelError(this, "Name must have a value."));
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
        public SISComponent Append(string name)
        {
            SISComponent component;

            if (_parent is InputDeviceGroup initiatorGroup) component = new InitiatorComponent(initiatorGroup, name);
            else if (_parent is LogicSolverGroup solverGroup) component = new LogicSolverComponent(solverGroup, name);
            else if (_parent is FinalElementGroup finalGroup) component = new FinalElementComponent(finalGroup, name);
            else throw new Exception($"Unexpeced type of Group: {_parent.GetType()}");

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
                var duplicates = _items.Where(x => x.Name.Value == component.Name.Value);
                if (duplicates.Count() > 1) errors.Add(new ModelError(component, "Name must be unique."));
                component.Validate(errors);
            }
        }

    }
}
