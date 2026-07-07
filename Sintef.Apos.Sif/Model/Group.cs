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
        public Attribute<int?> MInVotingMooN { get; protected set; }
        public Attribute<int?> NInVotingMooN { get; protected set; }
        public Attribute<string> Subsystem { get; protected set; }
        public Attribute<double?> PFDBudget { get; protected set; }

        private readonly Voter _voter;

        public Group(Node parent, string name) : base(parent, name)
        {
            SetAttributes(Definition.GetAttributes(this, 4));
            Components = new SISComponents(this);
            Groups = new Groups(this);

            _voter = new Voter(this, MInVotingMooN, "M", NInVotingMooN, "N");
        }

        public void VoteWithinGroup(int M, int N)
        {
            MInVotingMooN.Value = M;
            NInVotingMooN.Value = N;
        }

        public bool Remove(SISDeviceRequirements item)
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
            foreach (var property in Attributes)
            {
                property.Validate(this, errors);
            }

            if (!Components.Any() && !Groups.Any())
            {
                errors.Add(new ModelError(this, "Group is empty."));
            }
            else if (!Components.Any() && Groups.Count() == 1)
            {
                errors.Add(new ModelError(this, "Group has no components and only one group."));
            }
            else
            {
                _voter.Validate(errors);
            }

            Groups.Validate(errors);
            Components.Validate(errors);
        }

        public override void PushAttributes()
        {
            foreach (var attribute in Definition.GetAdditionalAttributes(GetType().Name))
            {
                TryAddAttribute(attribute.Clone(this));
            }

            Groups.PushAttributes();
            Components.PushAttributes();
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
            var group = new Group(_parent, "Group");

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
            if (_items.Count != groups.Count())
            {
                return false;
            }

            var alreadyMatchedGroups = new List<Group>();

            foreach (var group in groups)
            {
                var myGroup = _items.FirstOrDefault(x => !alreadyMatchedGroups.Contains(x) && x.IsSameAs(group));

                if (myGroup == null)
                {
                    return false;
                }

                alreadyMatchedGroups.Add(myGroup);
            }

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var group in _items)
            {
                group.Validate(errors);
            }
        }

        public void PushAttributes()
        {
            foreach (var group in _items)
            {
                group.PushAttributes();
            }
        }
    }
}
