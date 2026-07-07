using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class SafetyInstrumentedFunction : Node
    {
        public Groups Groups { get; }

        public Documents Documents { get; }

        //Attributes

        public Attribute<double?> MaximumAllowableDemandRate { get; protected set; }
        public Attribute<double?> MaximumAllowableSIFResponseTime { get; protected set; }
        public Attribute<string> SafeStateOfProcess { get; protected set; }
        public Attribute<string> SafetyIntegrityLevelRequirement { get; protected set; }
        public Attribute<string> SILAllocationMethod { get; protected set; }
        public Attribute<string> SIFDescription { get; protected set; }
        public Attribute<string> ModeOfOperation { get; protected set; }
        public Attribute<string> EnvironmentalIntegrityLevelRequirement { get; protected set; }
        public Attribute<string> AssetIntegrityLevelRequirement { get; protected set; }
        public Attribute<double?> PFDRequirement { get; protected set; }
        public Attribute<string> SIFName { get; protected set; }
        public Attribute<string> SIFType { get; protected set; }
        public Attribute<string> SIFID { get; protected set; }
        public Attribute<string> Cause { get; protected set; }
        public Attribute<string> Effect { get; protected set; }
        public Attribute<double?> MaximumAllowableSpuriousTripRate { get; protected set; }
        public Attribute<string> ManuallyActivatedShutdownRequirement { get; protected set; }
        public Attribute<double?> PFHRequirement { get; protected set; }
        public Attribute<string> DemandSource { get; protected set; }
        public Attribute<string> SIFTypicalID { get; protected set; }
        public Attribute<string> SIFVersion { get; protected set; }
        public AttributeList<string> IndependentProtectionLayer { get; protected set; }
        public AttributeList<string> MeasureToAvoidCommonCauseFailures { get; protected set; }
        public AttributeList<string> PlantOperatingMode { get; protected set; }
        public AttributeList<string> QuantificationMethodOrTool { get; protected set; }
        public AttributeList<string> ReferenceToBarrier { get; protected set; }
        public AttributeList<string> ReferenceToEquipmentUnderControl { get; protected set; }
        public AttributeList<string> SurvivabilityRequirement { get; protected set; }


        public SafetyInstrumentedFunction(Root parent) : base(parent, $"SIF{parent.SIFs.Count() + 1}")
        {
            SetAttributes(Definition.GetAttributes(this, 28));

            Groups = new Groups(this);

            Documents = new Documents(this);
        }

 
        public bool Remove(Group item)
        {
            return Groups.Remove(item);
        }

        public bool IsSameAs(SafetyInstrumentedFunction sif)
        {
            if (!HaveSameAttributeValues(sif))
            {
                return false;
            }

            if (!Groups.IsSameAs(sif.Groups))
            {
                return false;
            }
            
            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach(var property in Attributes)
            {
                property.Validate(this, errors);
            }

            if (!Groups.Any())
            {
                errors.Add(new ModelError(this, "Missing groups."));
            }

            Groups.Validate(errors);

            Documents.Validate(errors);
        }

        public IEnumerable<Group> GetAllGroups()
        {
            return Groups;
        }

        public override void PushAttributes()
        {
            foreach(var attribute in Definition.GetAdditionalAttributes(GetType().Name))
            {
                TryAddAttribute(attribute.Clone(this));
            }

            Groups.PushAttributes();
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

        public void PushAttributes()
        {
            foreach (var sif in _items)
            {
                sif.PushAttributes();
            }
        }
    }
}
