using Sintef.Apos.Sif.Model;

namespace SIFEditor
{
    static class ModelHelper
    {
        public static string DisplayName(this Node node)
        {
            if (node is Root) return "SIFs";
            if (node is SIF sif) return sif.SIFID.Value;
            if (node is SIFComponent sifComponent) return $"{node.GetType().Name} ({sifComponent.GroupVoter.M.Value}oo{sifComponent.Groups.Count()})";
            if (node is Group group) return $"{node.GetType().Name} ({group.ComponentVoter.K.Value}oo{group.Components.Count() + group.Groups.Count()})";
            if (node is SISComponent sisComponent) return sisComponent.Name.Value;
            return node.GetType().Name;
        }
    }
}

