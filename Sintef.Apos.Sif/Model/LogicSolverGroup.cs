using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class LogicSolverGroup : Group
    {
        public LogicSolverGroup(LogicSolver parent) : base(parent, "LogicSolverGroup")
        {
        }
        public LogicSolverGroup(LogicSolverGroup parent) : base(parent, "LogicSolverGroup")
        {
        }
    }
}
