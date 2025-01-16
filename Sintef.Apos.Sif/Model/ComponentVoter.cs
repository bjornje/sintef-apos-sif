using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class ComponentVoter : Node
    {
        public Integer K { get; }

        public ComponentVoter(Group parent) : base(parent, "ComponentVoter")
        {
            AddAttribute(new Integer(nameof(K), "Voting KooN between components or groups in a Group."));

            K = Attributes.Single(x => x.Name == nameof(K)) as Integer; // 1
            K.Value = "1";

            const int expectedNumberOfAttributes = 1;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
        }
    }
}
