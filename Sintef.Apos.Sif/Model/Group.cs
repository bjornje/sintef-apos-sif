using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class Group : Node
    {
        public Groups Groups { get; }
        public SISComponents Components { get; }
        public Integer MInVotingMooN { get; protected set; }
        public Integer NumberOfDevicesWithinGroup { get; protected set; }
        public Boolean AllowAnyComponents { get; protected set; }

        public const string RefBaseSystemUnitPath = "SIF Unit Classes/Group";

        private readonly Voter _voter;

        public Group(Node parent, string name) : base(parent, name)
        {
            SetAttributes(Definition.GetAttributes(this, 3));
            Components = new SISComponents(this);
            Groups = new Groups(this);

            _voter = new Voter(this, MInVotingMooN, "M", NumberOfDevicesWithinGroup, "N");
        }

        public void VoteWithinGroup(int M, int N)
        {
            MInVotingMooN.Value = M;
            NumberOfDevicesWithinGroup.Value = N;
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

            if (!Components.IsSameAs(group.Components)) return false;
            if (!Groups.IsSameAs(group.Groups)) return false;

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var property in Attributes) property.Validate(this, errors);

            if (!Components.Any() && !Groups.Any())
            {
                errors.Add(new ModelError(this, "Group is empty."));
            }
            else
            {
                _voter.Validate(errors);
            }

            Groups.Validate(errors);
            Components.Validate(errors);
        }

        public IEnumerable<Group> GetAllGroups()
        {
            var groups = new List<Group>();

            groups.AddRange(Groups);

            foreach (var group in Groups) groups.AddRange(group.GetAllGroups());

            return groups;
        }

        public override int GetNumberOfElements()
        {
            return Groups.Count() + Components.Count();
        }

    }

    public class Groups : IEnumerable<Group>
    {
        private readonly Collection<Group> _items = new Collection<Group>();
        private readonly Node _parent;
        public Groups(Node parent)
        {
            _parent = parent;
        }

        public Group Append()
        {
            Group group = new Group(_parent, "Group");

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
    public class CrossSubsystemGroups : Node, IEnumerable<Group>
    {
        public Integer VoteBetweenGroups_M_in_MooN { get; protected set; }
        public Integer NumberOfGroups_N { get; protected set; }

        private readonly Voter _voter;

        private readonly Collection<Group> _items = new Collection<Group>();
        public CrossSubsystemGroups(Node parent) : base(parent, "CrossSubsystemGroups")
        {
            NumberOfGroups_N = new Integer("NumberOfGroups_N", "", "", this);
            VoteBetweenGroups_M_in_MooN = new Integer("VoteBetweenGroups_M_in_MooN", "", "", this);

            _voter = new Voter(this, VoteBetweenGroups_M_in_MooN, "M", NumberOfGroups_N, "N");

        }

        public void VoteBetweenGroups(int M, int N)
        {
            VoteBetweenGroups_M_in_MooN.Value = M;
            NumberOfGroups_N.Value = N;
        }

        public override int GetNumberOfElements()
        {
            return _items.Count;
        }

        public void Validate(Collection<ModelError> errors)
        {
            if (!_items.Any()) return;
            _voter.Validate(errors);
        }

        public bool Add(Group item)
        {
            if (_items.Contains(item)) return false;

            _items.Add(item);
            return true;
        }



        public bool Remove(Group item)
        {
            return _items.Remove(item);
        }

        public void Clear()
        {
            _items.Clear();
        }

        public IEnumerator<Group> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

    }
}
