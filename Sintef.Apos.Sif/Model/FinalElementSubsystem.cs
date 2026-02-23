using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class FinalElementSubsystem : Subsystem
    {
        public FinalElementSubsystem(SafetyInstrumentedFunction parent) : base(parent, "FinalElement")
        {
        }
    }
}
