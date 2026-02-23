using Sintef.Apos.Sif.Model.Attributes;

namespace Sintef.Apos.Sif.Model
{
    public class LogicSolverRequirements : SISDeviceRequirements
    {
        public Boolean ResetAfterShutdownRequirement { get; protected set; }
        public LogicSolverRequirements(Group parent, string name) : base(parent, name, 1)
        {
        }
    }
}
