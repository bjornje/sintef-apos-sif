using Sintef.Apos.Sif.Model.Attributes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using String = Sintef.Apos.Sif.Model.Attributes.String;

namespace Sintef.Apos.Sif.Model
{
    public class SafetyInstrumentedFunction : Node
    {
        public Subsystems Subsystems { get; }

        public CrossSubsystemGroups CrossSubsystemGroups { get; }

        public InputDeviceSubsystem InputDevice => Subsystems.SingleOrDefault(x => x is InputDeviceSubsystem) as InputDeviceSubsystem;
        public LogicSolverSubsystem LogicSolver => Subsystems.SingleOrDefault(x => x is LogicSolverSubsystem) as LogicSolverSubsystem;
        public FinalElementSubsystem FinalElement => Subsystems.SingleOrDefault(x => x is FinalElementSubsystem) as FinalElementSubsystem;

        public Documents Documents { get; }

        //Attributes

        public FrequecyPerYear MaximumAllowableDemandRate { get; private set; }
        public DurationSeconds MaximumAllowableSIFResponseTime { get; private set; }
        public String SafeStateOfProcess { get; private set; }
        public SILLevel SafetyIntegrityLevelRequirement { get; private set; }
        public String SILAllocationMethod { get; private set; }
        public String SIFDescription { get; private set; }
        public ModeOfOperation ModeOfOperation { get; private set; }
        public EnvironmentalIntegrityLevel EnvironmentalIntegrityLevelRequirement { get; private set; }
        public AssetIntegrityLevel AssetIntegrityLevelRequirement { get; private set; }
        public Probability PFDRequirement { get; private set; }
        public String SIFName { get; private set; }
        public SIFType SIFType { get; private set; }
        public String SIFID { get; private set; }
        public String Cause { get; private set; }
        public String Effect { get; private set; }
        public FrequecyPerHour MaximumAllowableSpuriousTripRate { get; private set; }
        public String ManuallyActivatedShutdownRequirement { get; private set; }
        public String Effect4 { get; private set; }
        public FrequecyPerHour PFHRequirement { get; private set; }
        public String DemandSource { get; private set; }
        public String SIFTypicalID { get; private set; }
        public String SIFVersion { get; private set; }

        public SafetyInstrumentedFunction(Root parent) : base(parent, $"SIF{parent.SIFs.Count() + 1}")
        {
            SetAttributes(Definition.GetAttributes(this, 21));

            Subsystems = new Subsystems(this);

            CrossSubsystemGroups = new CrossSubsystemGroups(this);
            CrossSubsystemGroups.VoteBetweenGroups(1, 1);

            Documents = new Documents(this);
        }

 
        public bool Remove(Subsystem item)
        {
            return Subsystems.Remove(item);
        }

        public bool IsSameAs(SafetyInstrumentedFunction sif)
        {
            if (!HaveSameAttributeValues(sif)) return false;

            if (!Subsystems.IsSameAs(sif.Subsystems)) return false;
            
            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach(var property in Attributes) property.Validate(this, errors);

            CrossSubsystemGroups.Validate(errors);

            if (!Subsystems.Any()) errors.Add(new ModelError(this, "Missing SIFSubsystem."));
            Subsystems.Validate(errors);
        }

        public IEnumerable<Group> GetAllGroups()
        {
            var groups = new List<Group>();

            foreach (var subsystem in Subsystems) groups.AddRange(subsystem.GetAllGroups());

            return groups;
        }

        public void SetCrossVotingGroups()
        {
            CrossSubsystemGroups.Clear();

            foreach (var group in GetAllGroups().Where(x => x.AllowAnyComponents.Value.HasValue && x.AllowAnyComponents.Value.Value))
            {
                CrossSubsystemGroups.Add(group);
            }
        }
    }

    public class SIFs : IEnumerable<SafetyInstrumentedFunction>
    {
        private readonly Collection<SafetyInstrumentedFunction> _items = new Collection<SafetyInstrumentedFunction>();
        private readonly Root _parent;

        public Root Parent => _parent;

        public SIFs(Root parent)
        {
            _parent = parent;
        }

        public SafetyInstrumentedFunction Append(string sifId = null)
        {
            var sif = new SafetyInstrumentedFunction(_parent);
            sif.SIFID.StringValue = sifId ?? "New SIF";
            _items.Add(sif);
            return sif;
        }

        public bool Remove(SafetyInstrumentedFunction item)
        {
            return _items.Remove(item);
        }
        public IEnumerator<SafetyInstrumentedFunction> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool IsSameAs(SIFs sifs)
        {
            if (_items.Count != sifs.Count()) return false;

            var alreadyMatchedSifs = new List<SafetyInstrumentedFunction>();

            foreach (var sif in sifs)
            {
                var mySif = _items.FirstOrDefault(x => !alreadyMatchedSifs.Contains(x) && x.IsSameAs(sif));
                if (mySif == null) return false;
                alreadyMatchedSifs.Add(mySif);
            }

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var sif in _items)
            {
                var duplicates = _items.Where(x => x.SIFID.StringValue == sif.SIFID.StringValue);
                if (duplicates.Count() > 1) errors.Add(new ModelError(sif, "SIFID must be unique."));
                sif.Validate(errors);
            }
        }
    }
}
