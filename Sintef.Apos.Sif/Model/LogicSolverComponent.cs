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

        public LogicSolverComponent(LogicSolverGroup parent, string name) : base(parent, name)
        {
            const int expectedNumberOfAttributes = 2;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
        }
    }
}
