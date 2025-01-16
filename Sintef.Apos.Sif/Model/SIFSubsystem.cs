using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class SIFSubsystem : Node
    {
        public InputDevice InputDevice { get; private set; }
        public LogicSolver LogicSolver { get; private set; }
        public FinalElement FinalElement { get; private set; }

        public Percent PFDBudget { get; }
        public Hours ProcessSafetyTime { get; }
        public Seconds MaxAllowableResponseTime { get; }
        public Hours ProofTestInterval { get; }
        public SILLevel SIL { get; }
        public InputDevice CreateInputDevice()
        {
            InputDevice = new InputDevice(this);
            return InputDevice;
        }

        public LogicSolver CreateLogicSolver()
        {
            LogicSolver = new LogicSolver(this);
            return LogicSolver;
        }

        public FinalElement CreateFinalElement()
        {
            FinalElement = new FinalElement(this);
            return FinalElement;
        }

        public bool Remove(SIFComponent item)
        {
            if (item is InputDevice)
            {
                var exists = InputDevice != null;
                InputDevice = null;
                return exists;
            }
            else if (item is LogicSolver)
            {
                var exists = LogicSolver != null;
                LogicSolver = null;
                return exists;
            }
            else if (item is FinalElement)
            {
                var exists = FinalElement != null;
                FinalElement = null;
                return exists;
            }

            return false;
        }

        public SIFSubsystem(SIF parent) : base(parent, $"SIFSubsystem{parent.Subsystems.Count() + 1}")
        {
            var attributes = Definition.GetAttributes(this);

            foreach (var attribute in attributes)
            {
                AddAttribute(attribute);
            }

            PFDBudget = Attributes.Single(x => x.Name == nameof(PFDBudget)) as Percent; // 1
            ProcessSafetyTime = Attributes.Single(x => x.Name == nameof(ProcessSafetyTime)) as Hours;
            MaxAllowableResponseTime = Attributes.Single(x => x.Name == nameof(MaxAllowableResponseTime)) as Seconds;
            ProofTestInterval = Attributes.Single(x => x.Name == nameof(ProofTestInterval)) as Hours;
            SIL = Attributes.Single(x => x.Name == nameof(SIL)) as SILLevel; // 5

            const int expectedNumberOfAttributes = 5;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
        }

        public IEnumerable<SIFComponent> Components()
        {
            var list = new List<SIFComponent>();

            if (InputDevice != null) list.Add(InputDevice);
            if (LogicSolver != null) list.Add(LogicSolver);
            if (FinalElement != null) list.Add(FinalElement);

            return list;
        }

        public bool IsSameAs(SIFSubsystem subsystem)
        {
            if (!HaveSameAttributeValues(subsystem)) return false;

            if (!HaveSameComponent(InputDevice, subsystem.InputDevice)) return false;
            if (!HaveSameComponent(LogicSolver, subsystem.LogicSolver)) return false;
            if (!HaveSameComponent(FinalElement, subsystem.FinalElement)) return false;

            return true;
        }

        private static bool HaveSameComponent(SIFComponent myComponent, SIFComponent component)
        {
            if (myComponent == null && component != null) return false;
            if (myComponent != null && component == null) return false;
            if (myComponent == null && component == null) return true;

            if (!myComponent.IsSameAs(component)) return false;
            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var property in Attributes) property.Validate(this, errors);

            var n = 0;

            if (InputDevice != null) n++;
            if (LogicSolver != null) n++;
            if (FinalElement != null) n++;

            if (n == 0) errors.Add(new ModelError(this, "Missing SIFComponent. (InputDevice, LogicSolver or FinalElement.)"));

            InputDevice?.Validate(errors);
            LogicSolver?.Validate(errors);
            FinalElement?.Validate(errors);
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
        public SIFSubsystem Append()
        {
            var subsystem = new SIFSubsystem(_parent);
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
