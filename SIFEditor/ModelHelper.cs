using Sintef.Apos.Sif.Model;

namespace SIFEditor
{
    static class ModelHelper
    {
        public static string DisplayName(this Node node)
        {
            if (node is Root) return "SIFs";

            if (node is SIF sif) return sif.SIFID.StringValue;

            if (node is SIFSubsystem sifSubsystem)
            {
                return $"{node.GetType().Name.Substring(0, node.GetType().Name.Length - 9)} ({sifSubsystem.VoteBetweenGroups_M_in_MooN.StringValue}oo{sifSubsystem.NumberOfGroups_N.StringValue})";
            }

            if (node is Group group)
            {
                return $"{node.GetType().Name} ({group.VoteWithinGroup_K_in_KooN.StringValue}oo{group.NumberOfComponentsOrSubgroups_N.StringValue})";
            }

            if (node is SISComponent sisComponent) return sisComponent.Name.StringValue;

            return node.GetType().Name;
        }
    }
}

