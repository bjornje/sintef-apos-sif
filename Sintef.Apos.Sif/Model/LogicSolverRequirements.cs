namespace Sintef.Apos.Sif.Model
{
    public class LogicSolverRequirements : SISDeviceRequirements
    {
        public Attribute<bool?> ResetAfterShutdownRequirement { get; protected set; }
        public Attribute<string> StandbyRedundancyType { get; protected set; }
        public override string Subsystem => v_subsystem.logic_solver_subsystem;

        public LogicSolverRequirements(Group parent, string name) : base(parent, name, 2)
        {
        }
    }
}
