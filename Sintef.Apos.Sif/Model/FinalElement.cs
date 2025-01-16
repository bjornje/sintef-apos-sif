using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class FinalElement : SIFComponent
    {
        public FailSafePosition FailSafePosition { get; }

        public FinalElement(SIFSubsystem parent) : base(parent, "FinalElement")
        {
            FailSafePosition = Attributes.Single(x => x.Name == nameof(FailSafePosition)) as FailSafePosition; // 17

            const int expectedNumberOfAttributes = 17;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
        }
    }
}
