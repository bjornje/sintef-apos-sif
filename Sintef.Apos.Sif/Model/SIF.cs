using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class SIF : Node
    {
        public SIFSubsystems Subsystems { get; }

        public Groups CrossSubsystemGroups { get; }

        public InputDeviceSubsystem InputDevice => Subsystems.SingleOrDefault(x => x is InputDeviceSubsystem) as InputDeviceSubsystem;
        public LogicSolverSubsystem LogicSolver => Subsystems.SingleOrDefault(x => x is LogicSolverSubsystem) as LogicSolverSubsystem;
        public FinalElementSubsystem FinalElement => Subsystems.SingleOrDefault(x => x is FinalElementSubsystem) as FinalElementSubsystem;


        //Attributes
        public ILLevel AILLevel { get; private set; } //1
        public String Cause { get; private set; }
        public PerYear DemandRate { get; private set; }
        public String DemandSource { get; private set; }
        public String Effect { get; private set; } //5
        public ILLevel EILLevel { get; private set; }
        public String EnvironmentalExtremes { get; private set; }
        public ManualActivation ManualActivation { get; private set; }
        public Seconds MaxAllowableResponseTime { get; private set; }
        public String MeasureToAvoidCCF { get; private set; } //10
        public ModeOfOperation ModeOfOperation { get; private set; }
        public Frequecy PFDRequirement { get; private set; }
        public PerHour PFHRequirement { get; private set; }
        public String PlantOperatingMode { get; private set; }
        public String QuantificationMethod { get; private set; } //15
        public String SafeProcessState { get; private set; }
        public String SIFDescription { get; private set; }
        public String SIFID { get; private set; }
        public String SIFName { get; private set; }
        public SIFType SIFType { get; private set; } //20
        public String SIFTypicalID { get; private set; }
        public String SILAllocationMethod { get; private set; }
        public SILLevel SILLevel { get; private set; }
        public PerHour SpuriousTripRate { get; private set; }
        public String SurvaivabilityRequirement { get; private set; } //25

        public const string RefBaseSystemUnitPath = "SIF Unit Classes/SIF";

        public SIF(Root parent) : base(parent, $"SIF{parent.SIFs.Count() + 1}")
        {
            SetAttributes(Definition.GetAttributes(this, 25));

            Subsystems = new SIFSubsystems(this);

            CrossSubsystemGroups = new Groups(this);
        }

 
        public bool Remove(SIFSubsystem item)
        {
            return Subsystems.Remove(item);
        }

        public bool IsSameAs(SIF sif)
        {
            if (!HaveSameAttributeValues(sif)) return false;

            if (!Subsystems.IsSameAs(sif.Subsystems)) return false;
            
            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach(var property in Attributes) property.Validate(this, errors);

            if (!Subsystems.Any()) errors.Add(new ModelError(this, "Missing SIFSubsystem."));
            Subsystems.Validate(errors);
        }
    }

    public class SIFs : IEnumerable<SIF>
    {
        private readonly Collection<SIF> _items = new Collection<SIF>();
        private readonly Root _parent;

        public Root Parent => _parent;
        public SIFs(Root parent)
        {
            _parent = parent;
        }
        public SIF Append(string sifId = null)
        {
            var sif = new SIF(_parent);
            sif.SIFID.StringValue = sifId;
            _items.Add(sif);
            return sif;
        }

        public bool Remove(SIF item)
        {
            return _items.Remove(item);
        }
        public IEnumerator<SIF> GetEnumerator()
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

            var alreadyMatchedSifs = new List<SIF>();

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
