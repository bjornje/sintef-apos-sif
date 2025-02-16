using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class LogicSolverComponent : SISComponent
    {
        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/LogicSolverComponent";

        public LogicSolverComponent(Group parent, string name) : base(parent, name, 0)
        {
        }
    }
}
