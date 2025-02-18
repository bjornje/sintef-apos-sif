using Sintef.Apos.Sif.Model;

namespace SIFEditor
{
    static class ModelHelper
    {
        public static string DisplayName(this Node node, TreeNode parent)
        {
            if (node is Root) return "SIFs";

            if (node is SIF sif) return sif.SIFID.StringValue;

            if (node is SIFSubsystem sifSubsystem)
            {
                return $"{sifSubsystem.GetPathStep()} ({sifSubsystem.VoteBetweenGroups_M_in_MooN.StringValue}oo{sifSubsystem.NumberOfGroups_N.StringValue})";
            }

            if (node is Group group)
            {
                if (parent.Tag is SIF parentSif)
                {
                    return $"CrossVoting ({group.GetPath(parentSif)})";
                }

                return $"{node.GetPathStep()} ({group.VoteWithinGroup_k_in_kooN.StringValue}oo{group.NumberOfComponentsOrSubgroups_N.StringValue})";
            }

            if (node is SISComponent sisComponent) return sisComponent.Name.StringValue;

            return node.GetType().Name;
        }
    }
}

