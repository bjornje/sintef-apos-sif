using System;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class GroupVoter : Node
    {
        public Integer M { get; }
        public GroupVoter(SIFComponent parent) : base(parent, "GroupVoter")
        {
            AddAttribute(new Integer(nameof(M), "Voting MooN between groups in a SIFComponent."));

            M = Attributes.Single(x => x.Name == nameof(M)) as Integer; // 1
            M.Value = "1";

            const int expectedNumberOfAttributes = 1;
            if (Attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {Attributes.Count()}.");
        }
    }
}
