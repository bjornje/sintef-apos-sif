using System;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class FinalElementComponent : SISComponent
    {
        public Boolean ManualOperation { get; protected set; } //1
        public kg_s MaximumAllowableLeakageRate { get; protected set; }
        public Percent PartialStrokeTestCoverage { get; protected set; }
        public Hours PartialStrokeTestInterval { get; protected set; }
        public ResetAfterShutdown_FinalElement ResetAfterShutdown { get; protected set; } //5
        public Boolean TightShutOff { get; protected set; }

        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/FinalElementComponent";

        public FinalElementComponent(Group parent, string name) : base(parent, name, 6)
        {
        }
    }
}
