namespace Sintef.Apos.Sif.Model
{
    public class FinalElementRequirements : SISDeviceRequirements
    {
        public AttributeList<string> AssociatedDevice { get; protected set; }
        public Attribute<bool?> ManualOperationIsPossible { get; protected set; }
        public Attribute<double?> MaximumAllowableLeakageRate { get; protected set; }
        public Attribute<string> ResetAfterShutdownRequirement { get; protected set; }
        public Attribute<bool?> TightShutoffIsRequired { get; protected set; }
        public override string Subsystem => v_subsystem.final_element_subsystem;

        public FinalElementRequirements(Group parent, string name) : base(parent, name, 5)
        {
        }
    }
}
