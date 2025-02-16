using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class SIFSubsystem : Node
    {
        public const string RefBaseSystemUnitPath = "SIF Unit Classes/SIFSubsystem";
        public Groups Groups { get; }
        public Percent PFDBudget { get; protected set; }
        public Integer VoteBetweenGroups_M_in_MooN { get; protected set; }
        public Integer NumberOfGroups_N { get; protected set; }

        private readonly Voter _voter;
        public SIFSubsystem(SIF parent, bool isTemporary = false) : base(parent, $"SIFSubsystem{parent.Subsystems.Count() + 1}")
        {
            if (isTemporary) return;

            SetAttributes(Definition.GetAttributes(this, 3));

            _voter = new Voter(this, VoteBetweenGroups_M_in_MooN, "M", NumberOfGroups_N, "N");

            Groups = new Groups(this);
        }

        public void VoteBetweenGroups(int M, int N)
        {
            VoteBetweenGroups_M_in_MooN.Value = M;
            NumberOfGroups_N.Value = N;
        }

        public bool IsSameAs(SIFSubsystem subsystem)
        {
            if (!HaveSameAttributeValues(subsystem)) return false;
            if (!Groups.IsSameAs(subsystem.Groups)) return false;

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var property in Attributes) property.Validate(this, errors);

            if (!Groups.Any())
            {
                errors.Add(new ModelError(this, "Subsystem has no groups."));
            }
            else
            {
                _voter.Validate(errors);
            }

            Groups.Validate(errors);
        }
    }

    public class SIFSubsystems : IEnumerable<SIFSubsystem>
    {
        private readonly Collection<SIFSubsystem> _items = new Collection<SIFSubsystem>();
        private readonly SIF _parent;
        public SIFSubsystems(SIF parent)
        {
            _parent = parent;
        }

        public InputDeviceSubsystem AppendInputDevice()
        {
            if (_parent.InputDevice != null) throw new Exception("InputDevice already exists.");

            var subsystem = new InputDeviceSubsystem(_parent);
            _items.Add(subsystem);

            return subsystem;
        }

        public LogicSolverSubsystem AppendLogicSolver()
        {
            if (_parent.LogicSolver != null) throw new Exception("LogicSolver already exists.");

            var subsystem = new LogicSolverSubsystem(_parent);
            _items.Add(subsystem);

            return subsystem;
        }

        public FinalElementSubsystem AppendFinalElement()
        {
            if (_parent.FinalElement != null) throw new Exception("FinalElement already exists.");

            var subsystem = new FinalElementSubsystem(_parent);
            _items.Add(subsystem);

            return subsystem;
        }



        public bool Remove(SIFSubsystem item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<SIFSubsystem> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool IsSameAs(SIFSubsystems subsystems)
        {
            if (_items.Count != subsystems.Count()) return false;

            var alreadyMatchedSubsystems = new List<SIFSubsystem>();

            foreach (var subsystem in subsystems)
            {
                var mySubsystem = _items.FirstOrDefault(x => !alreadyMatchedSubsystems.Contains(x) && x.IsSameAs(subsystem));
                if (mySubsystem == null) return false;
                alreadyMatchedSubsystems.Add(mySubsystem);
            }

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var subsystem in _items)
            {
                subsystem.Validate(errors);
            }
        }


    }
}
