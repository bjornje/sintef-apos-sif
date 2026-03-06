using Sintef.Apos.Sif;
using Sintef.Apos.Sif.Model;

namespace SIFEditor
{
    static class ModelHelper
    {
        public static string NodeDisplayName(this Node node, TreeNode parent)
        {
            if (node is Root) return "SIFs";

            if (node is SafetyInstrumentedFunction sif)
            {
                return sif.SIFID.StringValue;
            }

            if (node is Subsystem sifSubsystem)
            {
                return $"{sifSubsystem.GetPathStep()} ({sifSubsystem.MInVotingMooN.StringValue}oo{sifSubsystem.NumberOfGroups.StringValue})";
            }

            if (node is Group group)
            {
                if (parent.Tag is CrossSubsystemGroups)
                {
                    return $"{group.GetPath(group.GetSIF())}";
                }

                return $"{node.GetPathStep()} ({group.MInVotingMooN.StringValue}oo{group.NumberOfDevicesWithinGroup.StringValue})";
            }

            if (node is CrossSubsystemGroups crossGroups)
            {
                //if (crossGroups.GetNumberOfElements() == 0)
                //{
                //    return "CrossVoting";
                //}

                //return $"CrossVoting ({crossGroups.VoteBetweenGroups_M_in_MooN.StringValue}oo{crossGroups.NumberOfGroups_N.StringValue})";

                return "CrossVoting";
            }

            if (node is SISDeviceRequirements sisComponent)
            {
                return sisComponent.TagName.StringValue;
            }

            if (node is Document document)
            {
                return document.Name.StringValue;
            }

            return node.GetType().Name;
        }

        public static string TypeDisplayName(this IAttribute attribute, out string? itemRefAttributeType)
        {
            var displayName = "";

            if (attribute.IsOrderedList)
            {
                var listItem = attribute.CreateItem();
                itemRefAttributeType = listItem.RefAttributeType;

                if (Definition.TryGetAttributeTypeName(listItem.RefAttributeType, out var typeName))
                {
                    displayName = $"List of {typeName} values";
                }

                if (Definition.TryGetAttributeUnit(listItem.RefAttributeType, out var unit))
                {
                    displayName = $"[{unit}] {displayName}";
                }
            }
            else
            {
                itemRefAttributeType = null;

                if (Definition.TryGetAttributeTypeName(attribute.RefAttributeType, out var typeName))
                {
                    displayName = typeName;
                }

                if (Definition.TryGetAttributeUnit(attribute.RefAttributeType, out var unit))
                {
                    displayName = $"[{unit}] {displayName}";
                }
            }

            return displayName;
        }

        public static string RequirementText(this IAttribute attribute)
        {
            if (Definition.TryGetAttributeTypeName(attribute.RefAttributeType, out var typeName) && typeName == "info")
            {
                return "";
            }

            if (attribute.IsMandatory)
            {
                return "Mandatory";
            }

            if (attribute.IsPartOfModel)
            {
                return "Optional";
            }

            return $"Not part of model version {Definition.Version}";
        }

    }
}

