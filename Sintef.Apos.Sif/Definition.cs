using Aml.Engine.CAEX;
using Sintef.Apos.Sif.Model;
using Sintef.Apos.Sif.Model.Attributes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;
using String = Sintef.Apos.Sif.Model.Attributes.String;

namespace Sintef.Apos.Sif
{
    public static class Definition
    {
        public const string SIFUnitClasses = "SIF Unit Classes";
        public const string SISUnitClasses = "SIS Unit Classes";
        const string BasicUnitsLib = "APOS.BasicUnits_AttributeTypeLib";
        const string FailureClassificationTypesLib = "APOS.FailureClassificationTypes_AttributeTypeLib";
        const string SIFTypesLib = "APOS.SIFTypes_AttributeTypeLib";
        const string SISDeviceTypesLib = "APOS.SISDeviceTypes_AttributeTypeLib";
        static Definition()
        {
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

                if (!string.IsNullOrEmpty(item.Description))
                {
                    _typeDescriptions.Add(refAttributeType, item.Description);
                }


                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(refAttributeType, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }

            foreach (var item in failureClassificationTypes)
            {
                var refAttributeType = $"{FailureClassificationTypesLib}/{item.Name}";

                if (!string.IsNullOrEmpty(item.Description))
                {
                    _typeDescriptions.Add(refAttributeType, item.Description);
                }


                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(refAttributeType, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }


            foreach (var item in sifTypes)
            {
                var refAttributeType = $"{SIFTypesLib}/{item.Name}";

                if (!string.IsNullOrEmpty(item.Description))
                {
                    _typeDescriptions.Add(refAttributeType, item.Description);
                }


                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(refAttributeType, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }


            foreach (var item in sisDeviceTypes)
            {
                var refAttributeType = $"{SISDeviceTypesLib}/{item.Name}";

                if (!string.IsNullOrEmpty(item.Description))
                {
                    _typeDescriptions.Add(refAttributeType, item.Description);
                }


                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(refAttributeType, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }

            //var expectedTypeValues = new[] { 
            //    "TypeAB", //SISComponent
            //    "SISType", //*** not in use ***
            //    "SILLevel", //SIF
            //    "SIFType", //SIF
            //    "ResetAfterShutdown_FinalElement", //FinalElementComponent
            //    "ModeOfOperation", //SIF
            //    "ManualActivation", //SIF
            //    "InputDeviceTrigger", //*** not in use ***
            //    "ILLevel", //SIF
            //    "FinalElementFunction", //*** not in use ***
            //    "FailSafePosition", //*** not in use ***
            //    "EnergizeSource", //*** not in use ***
            //    "EffectActivationMode", //*** not in use ***
            //    "E_DEToTrip", //SISComponent
            //    "Comparison", //SISComponent
            //    "CauseRole", //*** not in use ***
            //    "BypassControl", //SISComponent
            //    "AlarmOrWarning", //InputDeviceComponent
            //    "Application", //*** new, not in use ***
            //    "DetectedOrUndetected", //*** new, not in use ***
            //    "DetectionMethods", //*** new, not in use ***
            //    "DeviceType_ValveTopside", //*** new, not in use ***
            //    "ExternalExposure", //*** new, not in use ***
            //    "FailPass", //new, SISComponent
            //    "FailureTypes", //*** new, not in use ***
            //    "FluidSeverity", //*** new, not in use ***
            //    "SystematicOrRHF", //*** new, not in use ***
            //};
            //if (_typeValues.Count != expectedTypeValues.Length) throw new Exception($"Expected number of attribute types with value list {expectedTypeValues.Length} differes from actual number {_typeValues.Count}.");

            //foreach (var type in expectedTypeValues) if (!_typeValues.ContainsKey(type)) throw new Exception($"Attribute type with value list not found: {type}.");

            _typeValues.Add($"{BasicUnitsLib}/boolean", new[] { "true", "false" });

            foreach (var item in sifUnitClasses)
            {
                foreach (var attr in item.Attribute)
                {
                    SetAttribute(item.Name, attr);
                }

                foreach (var subItem in item.InternalElement)
                {
                    var documentLink = subItem.ExternalInterface.FirstOrDefault(x => x.Name == "DocumentLink");
                    if (documentLink != null)
                    {
                        var x = documentLink.GetType();
                    }

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
                }

            }

            foreach (var item in sisUnitClasses)
            {

                foreach (var attr in item.Attribute)
                {
                    SetAttribute(item.Name, attr);
                }
            }


        }

        private static Dictionary<string, string> _typeDescriptions = new Dictionary<string, string>();
        private static Dictionary<string, string[]> _typeValues = new Dictionary<string, string[]>();

        public static bool TryGetAttributeTypeDescription(string name, out string description)
        {
            if (_typeDescriptions.TryGetValue(name, out description))
            {
                return true;
            }

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

        private static void SetAttribute(string nodeName, Aml.Engine.CAEX.AttributeType attr)
        {
            var attributeName = attr.Name;
            var attributeDescription = attr.Description;
            var className = attr.Class?.Name;

            if (!_attributes.TryGetValue(nodeName, out var attributes))
            {
                attributes = new List<Model.Attributes.AttributeType>();
                _attributes[nodeName] = attributes;
            }

            if (className == null)
            {
                Console.Write("Undefined attribute class for: " + attributeName);
                return;
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

            var refAttributeType = attr.RefAttributeType;

            switch (className)
            {
                case "accuracy":
                    attributes.Add(new Accuracy(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "assetIntegrityLevel":
                    attributes.Add(new AssetIntegrityLevel(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "boolean":
                    attributes.Add(new Model.Attributes.Boolean(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "comparison":
                    attributes.Add(new Comparison(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "duration_hours":
                    attributes.Add(new DurationHours(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "duration_seconds":
                    attributes.Add(new DurationSeconds(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "environmentalExtremes":
                    attributes.Add(new EnvironmentalExtremes(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "environmentalIntegrityLevel":
                    attributes.Add(new EnvironmentalIntegrityLevel(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "failurePhilosophy":
                    attributes.Add(new FailurePhilosophy(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "frequency_perhour":
                    attributes.Add(new FrequecyPerHour(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "frequency_peryear":
                    attributes.Add(new FrequecyPerYear(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "integer":
                    attributes.Add(new Integer(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "leakagerate_kg_s":
                    attributes.Add(new LeakageRateKilogramsPerSecond(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "modeOfOperation":
                    attributes.Add(new ModeOfOperation(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "percent":
                    attributes.Add(new Percent(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "probability":
                    attributes.Add(new Probability(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "rangeMax":
                    attributes.Add(new RangeMax(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "rangeMin":
                    attributes.Add(new RangeMin(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "resetAfterShutdown_FinalElement":
                    attributes.Add(new ResetAfterShutdown_FinalElement(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "SCLevel":
                    attributes.Add(new SCLevel(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "SIFType":
                    attributes.Add(new SIFType(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "SILLevel":
                    attributes.Add(new SILLevel(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "string":
                    attributes.Add(new String(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "tagName":
                    attributes.Add(new TagName(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "tripEnergyMode":
                    attributes.Add(new TripEnergyMode(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "typeAB":
                    attributes.Add(new TypeAB(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                case "unitOfMeasure":
                    attributes.Add(new UnitOfMeasure(attributeName, attributeDescription, refAttributeType, isMandatory, null));
                    break;
                default:
                    Console.Write("Unknown attribute class: " + className);
                    break;
            }
        }

        public static CAEXDocument ModelCAEX { get; }
        public static string Version { get; }

        private static Dictionary<string, List<Model.Attributes.AttributeType>> _attributes = new Dictionary<string, List<Model.Attributes.AttributeType>>();

        public static IEnumerable<Model.Attributes.AttributeType> GetAttributes(Node node)
        {
            var attributes = new List<Model.Attributes.AttributeType>();

            if (node is Subsystem)
            {
                if (_attributes.TryGetValue("Subsystem", out var sifComponentAttributes))
                {
                    attributes.AddRange(sifComponentAttributes);
                }
            }
            else if (node is SISDeviceRequirements)
            {
                if (_attributes.TryGetValue("SISDeviceRequirements", out var sisComponentAttributes))
                {
                    attributes.AddRange(sisComponentAttributes);
                }
            }

            var name = node.GetType().Name;

            if (name == "SIF")
            {
                name = "SafetyInstrumentedFunction";
            }
            else if (name == "InputDeviceComponent")
            {
                name = "InputDeviceRequirements";
            }
            else if (name == "LogicSolverComponent")
            {
                name = "LogicSolverRequirements";
            }
            else if (name == "FinalElementComponent")
            {
                name = "FinalElementRequirements";
            }

            if (_attributes.TryGetValue(name, out var ownAttributes))
            {
                attributes.AddRange(ownAttributes);
            }

            foreach (var item in attributes)
            {
                if (attributes.Count(x => x.Name == item.Name) > 1)
                {
                    throw new Exception("Duplicate attribute name: " + item.Name);
                }
            }

            return attributes;
        }

        public static Collection<Model.Attributes.AttributeType> GetAttributes(Node target, int expectedNumberOfAttributes)
        {
            var attributes = GetAttributes(target);
            if (attributes.Count() != expectedNumberOfAttributes)
            {
                throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {attributes.Count()}.");
            }

            var type = target.GetType();

            var attributeTypes = new Collection<Model.Attributes.AttributeType>();

            foreach (var attribute in attributes)
            {
                var propertyInfo = type.GetProperty(attribute.Name);
                if (propertyInfo == null)
                {
                    throw new Exception($"No property exists for attribute {attribute.Name}.");
                }

                var attributeClone = attribute.Clone(target);
                propertyInfo.SetValue(target, attributeClone);
                attributeTypes.Add(attributeClone);
            }

            return attributeTypes;
        }

        public static readonly string Model = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CAEXFile SchemaVersion=""3.0"" FileName="""" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance""
    xmlns=""http://www.dke.de/CAEX""
    xsi:schemaLocation=""http://www.dke.de/CAEX CAEX_ClassModel_V.3.0.xsd"">
    <Description>This document is generated from the APOS_SIF.qea model version 26 by the
        EA_UML_to_AML.js transformation script.
        The model and transformation script are derived from initial SIF model and transformation
        script from Kongsberg.</Description>
    <AdditionalInformation />
    <SuperiorStandardVersion>AutomationML 2.10</SuperiorStandardVersion>
    <SourceDocumentInformation OriginName=""APOS_SIF.qea"" OriginID="""" OriginVersion=""26""
        LastWritingDateTime=""2024-12-02T10:21:46.000Z"" OriginVendor=""SINTEF""
        OriginVendorURL=""www.sintef.no"" />
    <ExternalReference
        Path=""https://9t4kqL7NX8JsW6o@automationml.ovgu.de/public.php/webdav/AutomationML_Base_Libraries_AMLEd2_2.11.0.aml""
        Alias=""AutomationMLBaseLibrariesAMLEd22_11_0"" />
    <SystemUnitClassLib Name=""SIF Unit Classes"">
        <Description></Description>
        <Version>1.0</Version>
        <SystemUnitClass Name=""Group"" ID=""{91B2A42C-84AD-4c59-9520-E18974D4B24B}"">
            <Description>group of tags / functional locations / devices in a safety instrumented
                function</Description>
            <Version>1.0</Version>
            <Attribute Name=""MInVotingMooN"" ID=""{9F5BA9BA-7FAA-45f9-910A-F5862E1695FB}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/integer"">
                <Description>""M"" in a ""MooN"" system that is functioning if at least M out of its N
                    redundant channels are functioning</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""NumberOfDevicesWithinGroup"" ID=""{F034ACC7-4B73-42c5-A89E-4EC61DCC639A}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/integer"">
                <Description>number of devices within an input device group, logic solver group, or
                    final element group</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""AllowAnyComponents"" ID=""{ABC76BF3-4F15-4456-A05A-74C0E18A1ABB}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/boolean"">
                <Description>specifies if a group of components can comprise components from two or
                    more SIF subsystems (input devices, logic solvers and final elements)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""SafetyInstrumentedFunction""
            ID=""{33E2F2BF-C9F7-4ac5-8B0B-6C216BD4B771}"">
            <Description>safety function to be implemented by a safety instrumented system (SIS)</Description>
            <Version>1.0</Version>
            <Attribute Name=""MaximumAllowableDemandRate"" ID=""{85F435A5-3D42-443b-8CF0-164DAE95CAE9}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/frequency_peryear"">
                <Description>maximum allowable demand rate (demands per year) for a safety
                    instrumented function (SIF)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MaximumAllowableSIFResponseTime""
                ID=""{029EE774-2441-495d-9620-3C0DAF70D95C}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_seconds"">
                <Description>maximum allowable time period between the process reaching the SIF
                    activation set point and the SIF final elements completing the necessary actions
                    to achieve the process safe state</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SafeStateOfProcess"" ID=""{DEBD4AAE-D31F-4e6b-BD46-C3A8AEEE0BEB}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>state of the process when safety is achieved</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SIFID"" ID=""{6D45C9AF-E507-4f24-9174-73086558496E}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>unique human-readable identifier of a safety instrumented function
                    (SIF)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SafetyIntegrityLevelRequirement""
                ID=""{78A8B42B-6001-4f06-9CED-F5E7210A85E1}""
                RefAttributeType=""APOS.SIFTypes_AttributeTypeLib/SILLevel"">
                <Description>required level (one out of four) of a safety instrumented function
                    (SIF) for the safety integrity of the safety instrumented system (SIS)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SILAllocationMethod"" ID=""{91D515DA-1C3E-428e-8820-45A9DC5C7DA1}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>method applied for allocating SIL requirement to a SIF</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SIFDescription"" ID=""{29F0BCBD-8483-48fa-8630-D3F32FB47D0B}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>description of the design intent of the SIF</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SIFName"" ID=""{A0E538F3-6F27-476d-8FDF-8FD31444441A}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>human-readable name of a safety instrumented function (SIF)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SIFType"" ID=""{8E9F05B7-CAEB-420e-B5EA-8E41099A501B}""
                RefAttributeType=""APOS.SIFTypes_AttributeTypeLib/SIFType"">
                <Description>specifies whether the safety instrumented function (SIF) is a local SIF
                    or a global SIF</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""ModeOfOperation"" ID=""{DCFB6E80-4EC8-4f55-989F-2A01B6AD7087}""
                RefAttributeType=""APOS.SIFTypes_AttributeTypeLib/modeOfOperation"">
                <Description>way in which a safety-related system is intended to be used, with
                    respect to the frequency of demands made upon it, which may be either low demand
                    mode, where the frequency of demands for operation made on a safety related,
                    system is no greater than one per year and no greater than twice the proof-test,
                    frequency; or high demand or continuous mode, where the frequency of demands for
                    operation made, on a safety-related system is greater than one per year or
                    greater than twice the proof check frequency</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""Cause"" ID=""{CE8E154B-674E-40a1-9BD4-9E9F4CBD4CD3}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>occurrence in a production process which initiates a reaction of a
                    technical system</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""Effect"" ID=""{4B7289A7-2C11-4737-BDDF-1CD5520016FF}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>reaction of a technical system to a cause</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""QuantificationMethodOrTool"" ID=""{60B82A16-13F8-48e5-A993-E429C5DB87FC}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>method and/or tool used for quantification of probability of failure on
                    demand (PFD), probability of failure per hour (PFH) and/or the common cause
                    failure (CCF)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
                <Attribute Name=""1"" AttributeDataType=""APOS.BasicUnits_AttributeTypeLib/string"" />
            </Attribute>
            <Attribute Name=""EnvironmentalIntegrityLevelRequirement""
                ID=""{6A963F8D-A12D-4a29-9C85-5FEF7CB75A2A}""
                RefAttributeType=""APOS.SIFTypes_AttributeTypeLib/environmentalIntegrityLevel"">
                <Description>required level (one out of four) allocated to the safety instrumented
                    function (SIF) for specifying the environmental integrity requirement to be
                    achieved by the safety instrumented system (SIS)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""AssetIntegrityLevelRequirement""
                ID=""{40CEA4E0-D76E-45c5-8600-C08F387F130A}""
                RefAttributeType=""APOS.SIFTypes_AttributeTypeLib/assetIntegrityLevel"">
                <Description>required level (one out of four) of a safety instrumented function
                    (SIF) for specifying the asset integrity of the safety instrumented system (SIS)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""PFDRequirement"" ID=""{4BE0501A-3E0F-4c2d-93E9-9EECF215F1DB}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/probability"">
                <Description>maximum allowable (average) Probability of dangerous failure on demand
                    (PFD)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""PlantOperatingMode"" ID=""{17C19438-2B6D-4407-8700-631E7695C6AA}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>process operating mode, any planned state of process operation,
                    including modes such as start-up after emergency shutdown, normal start-up,
                    operation, temporary operation, emergency operation and shutdown.</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""PFHRequirement"" ID=""{7CBCC5DF-724B-433b-AA4B-B5893A9BE9E5}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/frequency_perhour"">
                <Description>maximum allowable average frequency of dangerous failure (failures per
                    hour)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""DemandSource"" ID=""{AB1AFDE9-3A3F-4607-B1E0-C0363EB54B2A}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>origin of the demand (why it occurred)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MaximumAllowableSpuriousTripRate""
                ID=""{CE771A94-AB81-4417-90BE-6D274B48A3E2}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/frequency_perhour"">
                <Description>maximum allowable spurious trip rate for a safety instrumented function
                    or a component</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""ManuallyActivatedShutdownRequirement""
                ID=""{28168C89-5378-40be-841B-4058965B39F0}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>requirement for being able to manually bring the process to a safe
                    state</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MeasureToAvoidCommonCauseFailures""
                ID=""{FAFD4587-C5E2-45ac-BBC1-4B8A4CB2368B}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>specific action or intervention designed to reduce the probability of
                    common cause failures</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SurvaivabilityRequirement"" ID=""{039EE23D-5153-488d-8930-62F1FFA8D4BB}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>requirements for the safety systems and barriers to function in or
                    after a design accidental event</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""ReferenceToEquipmentUnderControl""
                ID=""{3590BE2B-BA33-40d1-843C-4F6D3F44CA1E}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>reference to block EquipmentUnderControl</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
                <Attribute Name=""1"" AttributeDataType=""APOS.BasicUnits_AttributeTypeLib/string"" />
            </Attribute>
            <Attribute Name=""SIFTypicalID"" ID=""{70F9AE68-B985-47db-853C-93D2D2A02A1F}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>unique human-readable identifier of a SIF typical</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SIFVersion"" ID=""{52A52B21-9ACB-485f-90A3-3AF143A1D6D9}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>Version of SIF</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""ReferenceToBarrier"" ID=""{88D123B9-19E7-4818-B96E-D7F3C692A8BD}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>reference to or description of barrier that the safety instrumented
                    function or part of the safety instrumented system represents</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""IndependentProtectionLayer"" ID=""{8847E81D-A676-41b5-8FC8-EEA697972966}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>any independent mechanism that reduces risk by control, prevention or
                    mitigation</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <InternalElement Name=""Document1"" ID=""{7A41F664-9972-4d58-81AE-7078CB05359A}"">
                <Attribute Name=""aml-DocLang"" AttributeDataType=""xs:string"">
                    <Value>en-US</Value>
                </Attribute>
                <ExternalInterface Name=""DocumentLink""
                    RefBaseClassPath=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLInterfaceClassLib/AutomationMLBaseInterface/ExternalDataConnector/ExternalDataReference""
                    ID=""0a5ec13b-949e-4ac1-894b-0875d96fa204"" />
                <RoleRequirements
                    RefBaseRoleClassPath=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseRoleClassLib/AutomationMLBaseRole/ExternalData"" />
            </InternalElement>
            <InternalElement Name=""InputDeviceSubsystem"" ID=""{63D8A4F8-DEF5-4ead-BA95-B3F8399A1B77}""
                RefBaseSystemUnitPath=""SIF Unit Classes/Subsystem"">
                <Description>part of the BPCS or SIS that measures or detects the process condition
                    and/or provides input information for the logic solver</Description>
            </InternalElement>
            <InternalElement Name=""LogicSolverSubsystem"" ID=""{E8D9FD92-23B6-4ed8-8E97-088523E92953}""
                RefBaseSystemUnitPath=""SIF Unit Classes/Subsystem"">
                <Description>part of either a BPCS or SIS that performs one or more logic
                    function(s)</Description>
            </InternalElement>
            <InternalElement Name=""FinalElementSubsystem""
                ID=""{8A647C27-BADF-4690-85BC-4FE8DE6B7178}""
                RefBaseSystemUnitPath=""SIF Unit Classes/Subsystem"">
                <Description>part of the BPCS or SIS that implements the physical action necessary
                    to achieve or maintain a safe state</Description>
            </InternalElement>
        </SystemUnitClass>
        <SystemUnitClass Name=""Subsystem"" ID=""{CF723864-9C1F-4347-98E5-1F4F1044E931}"">
            <Description>independent part of a SIS whose disabling dangerous failure results in a
                disabling dangerous failure of the SIS</Description>
            <Version>1.0</Version>
            <Attribute Name=""PFDBudget"" ID=""{659C7645-93DB-438d-961E-A514F4E9722F}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/percent"">
                <Description>budget (in percentage) of the total PFD for the SIF that is allocated
                    to a component or device a group of devices</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MInVotingMooN"" ID=""{67DD139F-A86B-45c5-ABD8-92801CAACD51}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/integer"">
                <Description>""M"" in a ""MooN"" system that is functioning if at least M out of its N
                    redundant channels are functioning</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""NumberOfGroups"" ID=""{A89B3118-0E72-4a5b-AA43-83064A7CB188}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/integer"">
                <Description>total number of input device groups, logic solver groups and final
                    element groups specified for a safety instrumented function (SIF)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
        </SystemUnitClass>
    </SystemUnitClassLib>
    <SystemUnitClassLib Name=""SIS Unit Classes"">
        <Description></Description>
        <Version>1.0</Version>
        <SystemUnitClass Name=""ESD"" ID=""{908C6D3A-3EF5-4a72-9E3C-235EF45A45BA}""
            RefBaseClassPath=""SIS Unit Classes/SafetyInstrumentedSystem"">
            <Description>emergency shutdown: to shut down rapidly in order to prevent or remedy a
                dangerous situation</Description>
            <Version>1.0</Version>
        </SystemUnitClass>
        <SystemUnitClass Name=""FG"" ID=""{FB636EC0-2CEC-47e0-9A7A-E22E64FE0F23}""
            RefBaseClassPath=""SIS Unit Classes/SafetyInstrumentedSystem"">
            <Description>fire and gas: to detect possible leakages of hydrocarbons and/or fire</Description>
            <Version>1.0</Version>
        </SystemUnitClass>
        <SystemUnitClass Name=""FinalElementRequirements"" ID=""{F3E586A2-A7B6-4d03-A990-887E933A10A3}""
            RefBaseClassPath=""SIS Unit Classes/SISDeviceRequirements"">
            <Description>requirements for a device within subsystem final elements</Description>
            <Version>1.0</Version>
            <Attribute Name=""MaximumAllowableLeakageRate""
                ID=""{2301FBEE-D799-4265-9ADE-5BEC70CF354B}""
                RefAttributeType=""APOS.FailureClassificationTypes_AttributeTypeLib/leakagerate_kg_s"">
                <Description>maximum allowable rate of leakage through a device</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""TightShutoffIsRequired"" ID=""{A6B5AF49-3226-415f-9712-87ABDD52524C}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/boolean"">
                <Description>specifies whether it is required with tight shut-off for the valve</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""ResetAfterShutdownRequirement""
                ID=""{04ABDB88-2FA3-458b-859C-ADC31E3E5137}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/resetAfterShutdown_FinalElement"">
                <Description>requirements for reset of a final element after shutdown, such as if it
                    is required a physical reset in field </Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""ManualOperationIsPossible"" ID=""{58DAA35B-8C15-4a86-ACE3-98F52292E12E}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/boolean"">
                <Description>specifies whether the implemented SIS device can be operated manually
                    or not</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <!-- <Attribute Name=""MaximumTestIntervalForSILCompliance""
                ID=""{A3C0D27B-A8AA-4d66-9A11-96B0BDA5859B}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_hours"">
                <Description>maximum allowable number of hours between two consecutive proof tests
                    (or partial stroke tests) for the device, to achieve SIL compliance.</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute> -->
            <!-- <Attribute Name=""TestCoverage"" ID=""{362DCA97-B3ED-4ce3-BBBB-C1EAB3BD310A}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/percent"">
                <Description>fraction of dangerous undetected failures assumed detected during a
                    test</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute> -->
        </SystemUnitClass>
        <SystemUnitClass Name=""InputDeviceRequirements"" ID=""{D0101401-669D-4476-8960-CB1FE6BE378E}""
            RefBaseClassPath=""SIS Unit Classes/SISDeviceRequirements"">
            <Description>requirements for a device within subsystem input devices</Description>
            <Version>1.0</Version>
            <Attribute Name=""AlarmType"" ID=""{685D2522-9C9F-4060-AFD9-F84E36A4E4E9}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>alarm attribute which give a distinction of the alarm condition</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""UnitOfMeasure"" ID=""{BAD5EF7D-EE9D-40f0-9ECD-A9F2887BAF60}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/unitOfMeasure"">
                <Description>short name of the standardized engineering unit for the value of a trip
                    point</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""TripPointValue"" ID=""{34C1AF7D-5E24-4492-A9DD-CA143D3C4187}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>value of a trip point variable</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
                <Attribute Name=""1""
                    AttributeDataType=""APOS.SISDeviceTypes_AttributeTypeLib/tripPointValue"" />
            </Attribute>
            <Attribute Name=""RangeMin"" ID=""{D1E09A88-A0E8-4dfd-BF70-E1C1B1542059}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/rangeMin"">
                <Description>term indicating lower range-limit</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""RangeMax"" ID=""{73CCE009-7DA4-40b1-AD33-77A897C92233}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/rangeMax"">
                <Description>term indicating upper-range limit</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""Accuracy"" ID=""{705C2451-F8CF-4cc1-A1B5-8B47035CD0E7}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/accuracy"">
                <Description>quality which characterizes the ability of a measuring instrument to
                    provide an indicated value close to a true value of the measurand expressed as a
                    percentage of reading, span or full range etc. under reference conditions</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""AlarmResetAfterShutdownIsRequired""
                ID=""{3B46276C-6321-4911-AA17-5A2D088F1B05}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/boolean"">
                <Description>specifies whether it is required to have alarm reset after shutdown for
                    the input device</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""AlarmDescription"" ID=""{2FA0F75E-4D8B-4747-8F48-3CCECDA61079}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>textual description of an alarm, such as the intention of the alarm</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""OtherAlarmType"" ID=""{5A2E2501-EC39-4961-8E33-C226C739B32E}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>alarm type other than HHH, HH, H, LLL, LL or L - to be specified</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MeasurementComparisonIsRequired""
                ID=""{32EC43E3-435F-46d4-A585-B93201B4E69C}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/boolean"">
                <Description>specifies whether measurement comparison is required or not, i.e.
                    whether another sensor that is used to implement additional diagnostics (e.g. a
                    parallel mounted BPCS-sensor) is required</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""LogicSolverRequirements"" ID=""{9E3D868A-21B5-479a-B2D1-BB6AFAAB9383}""
            RefBaseClassPath=""SIS Unit Classes/SISDeviceRequirements"">
            <Description>requirements for a device within subsystem logic solvers</Description>
            <Version>1.0</Version>
            <Attribute Name=""ResetAfterShutdownRequirement""
                ID=""{B3285EDD-F85E-46b1-BF8A-1B8763D8A65E}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/boolean"">
                <Description>requirements for reset of a final element after shutdown, such as if it
                    is required a physical reset in field </Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""Other"" ID=""{16A6B9E3-5548-4db6-B275-5D9807E07F09}""
            RefBaseClassPath=""SIS Unit Classes/SafetyInstrumentedSystem"">
            <Description>other system than ESD, PSD and FG. For instance HIPPS</Description>
            <Version>1.0</Version>
        </SystemUnitClass>
        <SystemUnitClass Name=""PSD"" ID=""{43DB928B-A6AC-4e8b-B1D1-4A6044E0B735}""
            RefBaseClassPath=""SIS Unit Classes/SafetyInstrumentedSystem"">
            <Description>process shutdown: to manipulate variables influencing the behavior of a
                process in order to obtain equipment protection</Description>
            <Version>1.0</Version>
        </SystemUnitClass>
        <SystemUnitClass Name=""SafetyInstrumentedSystem"" ID=""{C4C0E4FC-4D62-47a1-8458-FCA48F4F945D}"">
            <Description>instrumented system used to implement one or more safety instrumented
                functions (SIFs)</Description>
            <Version>1.0</Version>
            <Attribute Name=""SISID"" ID=""{85E6E820-BA06-4596-98D8-3586736B4079}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>unique human-readable identifier of a safety instrumented system (SIS)</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""InterfacingSystemSpecification""
                ID=""{E679A8C4-539D-4caa-B5DE-2E5E27F7AA79}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>specification or description of an interfacing system</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
                <Attribute Name=""1"" AttributeDataType=""APOS.BasicUnits_AttributeTypeLib/string"" />
            </Attribute>
            <Attribute Name=""RequirementForStartingUpAndRestartingSIS""
                ID=""{574EE055-151C-4569-A232-F015F267A000}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>any specific requirements related to the procedures for starting up and
                    restarting the SIS</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""SISDeviceRequirements"" ID=""{FF6852CF-0D99-4105-9F7C-8C12781E0A0C}"">
            <Description>Requirements defined for a device of a safety instrumented system (SIS),
                hardware with or without software, capable of performing a specified function</Description>
            <Version>1.0</Version>
            <Attribute Name=""MaximumTestIntervalForSILCompliance""
                ID=""{75F443E8-85CF-4769-9A6F-61DDD6C8B208}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_hours"">
                <Description>maximum allowable number of hours between two consecutive proof tests
                    (or partial stroke tests) for the device, to achieve SIL compliance.</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""TagName"" ID=""{907808CA-CE26-4d4a-8055-3E9B66B6D7C6}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/tagName"">
                <Description>alphanumeric character sequence uniquely identifying a measuring or
                    control point</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""PFDBudget"" ID=""{5948DF31-E6BF-433b-92F9-F04BF89BD48D}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/percent"">
                <Description>budget (in percentage) of the total PFD for the SIF that is allocated
                    to a component or device a group of devices</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""TagInformation"" ID=""{179BFC6E-EA57-4b2b-A82D-848425B076B9}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>description and information about a specific tag / functional location
                    and generic for the model (type) and instance</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SafeStateOfDevice"" ID=""{B17188F0-9532-4e38-A9BB-D0D5BBF3D735}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>state of the device when safety is achieved</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""TypeAOrB"" ID=""{78567E3D-737F-48c6-AF69-600EF2910A94}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/typeAB"">
                <Description>specifies whether the device is of type A or B according to IEC 61508</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""EquipmentType"" ID=""{888C8FD7-934A-403a-8592-3E5B92266376}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>particular feature of the design which is significantly different from
                    the other design(s) within the same equipment class</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""PFHBudget"" ID=""{9498669D-53B7-469f-ACAF-ABE4B9A3AC42}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/percent"">
                <Description>budget (in percentage) of the total PFH for the SIF that is allocated
                    to the a component or a group of components under consideration</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SystematicCapabilityRequirement""
                ID=""{19A99F65-098C-43c3-A130-3A7498E94EA5}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/SCLevel"">
                <Description>systematic capability required for a SIS component</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""TestCoverage"" ID=""{FDE4FDDE-6355-4acb-A53F-E12CF32993FD}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/percent"">
                <Description>fraction of dangerous undetected failures assumed detected during a
                    test</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MeanTimeToRestoration"" ID=""{EC241F8B-A8A0-474f-8222-FCFDAC723872}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_hours"">
                <Description>expected time to achieve restoration</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MinimumTestIntervalOperatorSpecification""
                ID=""{7A6CC76B-D147-4459-9259-3A08824195B6}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_hours"">
                <Description>minimum allowable number of hours between two consecutive proof tests
                    (or partial stroke tests), according to the plant operator specification</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MaximumAllowableSISDeviceResponseTime""
                ID=""{E082AFFE-62D6-47ea-AD37-985ED15A3233}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_seconds"">
                <Description>maximum allowable time period for the SIS device completing the
                    necessary actions to achieve the safe state of the device</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""ExpectedRepairTime"" ID=""{484220E8-0F5A-4785-B965-DFAFAFA38CFD}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_hours"">
                <Description></Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""FailCriterion"" ID=""{5E740012-D12C-43ae-A9BA-E6854C8A6A34}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>precise failure definition and the corresponding failure mode, and
                    thereby indirectly definition of the acceptability of (part of) a functional
                    test</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
                <Attribute Name=""1"" AttributeDataType=""APOS.BasicUnits_AttributeTypeLib/string"" />
            </Attribute>
            <Attribute Name=""NumericFailCriterionValue"" ID=""{EDD79564-6174-4c90-99B3-559C2E11A49C}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>requirement that the measured value upon a performed test (or demand)
                    is compared with for a test to fail</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""NumericFailCriterionOperator""
                ID=""{C26F54AE-337D-4bb5-9D21-5D6BD12F0814}""
                RefAttributeType=""APOS.FailureClassificationTypes_AttributeTypeLib/comparison"">
                <Description>operator (&gt;, &gt;=, &lt;, &lt;= or =) for comparison of the measured
                    value on a test and the fail criterion for a numerical fail criterion; measured
                    value + operator + fail criterion</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""NumericFailCriterionDescriptionOfMeasurement""
                ID=""{B30678A9-F14D-4c5b-A2B1-E10C333DB210}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/string"">
                <Description>textual description of the value or what to be measured upon a proof
                    test (or demand), for a numerical fail criterion</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""BypassAdministrativeControlRequired""
                ID=""{1DAD57C3-4F48-4f41-8C13-EA4F82FE7E46}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/boolean"">
                <Description>specifies whether administrative control to approve, remove and set a
                    bypass is required</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MaximumAllowableBypassTime"" ID=""{B5E92439-7FFD-4964-91DB-D4E5875E01DF}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_seconds"">
                <Description>maximum duration (in seconds) a bypass is allowed to be set</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""TimeDelayOfAction"" ID=""{382D9BDA-D26E-422d-8297-5DEF5FA26AC1}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>time delay in seconds before the action takes place</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""TripEnergyMode"" ID=""{3FC7CB67-CDA0-47d0-9E39-9B2F610B0EA9}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/tripEnergyMode"">
                <Description>whether energy needs to be supplied (energized) or cut off
                    (de-energized) to trip the device</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""SurvaibabilityRequirement"" ID=""{0D4DE559-767E-4293-B5BB-224D2D1BE40B}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>requirements for the safety systems and barriers to function in or
                    after a design accidental event</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""AdditionalDiagnosticRequirementForImplementation""
                ID=""{75D259C0-55B7-46cd-9D42-9A26C734D1DF}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>requirements for implementation and follow up of additional diagnostics
                    other than required from IEC 61511</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""FailurePhilosophy"" ID=""{CA19FE3A-80EF-45ab-8EC4-91AFAA1952F0}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/failurePhilosophy"">
                <Description>specifies whether the device shall remain in the same state upon
                    failure</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""DiagnosticRequired"" ID=""{16B8DDD2-AF20-46cf-929A-3263D72D2572}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>additional diagnostics required other than on-line diagnostics that is
                    covered by automatic response and manual response/action requirements amd proof
                    testing that is covered by requirements relating to proof test implementation</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""RequirementsForTesting"" ID=""{B4D9E10D-E358-4c91-A50F-0D02F3758A3C}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>requirements for testing other than test coverage, test interval and
                    what is given by the test procedure</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""EnvironmentalExtremes"" ID=""{C5919075-1F2B-4878-B789-49D05707266E}""
                RefAttributeType=""APOS.SISDeviceTypes_AttributeTypeLib/environmentalExtremes"">
                <Description>properties characterizing the extreme values which an influence
                    quantity can assume without damaging the device</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""FailureModeResponses"" ID=""{02F86559-787F-4d75-A4C6-8C471057FFE1}""
                RefAttributeType=""AutomationMLBaseLibrariesAMLEd22_11_0@AutomationMLBaseAttributeTypeLib/OrderedListType"">
                <Description>required response (automatic or manual) to a specific failure mode of a
                    device</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Optional</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
            <Attribute Name=""MaximumPermittedRepairTime"" ID=""{00EA4662-E8C6-47ab-AA7D-CD710372DA0C}""
                RefAttributeType=""APOS.BasicUnits_AttributeTypeLib/duration_hours"">
                <Description>maximum duration allowed to repair a fault after it has been detected</Description>
                <Constraint Name=""ModellingRule"">
                    <UnknownType>
                        <Requirements>Mandatory</Requirements>
                    </UnknownType>
                </Constraint>
            </Attribute>
        </SystemUnitClass>
    </SystemUnitClassLib>
    <AttributeTypeLib Name=""APOS.BasicUnits_AttributeTypeLib"">
        <Description></Description>
        <Version>1.0</Version>
        <AttributeType Name=""boolean"" ID=""{E51A2A9B-C0D5-4abf-8DBC-70B496ECDC47}""
            AttributeDataType=""xs:boolean"">
            <Description>specifies whether a statement is true or false</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""decimal"" ID=""{B2DA91BF-710E-4ee5-9DB7-DF70CD001239}""
            AttributeDataType=""xs:decimal"">
            <Description>decimal number</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""dimension"" ID=""{8DD8E408-B7D8-423E-8A61-83BD64D4CF70}"" Unit=""cm""
            AttributeDataType=""xs:decimal"">
            <Description>dimension measured in cm</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""duration_hours"" ID=""{3B360754-DE57-4e7c-BC69-EF2DCB603B16}"" Unit=""h""
            AttributeDataType=""xs:duration"">
            <Description>duration in hours as a decimal number</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""duration_seconds"" ID=""{1B18EB3E-1E0D-47e5-BCC8-F9DA3CB4F09A}"" Unit=""s""
            AttributeDataType=""xs:decimal"">
            <Description>duration in seconds as a decimal number</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""frequency_perhour"" ID=""{680F155E-B352-423c-8DD8-E408B7D8423E}""
            AttributeDataType=""xs:decimal"">
            <Description>number of events per hour as a decimal number</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""frequency_peryear"" ID=""{50D4F3B5-5ADD-41e7-BB29-4E7991FBA063}""
            AttributeDataType=""xs:decimal"">
            <Description>number of events per year as a decimal number</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""integer"" ID=""{A4662FA1-C30C-4dbb-8003-70950AEFC83C}""
            AttributeDataType=""xs:intenger"">
            <Description>number as integer</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""percent"" ID=""{1E32BB38-B1A9-4276-BED7-BECC41E448FA}""
            AttributeDataType=""xs:decimal"">
            <Description>percent as a decimal number</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""probability"" ID=""{BCC8F9DA-3CB4-F09A-AC4D-FF76977D9B72}""
            AttributeDataType=""xs:decimal"">
            <Description>probability as a decimal number</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""size_inches"" ID=""{8DD8E408-B7D8-423E-A68D-9A56CD215463}""
            AttributeDataType=""xs:decimal"">
            <Description>size measured in inches</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""string"" ID=""{A5BB6E10-32EC-4581-A89D-2ED558FFB8CB}""
            AttributeDataType=""xs:string"">
            <Description>text as an ordered sequence of characters</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""timestamp"" ID=""{BC69EF2D-CB60-3B16-BB2B-A4D61B5B0BC0}""
            AttributeDataType=""xs:dateTime"">
            <Description>date and time</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABN627/#001"" />
        </AttributeType>
    </AttributeTypeLib>
    <AttributeTypeLib Name=""APOS.FailureClassificationTypes_AttributeTypeLib"">
        <Description></Description>
        <Version>1.0</Version>
        <AttributeType Name=""leakagerate_kg_s"" ID=""{A2546FE1-4ACA-47a5-A1AA-F913C06620CC}""
            Unit=""kg/s"" AttributeDataType=""xs:decimal"">
            <Description>leakage rate in kg/s as a decimal number</Description>
            <Version>1.0</Version>
        </AttributeType>
        <AttributeType Name=""comparison"" ID=""{838053F1-DE1D-44ec-8925-9F868A53C72B}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>greater than</RequiredValue>
                    <RequiredValue>less than or equal</RequiredValue>
                    <RequiredValue>less than</RequiredValue>
                    <RequiredValue>greater than or equal</RequiredValue>
                    <RequiredValue>equal</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""detectedOrUndetected"" ID=""{AD2F5DB9-80D6-0DC3-A43E-955EC5FE668C}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>undetected</RequiredValue>
                    <RequiredValue>detected</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""failPass"" ID=""{AD2F5DB9-80D6-0DC3-AF5A-71335FA2D331}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>passed</RequiredValue>
                    <RequiredValue>failed</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""failureTypes"" ID=""{AD2F5DB9-80D6-0DC3-91DC-46B8C3E9D1D1}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>dangerous detected (DD)</RequiredValue>
                    <RequiredValue>dangerous undetected (DU)</RequiredValue>
                    <RequiredValue>spurious</RequiredValue>
                    <RequiredValue>safe undetected (SU)</RequiredValue>
                    <RequiredValue>safe detected (SD)</RequiredValue>
                    <RequiredValue>non-critical (NONC)</RequiredValue>
                    <RequiredValue>not applicable (NA)</RequiredValue>
                    <RequiredValue>safe</RequiredValue>
                    <RequiredValue>dangerous</RequiredValue>
                    <RequiredValue>degraded</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""independentOrCCF"" ID=""{A43E955E-C5FE-668C-A09F-F5A4DC7C2CC4}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>independent failure</RequiredValue>
                    <RequiredValue>common cause failure</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""systematicOrRHF"" ID=""{AD2F5DB9-80D6-0DC3-91FA-E7967265FC30}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>random hardware failure</RequiredValue>
                    <RequiredValue>systematic failure</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
    </AttributeTypeLib>
    <AttributeTypeLib Name=""APOS.SIFTypes_AttributeTypeLib"">
        <Description></Description>
        <Version>1.0</Version>
        <AttributeType Name=""assetIntegrityLevel"" ID=""{5E890B18-BC15-4c28-BFCC-9057FDA9E807}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>AIL 1</RequiredValue>
                    <RequiredValue>AIL 2</RequiredValue>
                    <RequiredValue>AIL 3</RequiredValue>
                    <RequiredValue>AIL 4</RequiredValue>
                    <RequiredValue>no AIL</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""environmentalIntegrityLevel""
            ID=""{BFCC9057-FDA9-E807-B966-D8379909F8CC}"" AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>EIL 1</RequiredValue>
                    <RequiredValue>EIL 2</RequiredValue>
                    <RequiredValue>EIL 3</RequiredValue>
                    <RequiredValue>EIL 4</RequiredValue>
                    <RequiredValue>no EIL</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""modeOfOperation"" ID=""{6F7C3CDE-30BC-4644-8C34-30BEE5823BA2}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABB910#006"" />
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>low demand</RequiredValue>
                    <RequiredValue>high demand</RequiredValue>
                    <RequiredValue>continuous</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""SIFType"" ID=""{FD2354A7-D96F-49fc-9006-741440576B96}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>global</RequiredValue>
                    <RequiredValue>local</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""SILLevel"" ID=""{0FF0E7E0-9EA4-4700-AB4A-659F54DF1ADF}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>SIL 1</RequiredValue>
                    <RequiredValue>SIL 2</RequiredValue>
                    <RequiredValue>SIL 3</RequiredValue>
                    <RequiredValue>SIL 4</RequiredValue>
                    <RequiredValue>SIL 0</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
    </AttributeTypeLib>
    <AttributeTypeLib Name=""APOS.SISDeviceTypes_AttributeTypeLib"">
        <Description></Description>
        <Version>1.0</Version>
        <AttributeType Name=""accuracy"" ID=""{B777EA2B-0F80-F26D-965A-20840469737B}""
            AttributeDataType=""xs:decimal"">
            <Description>quality which characterizes the ability of a measuring instrument to
                provide an indicated value close to a true value of the measurand expressed as a
                percentage of reading, span or full range etc. under reference conditions</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABD459#001"" />
        </AttributeType>
        <AttributeType Name=""environmentalExtremes"" ID=""{BF6AF5F6-338D-B32F-B14B-D28C13FDB001}""
            AttributeDataType=""xs:string"">
            <Description>properties characterizing the extreme values which an influence quantity
                can assume without damaging the device</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABC315#006"" />
        </AttributeType>
        <AttributeType Name=""fluid"" ID=""{BF6AF5F6-338D-B32F-94C3-E51CE7936D72}""
            AttributeDataType=""xs:string"">
            <Description>classification of a fluid according to its chemical composition or name</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABD525#005"" />
        </AttributeType>
        <AttributeType Name=""nameOfManufacturer"" ID=""{BF6AF5F6-338D-B32F-A68E-9BDD1FFF3967}""
            AttributeDataType=""xs:string"">
            <Description>name of the manufacturer of a component</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABA292#006"" />
        </AttributeType>
        <AttributeType Name=""physicalLocation"" ID=""{991D9E81-E3D7-A751-BF6A-F5F6338DB32F}""
            AttributeDataType=""xs:string"">
            <Description>plain text characterizing a physical location</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABB350#005"" />
        </AttributeType>
        <AttributeType Name=""productModelName"" ID=""{BF6AF5F6-338D-B32F-B890-645B2922E390}""
            AttributeDataType=""xs:string"">
            <Description>third level of a product description hierarchy as defined by the
                manufacturer, being the article or product designation under which the article or
                product is marketed</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABA567#009"" />
        </AttributeType>
        <AttributeType Name=""productTypeName"" ID=""{BF6AF5F6-338D-B32F-9D82-5E9C8F991D32}""
            AttributeDataType=""xs:string"">
            <Description>first level of a product description hierarchy as defined by the
                manufacturer, characterizing the article or product based on its usage, operation
                principle and its fabricated form</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABA566#007"" />
        </AttributeType>
        <AttributeType Name=""rangeMax"" ID=""{B777EA2B-0F80-F26D-BF0D-4910BA052E07}""
            AttributeDataType=""xs:decimal"">
            <Description>term indicating upper-range limit</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABM629#001"" />
        </AttributeType>
        <AttributeType Name=""rangeMin"" ID=""{B591D7EE-DCA0-745E-B777-EA2B0F80F26D}""
            AttributeDataType=""xs:decimal"">
            <Description>term indicating lower range-limit</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABM628#001"" />
        </AttributeType>
        <AttributeType Name=""serialNumber"" ID=""{C633DC30-2017-4d32-891C-B3700C0D0A5F}""
            AttributeDataType=""xs:string"">
            <Description>short name of the standardized engineering unit for the value of a trip
                point</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABA951#009"" />
        </AttributeType>
        <AttributeType Name=""tagName"" ID=""{1CCBE575-AEE3-40f0-991D-9E81E3D7A751}""
            AttributeDataType=""xs:string"">
            <Description>alphanumeric character sequence uniquely identifying a measuring or control
                point</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABB271#009"" />
        </AttributeType>
        <AttributeType Name=""tripPointValue"" ID=""{B591D7EE-DCA0-745E-B1F3-414D52BB76F8}""
            AttributeDataType=""xs:decimal"">
            <Description>value of a trip point variable</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABH706#001"" />
        </AttributeType>
        <AttributeType Name=""unitOfMeasure"" ID=""{BF6AF5F6-338D-B32F-9BA4-89CE1E3800A1}""
            AttributeDataType=""xs:string"">
            <Description>short name of the standardized engineering unit for the value of a trip
                point</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABH708#002"" />
        </AttributeType>
        <AttributeType Name=""alarmType"" ID=""{F6EE3886-84AA-48c6-B5C6-9FE733FAA597}""
            AttributeDataType=""xs:string"">
            <Description>alarm attribute which give a distinction of the alarm condition</Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>high high</RequiredValue>
                    <RequiredValue>high</RequiredValue>
                    <RequiredValue>low low low</RequiredValue>
                    <RequiredValue>low low</RequiredValue>
                    <RequiredValue>high high high</RequiredValue>
                    <RequiredValue>low</RequiredValue>
                    <RequiredValue>other</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""application"" ID=""{B1743A74-9F51-46bb-9883-3903BACEF378}""
            AttributeDataType=""xs:string"">
            <Description>text describing the designated use of the device</Description>
            <Version>1.0</Version>
            <RefSemantic CorrespondingAttributePath=""IRDI:0112/2///61987#ABB014#007"" />
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>emergency shutdown (ESD)</RequiredValue>
                    <RequiredValue>process shutdown (PSD)</RequiredValue>
                    <RequiredValue>fire and gas (FandG)</RequiredValue>
                    <RequiredValue>heating, ventilation, and air conditioning (HVAC)</RequiredValue>
                    <RequiredValue>emergency depressurisation (EDP) - blowdown</RequiredValue>
                    <RequiredValue>process control system (PCS)</RequiredValue>
                    <RequiredValue>high integrity pressure protection system (HIPPS)</RequiredValue>
                    <RequiredValue>pressure protection system (PPS)</RequiredValue>
                    <RequiredValue>equipment protection</RequiredValue>
                    <RequiredValue>offloading</RequiredValue>
                    <RequiredValue>monitoring</RequiredValue>
                    <RequiredValue>combined ESD and PSD</RequiredValue>
                    <RequiredValue>combined ESD, PSD and PCS</RequiredValue>
                    <RequiredValue>combined PSD and PCS</RequiredValue>
                    <RequiredValue>combined FandG and HVAC</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""deviceType_ValveTopside"" ID=""{39228FB1-9E9B-46b0-9990-05FEEAE5F14D}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>BallastValve</RequiredValue>
                    <RequiredValue>BlowdownValve</RequiredValue>
                    <RequiredValue>DelugeValve</RequiredValue>
                    <RequiredValue>FastOpeningValve_inFlare</RequiredValue>
                    <RequiredValue>FireWaterMonitorValve</RequiredValue>
                    <RequiredValue>FoamValve</RequiredValue>
                    <RequiredValue>GaseousAgentValve</RequiredValue>
                    <RequiredValue>HPU_BleedOffValve</RequiredValue>
                    <RequiredValue>OffloadingIsolationValve</RequiredValue>
                    <RequiredValue>PipelineIsolationValve</RequiredValue>
                    <RequiredValue>PressureReliefValve</RequiredValue>
                    <RequiredValue>ProcessIsolationValve</RequiredValue>
                    <RequiredValue>RiserIsolationValve</RequiredValue>
                    <RequiredValue>WaterMistValve</RequiredValue>
                    <RequiredValue>XmasTree_ChemicalInjectionValve_Topside</RequiredValue>
                    <RequiredValue>XmasTreeMasterValve_Topside</RequiredValue>
                    <RequiredValue>XmasTree_WingValve_Topside</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""energizeSource"" ID=""{CF88DB84-9D39-4dfc-B07F-7E497D26B055}""
            AttributeDataType=""xs:string"">
            <Description>form of energy that drives the actuation mechanism of the final element. It
                indicates whether the final element is operated using electrical, pneumatic, or
                hydraulic power</Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>electrical</RequiredValue>
                    <RequiredValue>pneumatic</RequiredValue>
                    <RequiredValue>hydraulic</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""externalExposure"" ID=""{4E49E2DB-EC59-4fdf-BABF-0B03A7D5D6B2}""
            AttributeDataType=""xs:string"">
            <Description>severity of external exposure (severe, moderate, or low) such as a weather
                exposed area versus a shielded area, indoor versus outdoor, etc.</Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>low</RequiredValue>
                    <RequiredValue>logic solver: outdoor cabinet</RequiredValue>
                    <RequiredValue>moderate</RequiredValue>
                    <RequiredValue>severe</RequiredValue>
                    <RequiredValue>logic solver: indoor cabinet</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""failurePhilosophy"" ID=""{8352AC7E-2810-2CC0-A7E6-DF0967780A8B}""
            AttributeDataType=""xs:string"">
            <Description>specifies whether the device shall remain in the same state upon failure</Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>fail-safe</RequiredValue>
                    <RequiredValue>fail as-is</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""fluidSeverity"" ID=""{252F043C-CBAE-4aea-AAFC-2EB539742489}""
            AttributeDataType=""xs:string"">
            <Description>description of the severity of the fluid handled by the device</Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>medium/moderate service</RequiredValue>
                    <RequiredValue>dirty/severe service</RequiredValue>
                    <RequiredValue>clean/benign service</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""resetAfterShutdown_FinalElement""
            ID=""{91AC4F6A-F734-4767-890D-755385E27876}"" AttributeDataType=""xs:string"">
            <Description>requirements for reset of a final element after shutdown, such as if it is
                required a physical reset in field </Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>reset in logic solver</RequiredValue>
                    <RequiredValue>reset in field</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""SCLevel"" ID=""{AB4A659F-54DF-1ADF-9A06-CB5E4E085C54}""
            AttributeDataType=""xs:string"">
            <Description>systematic capability required for a SIS component</Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>SC 1</RequiredValue>
                    <RequiredValue>SC 2</RequiredValue>
                    <RequiredValue>SC 3</RequiredValue>
                    <RequiredValue>SC 4</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""tripEnergyMode"" ID=""{02E17D1E-DBEF-4c28-9BCC-86B95C4A9328}""
            AttributeDataType=""xs:string"">
            <Description>whether energy needs to be supplied (energized) or cut off (de-energized)
                to trip the device</Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>de-energize to trip</RequiredValue>
                    <RequiredValue>energize to trip</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""typeAB"" ID=""{BA6FCCBD-F15D-4500-8352-AC7E28102CC0}""
            AttributeDataType=""xs:string"">
            <Description>specifies whether the device is of type A or B according to IEC 61508</Description>
            <Version>1.0</Version>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>type A</RequiredValue>
                    <RequiredValue>type B</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
    </AttributeTypeLib>
</CAEXFile>";
    }
}
