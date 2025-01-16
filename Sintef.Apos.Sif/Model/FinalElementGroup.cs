using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class FinalElementGroup : Group
    {
        public FinalElementGroup(FinalElement parent) : base(parent, "FinalElementGroup")
        {
        }
        public FinalElementGroup(FinalElementGroup parent) : base(parent, "FinalElementGroup")
        {
        }
    }
}
