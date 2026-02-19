namespace Sintef.Apos.Sif.Model
{
    public class FinalElementComponent : SISComponent
    {
        public Boolean ManualOperationIsPossible { get; protected set; }
        public LeakageRate_kg_s MaximumAllowableLeakageRate { get; protected set; }
        public ResetAfterShutdown_FinalElement ResetAfterShutdownRequirement { get; protected set; }
        public Boolean TightShutoffIsRequired { get; protected set; }

        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/FinalElementComponent";

        public FinalElementComponent(Group parent, string name) : base(parent, name, 6)
        {
        }
    }
}
