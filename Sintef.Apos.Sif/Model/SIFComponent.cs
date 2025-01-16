using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class SIFComponent : Node
    {
        public Percent DC { get; }
        public String LoopTypical { get; }
        public Hours UsefulLifetime { get; }
        public Hours MTTR { get; }
        public Percent PFDBudget { get; }
        public Percent PTC { get; }
        public Hours ProofTestInterval { get; }
        public Seconds ResponseTime { get; }
        public Percent SFF { get; }
        public SILLevel SIL { get; }
        public Percent β { get; }
         public FITs λDD { get; }
        public FITs λDU { get; }
        public FITs λS { get; }
        public Hours MRT { get; }
        public Seconds MaxAllowableResponseTime { get; }

        public GroupVoter GroupVoter { get; }
        public Groups Groups { get; }
        public const string RefBaseSystemUnitPath = "SIF Unit Classes/SIFComponent";

        public SIFComponent(SIFSubsystem parent, string name) : base(parent, name)
        {
            GroupVoter = new GroupVoter(this);
            Groups = new Groups(this);

            var attributes = Definition.GetAttributes(this);

            foreach (var attribute in attributes)
            {
                AddAttribute(attribute);
            }

            DC = Attributes.Single(x => x.Name == nameof(DC)) as Percent; // 1
            LoopTypical = Attributes.Single(x => x.Name == nameof(LoopTypical)) as String;
            UsefulLifetime = Attributes.Single(x => x.Name == nameof(UsefulLifetime)) as Hours;
            MTTR = Attributes.Single(x => x.Name == nameof(MTTR)) as Hours;
            PFDBudget = Attributes.Single(x => x.Name == nameof(PFDBudget)) as Percent; // 5
            PTC = Attributes.Single(x => x.Name == nameof(PTC)) as Percent;
            ProofTestInterval = Attributes.Single(x => x.Name == nameof(ProofTestInterval)) as Hours;
            ResponseTime = Attributes.Single(x => x.Name == nameof(ResponseTime)) as Seconds;
            SFF = Attributes.Single(x => x.Name == nameof(SFF)) as Percent;
            SIL = Attributes.Single(x => x.Name == nameof(SIL)) as SILLevel; //10
            β = Attributes.Single(x => x.Name == nameof(β)) as Percent;
            λDD = Attributes.Single(x => x.Name == nameof(λDD)) as FITs;
            λDU = Attributes.Single(x => x.Name == nameof(λDU)) as FITs;
            λS = Attributes.Single(x => x.Name == nameof(λS)) as FITs;
            MRT = Attributes.Single(x => x.Name == nameof(λS)) as Hours; // 15
            MaxAllowableResponseTime = Attributes.Single(x => x.Name == nameof(MaxAllowableResponseTime)) as Seconds; // 16

        }

        public bool Remove(Group item)
        {
            return Groups.Remove(item);
        }

        public bool IsSameAs(SIFComponent component)
        {
            if (!HaveSameAttributeValues(component)) return false;
            if (!GroupVoter.HaveSameAttributeValues(component.GroupVoter)) return false;
            if (!Groups.IsSameAs(component.Groups)) return false;

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var property in Attributes) property.Validate(this, errors);

            if (!Groups.Any()) errors.Add(new ModelError(this, "Missing Group."));
            if (long.TryParse(GroupVoter.M.Value, out var m))
            {
                if (m > Groups.Count()) errors.Add(new ModelError(this, "Number of groups is less than voting parameter M."));
                if (m < 1) errors.Add(new ModelError(this, "Voting parameter M must be >= 1."));
            }
            else
            {
                errors.Add(new ModelError(this, $"The value {GroupVoter.M.Value} provided for M for voting within {GetType().Name} is not an integer number."));
            }

            Groups.Validate(errors);
        }
    }
}
