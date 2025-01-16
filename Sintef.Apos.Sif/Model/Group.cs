using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class Group : Node
    {
        public Groups Groups { get; }
        public ComponentVoter ComponentVoter { get; }
        public SISComponents Components { get; }

        public const string RefBaseSystemUnitPath = "SIF Unit Classes/Group";
        public Group(SIFComponent parent, string name) : base(parent, $"{name}{parent.Groups.Count() + 1}")
        {
            ComponentVoter = new ComponentVoter(this);
            Components = new SISComponents(this);
            Groups = new Groups(this);
        }

        public Group(Group parent, string name) : base(parent, $"{name}{parent.Groups.Count() + 1}")
        {
            ComponentVoter = new ComponentVoter(this);
            Components = new SISComponents(this);
            Groups = new Groups(this);
        }

        public bool Remove(SISComponent item)
        {
            return Components.Remove(item);
        }

        public bool Remove(Group item)
        {
            return Groups.Remove(item);
        }

        public bool IsSameAs(Group group)
        {
            if (!HaveSameAttributeValues(group)) return false;
            if (!ComponentVoter.HaveSameAttributeValues(group.ComponentVoter)) return false;

            if (!Components.IsSameAs(group.Components)) return false;
            if (!Groups.IsSameAs(group.Groups)) return false;

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            if (!Components.Any() && !Groups.Any()) errors.Add(new ModelError(this, "Group is empty."));
            if (long.TryParse(ComponentVoter.K.Value, out var k))
            {
                if (k > (Components.Count() + Groups.Count())) errors.Add(new ModelError(this, "Number of components and/or groups is less than voting parameter K."));
                if (k < 1) errors.Add(new ModelError(this, "Voting parameter K must be >= 1."));
            }
            else
            {
                errors.Add(new ModelError(this, $"The value {ComponentVoter.K.Value} provided for K for voting within {GetType().Name} is not an integer number."));
            }

            Groups.Validate(errors);
            Components.Validate(errors);
        }
    }

    public class Groups : IEnumerable<Group>
    {
        private readonly Collection<Group> _items = new Collection<Group>();
        private readonly Node _parent;
        public Groups(SIFComponent parent)
        {
            _parent = parent;
        }

        public Groups(Group parent)
        {
            _parent = parent;
        }

        public Group Append()
        {
            Group group;

            if (_parent is InputDevice inputDevice) group = new InputDeviceGroup(inputDevice);
            else if (_parent is LogicSolver logicSolver) group = new LogicSolverGroup(logicSolver);
            else if (_parent is FinalElement finalElement) group = new FinalElementGroup(finalElement);
            else if (_parent is InputDeviceGroup inputDeviceGroup) group = new InputDeviceGroup(inputDeviceGroup);
            else if (_parent is LogicSolverGroup logicSolverGroup) group = new LogicSolverGroup(logicSolverGroup);
            else if (_parent is FinalElementGroup finalElementGroup) group = new FinalElementGroup(finalElementGroup);
            else throw new Exception($"Unexpected Node: {_parent.GetType()}");

            _items.Add(group);
            return group;
        }

        public bool Remove(Group item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<Group> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool IsSameAs(Groups groups)
        {
            if (_items.Count != groups.Count()) return false;

            var alreadyMatchedGroups = new List<Group>();

            foreach (var group in groups)
            {
                var myGroup = _items.FirstOrDefault(x => !alreadyMatchedGroups.Contains(x) && x.IsSameAs(group));
                if (myGroup == null) return false;
                alreadyMatchedGroups.Add(myGroup);
            }

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var group in _items) group.Validate(errors);
        }
    }
}
