namespace Sintef.Apos.Sif.Model
{
    public class LogicSolverComponent : SISComponent
    {
        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/LogicSolverComponent";
        public Boolean ResetAfterShutdown { get; protected set; }
        public LogicSolverComponent(Group parent, string name) : base(parent, name, 1)
        {
        }
    }
}
