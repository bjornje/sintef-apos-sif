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
        public String SIFID { get; }
        public Frequecy DemandRate { get; }
        public E_DEToTrip E_DEToTrip { get; }
        public String SafeState { get; }
        public String SILAllocationMethod { get; }

        public const string RefBaseSystemUnitPath = "SIF Unit Classes/SIF";

        public SIF(Root parent) : base(parent, $"SIF{parent.SIFs.Count() + 1}")
        {
            Subsystems = new SIFSubsystems(this);

            var attributes = Definition.GetAttributes(this);

            foreach (var attribute in attributes)
            {
                AddAttribute(attribute);
            }


            SIFID = Attributes.Single(x => x.Name == nameof(SIFID)) as String; // 1
            DemandRate = Attributes.Single(x => x.Name == nameof(DemandRate)) as Frequecy;
            E_DEToTrip = Attributes.Single(x => x.Name == nameof(E_DEToTrip)) as E_DEToTrip;
            SafeState = Attributes.Single(x => x.Name == nameof(SafeState)) as String;
            SILAllocationMethod = Attributes.Single(x => x.Name == nameof(SILAllocationMethod)) as String; // 5

            const int expectedNumberOfAttributes = 5;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
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
        public SIF Append(string sifId)
        {
            var sif = new SIF(_parent);
            sif.SIFID.Value = sifId;
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
                var duplicates = _items.Where(x => x.SIFID.Value == sif.SIFID.Value);
                if (duplicates.Count() > 1) errors.Add(new ModelError(sif, "SIFID must be unique."));
                sif.Validate(errors);
            }
        }
    }
}
