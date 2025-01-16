using System;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class FinalElementComponent : SISComponent
    {
        public FinalElementFunction Function { get; }
        public String LoopTypical { get; }
        public FailSafePosition FailSafePosition { get; }

        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/FinalElementComponent";

        public FinalElementComponent(FinalElementGroup parent, string name) : base(parent, name)
        {
            Function = Attributes.Single(x => x.Name == nameof(Function)) as FinalElementFunction; // 3
            LoopTypical = Attributes.Single(x => x.Name == nameof(LoopTypical)) as String;
            FailSafePosition = Attributes.Single(x => x.Name == nameof(FailSafePosition)) as FailSafePosition; // 5

            const int expectedNumberOfAttributes = 5;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
        }
    }
}
