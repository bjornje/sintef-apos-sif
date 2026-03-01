namespace Sintef.Apos.Sif.Model
{
    public class LogicSolverRequirements : SISDeviceRequirements
    {
        //public Boolean ResetAfterShutdownRequirement { get; protected set; }
        public Attribute<bool?> ResetAfterShutdownRequirement { get; protected set; }
        public LogicSolverRequirements(Group parent, string name) : base(parent, name, 1)
        {
        }
    }
}
