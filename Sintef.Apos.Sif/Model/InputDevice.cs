using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class InputDevice : SIFComponent
    {
        public InputDevice(SIFSubsystem parent) : base(parent, "InputDevice")
        {
            const int expectedNumberOfAttributes = 16;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
        }
    }
}
