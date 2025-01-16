using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class InputDeviceGroup : Group
    {

        public InputDeviceGroup(InputDevice parent) : base(parent, "InputDeviceGroup")
        {
        }

        public InputDeviceGroup(InputDeviceGroup parent) : base(parent, "InputDeviceGroup")
        {
        }
    }
}
