namespace Sintef.Apos.Sif.Model
{
    public class FinalElementRequirements : SISDeviceRequirements
    {
        //public Boolean ManualOperationIsPossible { get; protected set; }
        //public LeakageRateKilogramsPerSecond MaximumAllowableLeakageRate { get; protected set; }
        //public ResetAfterShutdown_FinalElement ResetAfterShutdownRequirement { get; protected set; }
        //public Boolean TightShutoffIsRequired { get; protected set; }

        public Attribute<bool?> ManualOperationIsPossible { get; protected set; }
        public Attribute<double?> MaximumAllowableLeakageRate { get; protected set; }
        public Attribute<string> ResetAfterShutdownRequirement { get; protected set; }
        public Attribute<bool?> TightShutoffIsRequired { get; protected set; }

        public FinalElementRequirements(Group parent, string name) : base(parent, name, 4)
        {
        }
    }
}
