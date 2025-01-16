using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class InitiatorComponent : SISComponent
    {
        public InputDeviceTrigger Criteria { get; }
        public String LoopTypical { get; }

        public new const string RefBaseSystemUnitPath = "SIS Unit Classes/InitiatorComponent";

        public InitiatorComponent(InputDeviceGroup parent, string name) : base(parent, name)
        {
            Criteria = Attributes.Single(x => x.Name == nameof(Criteria)) as InputDeviceTrigger; // 3
            LoopTypical = Attributes.Single(x => x.Name == nameof(LoopTypical)) as String; // 4

            const int expectedNumberOfAttributes = 4;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
        }
    }
}
