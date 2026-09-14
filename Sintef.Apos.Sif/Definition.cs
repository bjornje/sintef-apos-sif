using Aml.Engine.CAEX;
using Sintef.Apos.Sif.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Sintef.Apos.Sif
{
    public static class Definition
    {
        public static readonly string Model;
        public const string SIFUnitClasses = "SIF Unit Classes";
        public const string SISUnitClasses = "SIS Unit Classes";
        const string BasicUnitsLib = "APOS.BasicUnits_AttributeTypeLib";
        const string FailureClassificationTypesLib = "APOS.FailureClassificationTypes_AttributeTypeLib";
        const string SIFTypesLib = "APOS.SIFTypes_AttributeTypeLib";
        const string SISDeviceTypesLib = "APOS.SISDeviceTypes_AttributeTypeLib";
        const string AutomationMLBaseAttributeTypeLib = "AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib";

        static Definition()
        {
            Model = GetEmbeddedResource("APOS_SIF.aml");

            ModelCAEX = CAEXDocument.LoadFromString(Model);
            Version = ModelCAEX.CAEXFile.SourceDocumentInformation.Select(x => x.OriginVersion).SingleOrDefault();

            var sifUnitClasses = ModelCAEX.CAEXFile.SystemUnitClassLib.FirstOrDefault(x => x.Name == SIFUnitClasses);
            var sisUnitClasses = ModelCAEX.CAEXFile.SystemUnitClassLib.FirstOrDefault(x => x.Name == SISUnitClasses);

            var basicUnits = ModelCAEX.CAEXFile.AttributeTypeLib.FirstOrDefault(x => x.Name == BasicUnitsLib);
            var failureClassificationTypes = ModelCAEX.CAEXFile.AttributeTypeLib.FirstOrDefault(x => x.Name == FailureClassificationTypesLib);
            var sifTypes = ModelCAEX.CAEXFile.AttributeTypeLib.FirstOrDefault(x => x.Name == SIFTypesLib);
            var sisDeviceTypes = ModelCAEX.CAEXFile.AttributeTypeLib.FirstOrDefault(x => x.Name == SISDeviceTypesLib);

            foreach (var item in basicUnits)
            {
                var refAttributeType = $"{BasicUnitsLib}/{item.Name}";

                _types.Add(refAttributeType, item);

                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(refAttributeType, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }

            foreach (var item in failureClassificationTypes)
            {
                var refAttributeType = $"{FailureClassificationTypesLib}/{item.Name}";

                _types.Add(refAttributeType, item);

                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(refAttributeType, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }


            foreach (var item in sifTypes)
            {
                var refAttributeType = $"{SIFTypesLib}/{item.Name}";

                _types.Add(refAttributeType, item);

                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(refAttributeType, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }


            foreach (var item in sisDeviceTypes)
            {
                var refAttributeType = $"{SISDeviceTypesLib}/{item.Name}";

                _types.Add(refAttributeType, item);

                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(refAttributeType, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }

            _typeValues.Add($"{BasicUnitsLib}/boolean", new[] { "true", "false" });

            foreach (var item in sifUnitClasses)
            {
                foreach (var attr in item.Attribute)
                {
                    SetAttribute(item.Name, attr);
                }

                foreach (var subItem in item.InternalElement)
                {

                    foreach (var attr in subItem.Attribute)
                    {
                        SetAttribute(subItem.Name, attr);
                    }

                    foreach (var subSubItem in subItem.InternalElement)
                    {
                        foreach (var attr in subSubItem.Attribute)
                        {
                            SetAttribute(subSubItem.Name, attr);
                        }
                    }

                    if (subItem.Name == "Documents")
                    {
                        foreach (var document in subItem.InternalElement)
                        {
                            var externalInterface = document.ExternalInterface.FirstOrDefault();
                            var roleRequirements = document.RoleRequirements.FirstOrDefault();

                            if (externalInterface == null || roleRequirements == null)
                            {
                                continue;
                            }

                            if (!_externalInterfaces.ContainsKey(externalInterface.Name))
                            {
                                _externalInterfaces.Add(externalInterface.Name, new ExternalInterfaceTemplate(externalInterface.Name, externalInterface.RefBaseClassPath, roleRequirements.RefBaseRoleClassPath));
                            }

                        }
                    }
                }

            }

            foreach (var item in sisUnitClasses)
            {

                foreach (var attr in item.Attribute)
                {
                    SetAttribute(item.Name, attr);
                }
            }

#if DEBUG
            var fileName = $@"{System.IO.Directory.GetCurrentDirectory()}\AttributeValues.cs";

            using (var stream = System.IO.File.CreateText(fileName))
            {
                stream.WriteLine("namespace Sintef.Apos.Sif.Model");
                stream.WriteLine("{");
                stream.WriteLine("    public static class AttributeValues");
                stream.WriteLine("    {");
                stream.WriteLine($"        public const string Version = \"{Version}\";");
                stream.WriteLine("    }");
                stream.WriteLine();

                foreach (var keyValue in _typeValues)
                {
                    var keySplit = keyValue.Key.Split('/');
                    var name = keySplit[keySplit.Length - 1];

                    stream.WriteLine($"    public static class v_{name}");
                    stream.WriteLine("    {");

                    foreach (var value in keyValue.Value)
                    {
                        var suffix = "";

                        if (value == "true" || value == "false")
                        {
                            suffix = "_";
                        }

                        var valueAsName = $"{value.Replace(' ', '_').Replace('-', '_').Replace(':', '_').Replace('/', '_').Replace(",", "").Replace("(", "").Replace(")", "")}{suffix}";
                        stream.WriteLine($"        public const string {valueAsName} = \"{value}\";");
                    }

                    stream.WriteLine("    }");
                    stream.WriteLine();
                }

                stream.WriteLine("}");
            }

#endif
        }

        private static Dictionary<string, AttributeFamilyType> _types = new Dictionary<string, AttributeFamilyType>();
        private static Dictionary<string, string[]> _typeValues = new Dictionary<string, string[]>();
        private static Dictionary<string, ExternalInterfaceTemplate> _externalInterfaces = new Dictionary<string, ExternalInterfaceTemplate>();

        public static bool IsOrdereListType(string refAttributeType)
        {
            return refAttributeType == $"{AutomationMLBaseAttributeTypeLib}/OrderedListType";
        }

        public static IList<string> GetTypes()
        {
            return _types.Keys.ToList();
        }
        public static bool TryGetExternalInterface(string name, out ExternalInterfaceTemplate externalInterface)
        {
            if (_externalInterfaces.TryGetValue(name, out externalInterface))
            {
                return true;
            }

            return false;
        }

        public static bool TryGetAttributeTypeDescription(string name, out string description)
        {
            if (_types.TryGetValue(name, out var attribute) && attribute.Description != null)
            {
                description = attribute.Description;
                return true;
            }

            description = null;
            return false;
        }

        public static bool TryGetAttributeUnit(string name, out string unit)
        {
            if (_types.TryGetValue(name, out var attribute) && attribute.Unit != null)
            {
                unit = attribute.Unit;
                return true;
            }

            unit = null;
            return false;
        }

        public static bool TryGetAttributeTypeName(string name, out string typeName)
        {
            if (_types.TryGetValue(name, out var attribute) && attribute.Name != null)
            {
                typeName = attribute.Name;
                return true;
            }

            if (name.StartsWith("xs:"))
            {
                typeName = name.Substring(3);
                return true;
            }

            if (name == "info")
            {
                typeName = "info";
                return true;
            }

            typeName = null;
            return false;
        }

        public static bool TryGetAttributeTypeValues(string name, out string[] values)
        {
            if (_typeValues.TryGetValue(name, out values))
            {
                return true;
            }

            return false;
        }

        private static void SetAttribute(string nodeName, AttributeType attr)
        {
            var attributeName = attr.Name;
            var attributeDescription = attr.Description;
            var attributeDataType = attr.Class?.AttributeDataType;

            if (!_attributes.TryGetValue(nodeName, out var attributes))
            {
                attributes = new List<IAttribute>();
                _attributes[nodeName] = attributes;
            }

            if (attributes.Exists(x => x.Name == attributeName))
            {
                throw new Exception("Duplicate attributeName: " + attributeName);
            }

            var isMandatory = false;
            if (attr.Constraint.FirstOrDefault(x => x.Name == "ModellingRule") is AttributeValueRequirementType requirementType &&
                requirementType.UnknownType.Requirements == "Mandatory")
            {
                isMandatory = true;
            }

            var isPartOfModel = true;
            var refAttributeType = attr.RefAttributeType;

            if (attributeDataType == null)
            {
                if (IsOrdereListType(refAttributeType))
                {
                    if (!(attr.Attribute.First is AttributeType itemType))
                    {
                        throw new Exception($"Missing item type template");
                    }

                    var itemAttributeDataType = itemType.AttributeTypeReference.AttributeDataType;

                    var itemRefAttributeType = itemType.RefAttributeType;

                    if (TryGetAttribute(itemAttributeDataType, attributeName, attributeDescription, refAttributeType, itemRefAttributeType, isMandatory, isPartOfModel, out var attributeList))
                    {
                        attributes.Add(attributeList);
                    }
                    else
                    {
                        throw new Exception($"Failed to instanciate ordered list attribute {attributeName} with data type: " + itemAttributeDataType);
                    }

                    return;
                }

                if (attributeName == "aml-DocLang")
                {
                    return;
                }

                throw new Exception($"Undefined attribute type ref {refAttributeType} for {attributeName}");
            }

            if (TryGetAttribute(attributeDataType, attributeName, attributeDescription, refAttributeType, null, isMandatory, isPartOfModel, out var attribute))
            {
                attributes.Add(attribute);
            }
            else
            {
                throw new Exception($"Failed to instanciate attribute {attributeName} with data type: " + attributeDataType);
            }

        }

        public static CAEXDocument ModelCAEX { get; }
        public static string Version { get; }

        private static Dictionary<string, List<IAttribute>> _attributes = new Dictionary<string, List<IAttribute>>();
        private static Dictionary<string, List<IAttribute>> _additionlAttributes = new Dictionary<string, List<IAttribute>>();

        private static bool TryGetAttribute(string dataType, string name, string description, string typeRef, string itemTypeRef, bool isMandatory, bool isPartOfModel, out IAttribute attribute)
        {
            switch (dataType)
            {
                case "xs:string":
                    if (itemTypeRef == null)
                    {
                        attribute = new Attribute<string>(name, description, typeRef, isMandatory, isPartOfModel, null);
                    }
                    else
                    {
                        attribute = new AttributeList<string>(name, description, typeRef, itemTypeRef, isMandatory, isPartOfModel, null);

                    }
                    return true;
                case "xs:decimal":
                case "xs:duration":
                    if (itemTypeRef == null)
                    {
                        attribute = new Attribute<double?>(name, description, typeRef, isMandatory, isPartOfModel, null);
                    }
                    else
                    {
                        attribute = new AttributeList<double?>(name, description, typeRef, itemTypeRef, isMandatory, isPartOfModel, null);

                    }
                    return true;
                case "xs:integer":
                    if (itemTypeRef == null)
                    {
                        attribute = new Attribute<int?>(name, description, typeRef, isMandatory, isPartOfModel, null);
                    }
                    else
                    {
                        attribute = new AttributeList<int?>(name, description, typeRef, itemTypeRef, isMandatory, isPartOfModel, null);

                    }
                    return true;
                case "xs:boolean":
                    if (itemTypeRef == null)
                    {
                        attribute = new Attribute<bool?>(name, description, typeRef, isMandatory, isPartOfModel, null);
                    }
                    else
                    {
                        attribute = new AttributeList<bool?>(name, description, typeRef, itemTypeRef, isMandatory, isPartOfModel, null);

                    }
                    return true;
                default:
                    attribute = null;
                    return false;
            }
        }

        public static bool IstOldTypeRef(string oldRefAttributeType, out string refAttributeType)
        {
            if (oldRefAttributeType == null || !oldRefAttributeType.StartsWith("Types/"))
            {
                refAttributeType = null;
                return false;
            }

            var className = oldRefAttributeType.Substring(6);

            switch (className)
            {
                case "String":
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/string";
                    return true;
                case "Frequency":
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/frequency_peryear";
                    return true;
                case "Percent":
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/percent";
                    return true;
                case "Hours":
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/duration_hours";
                    return true;
                case "Seconds":
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/duration_seconds";
                    return true;
                case "Integer":
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/integer";
                    return true;
                case "SILLevel":
                    refAttributeType = "APOS.SIFTypes_AttributeTypeLib/SILLevel";
                    return true;
                case "PerYear":
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/frequency_peryear";
                    return true;
                case "SIFType":
                    refAttributeType = "APOS.SIFTypes_AttributeTypeLib/SIFType";
                    return true;
                case "ModeOfOperation":
                    refAttributeType = "APOS.SIFTypes_AttributeTypeLib/modeOfOperation";
                    return true;
                case "kg_s":
                    refAttributeType = "APOS.FailureClassificationTypes_AttributeTypeLib/leakagerate_kg_s";
                    return true;
                case "Boolean":
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/boolean";
                    return true;
                case "Comparison":
                    refAttributeType = "APOS.FailureClassificationTypes_AttributeTypeLib/comparison";
                    return true;
                case "TypeAB":
                    refAttributeType = "APOS.SISDeviceTypes_AttributeTypeLib/typeAB";
                    return true;
                case "E_DEToTrip":
                    refAttributeType = "APOS.SISDeviceTypes_AttributeTypeLib/tripEnergyMode";
                    return true;
                default:
                    refAttributeType = "APOS.BasicUnits_AttributeTypeLib/string";
                    return true;
            }
        }

        public static bool TryAddAttribute(string attributeName, string refAttributeType, bool isOrderedList, string targetNode)
        {
            if (!_additionlAttributes.TryGetValue(targetNode, out var attributes))
            {
                attributes = new List<IAttribute>();
                _additionlAttributes.Add(targetNode, attributes);
            }

            if (refAttributeType != null && _types.TryGetValue(refAttributeType, out var value))
            {
                if (isOrderedList)
                {
                    if (TryGetAttribute(value.AttributeDataType, attributeName, $"This attribute does not belong to the current UML-model version {Version}", $"{AutomationMLBaseAttributeTypeLib}/OrderedListType", refAttributeType, false, false, out var attribute))
                    {
                        attributes.Add(attribute);
                        return true;
                    }
                }
                else
                {
                    if (TryGetAttribute(value.AttributeDataType, attributeName, $"This attribute does not belong to the current UML-model version {Version}", refAttributeType, null, false, false, out var attribute))
                    {
                        attributes.Add(attribute);
                        return true;
                    }
                }

            }

            return false;
        }

        public static IEnumerable<IAttribute> GetAdditionalAttributes(string targetNode)
        {
            if (_additionlAttributes.TryGetValue(targetNode, out var value))
            {
                return value;
            }

            return Enumerable.Empty<IAttribute>();
        }

        public static IEnumerable<IAttribute> GetAttributes(Node node)
        {
            var attributes = new List<IAttribute>();

            if (node is SISDeviceRequirements && _attributes.TryGetValue("SISDeviceRequirements", out var sisComponentAttributes))
            {
                attributes.AddRange(sisComponentAttributes);
            }

            var name = node.GetType().Name;

            if (_attributes.TryGetValue(name, out var ownAttributes))
            {
                attributes.AddRange(ownAttributes);
            }

            var duplicateNames = attributes.GroupBy(attribute => attribute.Name)
                                 .FirstOrDefault(group => group.Count() > 1);

            if (duplicateNames != null)
            {
                throw new FormatException("Duplicate attribute name: " + duplicateNames.Key);
            }

            return attributes;
        }

        public static Collection<IAttribute> GetAttributes(Node target, int expectedNumberOfAttributes)
        {
            var type = target.GetType();
            var attributes = GetAttributes(target);
            if (attributes.Count() != expectedNumberOfAttributes)
            {
                foreach (var prop in type.GetProperties())
                {
                    if (!attributes.Any(x => x.Name == prop.Name))
                    {
                        Console.WriteLine(prop.Name);
                    }
                }

                throw new FormatException($"Expected {expectedNumberOfAttributes} attributes but got {attributes.Count()}.");
            }


            var attributeTypes = new Collection<IAttribute>();

            foreach (var attribute in attributes)
            {
                var propertyInfo = type.GetProperty(attribute.Name);
                if (propertyInfo == null)
                {
                    throw new FormatException($"No property exists for attribute {attribute.Name}.");
                }

                var attributeClone = attribute.Clone(target);
                propertyInfo.SetValue(target, attributeClone);
                attributeTypes.Add(attributeClone);
            }

            return attributeTypes;
        }

        private static string GetEmbeddedResource(string name)
        {
            // Get the current assembly
            Assembly assembly = Assembly.GetExecutingAssembly();

            var resourceFullName = assembly.GetName().Name + ".Resources." + name;

            // Resource names are case-sensitive and follow the pattern: ProjectNamespace.Folder.Filename.Extension
            using (Stream stream = assembly.GetManifestResourceStream(resourceFullName))
            {
                if (stream == null)
                {
                    throw new ArgumentException($"Resource '{resourceFullName}' not found. Verify the namespace path.");
                }

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
