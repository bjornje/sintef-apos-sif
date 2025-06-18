using Aml.Engine.CAEX;
using Sintef.Apos.Sif.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace Sintef.Apos.Sif
{
    public static class Definition
    {
        static Definition()
        {
            ModelCAEX = CAEXDocument.LoadFromString(Model);
            Version = ModelCAEX.CAEXFile.SourceDocumentInformation.Select(x => x.OriginVersion).SingleOrDefault();

            var sifUnitClasses = ModelCAEX.CAEXFile.SystemUnitClassLib.FirstOrDefault(x => x.Name == "SIF Unit Classes");
            var sisUnitClasses = ModelCAEX.CAEXFile.SystemUnitClassLib.FirstOrDefault(x => x.Name == "SIS Unit Classes");
            var attributeTypes = ModelCAEX.CAEXFile.AttributeTypeLib.FirstOrDefault(x => x.Name == "Types");

            foreach (var item in attributeTypes)
            {
                if (!string.IsNullOrEmpty(item.Description)) _typeDescriptions.Add(item.Name, item.Description);


                if (item.Constraint.First is AttributeValueRequirementType requirementType &&
                    requirementType.NominalScaledType.ValueAttributes.Count > 1)
                {
                    _typeValues.Add(item.Name, requirementType.NominalScaledType.ValueAttributes.Select(x => x.Value.ToString()).ToArray());
                }
            }

            var expectedTypeValues = new[] { 
                "TypeAB", //SISComponent
                "SISType", //*** not in use ***
                "SILLevel", //SIF
                "SIFType", //SIF
                "ResetAfterShutdown_FinalElement", //FinalElementComponent
                "ModeOfOperation", //SIF
                "ManualActivation", //SIF
                "InputDeviceTrigger", //*** not in use ***
                "ILLevel", //SIF
                "FinalElementFunction", //*** not in use ***
                "FailSafePosition", //*** not in use ***
                "EnergizeSource", //*** not in use ***
                "EffectActivationMode", //*** not in use ***
                "E_DEToTrip", //SISComponent
                "Comparison", //SISComponent
                "CauseRole", //*** not in use ***
                "BypassControl", //SISComponent
                "AlarmOrWarning", //InputDeviceComponent
                "Application", //*** new, not in use ***
                "DetectedOrUndetected", //*** new, not in use ***
                "DetectionMethods", //*** new, not in use ***
                "DeviceType_ValveTopside", //*** new, not in use ***
                "ExternalExposure", //*** new, not in use ***
                "FailPass", //new, SISComponent
                "FailureTypes", //*** new, not in use ***
                "FluidSeverity", //*** new, not in use ***
                "SystematicOrRHF", //*** new, not in use ***
            };
            if (_typeValues.Count != expectedTypeValues.Length) throw new Exception($"Expected number of attribute types with value list {expectedTypeValues.Length} differes from actual number {_typeValues.Count}.");

            foreach (var type in expectedTypeValues) if (!_typeValues.ContainsKey(type)) throw new Exception($"Attribute type with value list not found: {type}.");

            _typeValues.Add("Boolean", new[] { "true", "false" });

            foreach (var item in sifUnitClasses)
            {
                foreach (var attr in item.Attribute)
                {
                        SetAttribute(item.Name, attr.Name, attr.Description, attr.Class?.Name);
                }
                foreach (var subItem in item.InternalElement)
                {
                    foreach (var attr in subItem.Attribute) SetAttribute(subItem.Name, attr.Name, attr.Description, attr.Class?.Name);
                    foreach (var subSubItem in subItem.InternalElement)
                    {
                        foreach (var attr in subSubItem.Attribute) SetAttribute(subSubItem.Name, attr.Name, attr.Description, attr.Class?.Name);
                    }
                }

            }

            foreach (var item in sisUnitClasses)
            {

                foreach (var attr in item.Attribute)
                {
                    if (string.IsNullOrEmpty(attr.Class?.Name))
                    {
                        Console.Write("hi");
                    }

                    SetAttribute(item.Name, attr.Name, attr.Description, attr.Class?.Name);
                }
            }


        }

        private static Dictionary<string, string> _typeDescriptions = new Dictionary<string, string>();
        private static Dictionary<string, string[]> _typeValues = new Dictionary<string, string[]>();
        public static bool TryGetAttributeTypeDescription(string name, out string description)
        {
            if (_typeDescriptions.TryGetValue(name, out description)) return true;
            return false;
        }

        public static bool TryGetAttributeTypeValues(string name, out string[] values)
        {
            if (_typeValues.TryGetValue(name, out values)) return true;
            return false;
        }

        private static void SetAttribute(string nodeName, string attributeName, string attiributeDescription, string className)
        {
            if (!_attributes.TryGetValue(nodeName, out var attributes))
            {
                attributes = new List<Model.AttributeType>();
                _attributes[nodeName] = attributes;
            }

            if (className == null)
            {
                Console.Write("Undefined attribute class: " + attributeName);
                return;
            }

            switch (className)
            {
                case "String":
                    attributes.Add(new Model.String(attributeName, attiributeDescription, null));
                    break;
                case "Frequency":
                    attributes.Add(new Frequecy(attributeName, attiributeDescription, null));
                    break;
                case "Percent":
                    attributes.Add(new Percent(attributeName, attiributeDescription, null));
                    break;
                case "Hours":
                    attributes.Add(new Hours(attributeName, attiributeDescription, null));
                    break;
                case "Seconds":
                    attributes.Add(new Seconds(attributeName, attiributeDescription, null));
                    break;
                case "Integer":
                    attributes.Add(new Integer(attributeName, attiributeDescription, null));
                    break;
                case "SILLevel":
                    attributes.Add(new SILLevel(attributeName, attiributeDescription, null));
                    break;
                case "FITs":
                    attributes.Add(new FITs(attributeName, attiributeDescription, null));
                    break;
                case "E_DEToTrip":
                    attributes.Add(new E_DEToTrip(attributeName, attiributeDescription, null));
                    break;
                case "InputDeviceTrigger":
                    attributes.Add(new InputDeviceTrigger(attributeName, attiributeDescription, null));
                    break;
                case "FinalElementFunction":
                    attributes.Add(new FinalElementFunction(attributeName, attiributeDescription, null));
                    break;
                case "FailSafePosition":
                    attributes.Add(new FailSafePosition(attributeName, attiributeDescription, null));
                    break;
                case "PerYear":
                    attributes.Add(new PerYear(attributeName, attiributeDescription, null));
                    break;
                case "SIFType":
                    attributes.Add(new SIFType(attributeName, attiributeDescription, null));
                    break;
                case "ModeOfOperation":
                    attributes.Add(new ModeOfOperation(attributeName, attiributeDescription, null));
                    break;
                case "ILLevel":
                    attributes.Add(new ILLevel(attributeName, attiributeDescription, null));
                    break;
                case "PerHour":
                    attributes.Add(new PerHour(attributeName, attiributeDescription, null));
                    break;
                case "ManualActivation":
                    attributes.Add(new ManualActivation(attributeName, attiributeDescription, null));
                    break;
                case "ResetAfterShutdown_FinalElement":
                    attributes.Add(new ResetAfterShutdown_FinalElement(attributeName, attiributeDescription, null));
                    break;
                case "AlarmOrWarning":
                    attributes.Add(new AlarmOrWarning(attributeName, attiributeDescription, null));
                    break;
                case "TagNumber":
                    attributes.Add(new TagNumber(attributeName, attiributeDescription, null));
                    break;
                case "TypeAB":
                    attributes.Add(new TypeAB(attributeName, attiributeDescription, null));
                    break;
                case "Comparison":
                    attributes.Add(new Comparison(attributeName, attiributeDescription, null));
                    break;
                case "BypassControl":
                    attributes.Add(new BypassControl(attributeName, attiributeDescription, null));
                    break;
                case "kg_s":
                    attributes.Add(new kg_s(attributeName, attiributeDescription, null));
                    break;
                case "Boolean":
                    attributes.Add(new Model.Boolean(attributeName, attiributeDescription, null));
                    break;
                case "Probability":
                    attributes.Add(new Probability(attributeName, attiributeDescription, null));
                    break;
                case "Decimal":
                    attributes.Add(new Model.Decimal(attributeName, attiributeDescription, null));
                    break;
                default:
                    Console.Write("Unknown attribute class: " + className);
                    break;
            }
        }

        public static CAEXDocument ModelCAEX { get; }
        public static string Version { get; }

        private static Dictionary<string, List<Model.AttributeType>> _attributes = new Dictionary<string, List<Model.AttributeType>>();
        public static IEnumerable<Model.AttributeType> GetAttributes(Node node)
        {
            var attributes = new List<Model.AttributeType>();

            if (node is SIFSubsystem)
            {
                if (_attributes.TryGetValue("SIFSubsystem", out var sifComponentAttributes)) attributes.AddRange(sifComponentAttributes);
            }
            else if (node is SISComponent)
            {
                if (_attributes.TryGetValue("SISComponent", out var sisComponentAttributes)) attributes.AddRange(sisComponentAttributes);
            }

            var name = node.GetType().Name;
            if (_attributes.TryGetValue(name, out var ownAttributes))
            {
                attributes.AddRange(ownAttributes);
            }

            return attributes;
        }

        public static Collection<Model.AttributeType> GetAttributes(Node target, int expectedNumberOfAttributes)
        {
            var attributes = GetAttributes(target);
            if (attributes.Count() != expectedNumberOfAttributes) throw new Exception($"Expected {expectedNumberOfAttributes} attributes but got {attributes.Count()}.");

            var type = target.GetType();

            var attributeTypes = new Collection<Model.AttributeType>();

            foreach (var attribute in attributes)
            {
                var propertyInfo = type.GetProperty(attribute.Name);
                if (propertyInfo == null) throw new Exception($"No property exists for attribute {attribute.Name}.");

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
    <Description>This document is generated from the APOS_SIF_060525.qea model version 22 by the
        EA_UML_to_AML.js transformation script.
        The model and transformation script are derived from initial SIF model and transformation
        script from Kongsberg.</Description>
    <AdditionalInformation />
    <SuperiorStandardVersion>AutomationML 2.10</SuperiorStandardVersion>
    <SourceDocumentInformation OriginName=""APOS_SIF_060525.qea"" OriginID="""" OriginVersion=""22""
        LastWritingDateTime=""2025-05-06T11:04:31.000Z"" OriginVendor=""SINTEF""
        OriginVendorURL=""www.sintef.no"" />
    <RoleClassLib Name=""CDD"">
        <Description></Description>
        <RoleClass Name=""ControlCircuitResponseTime"" ID=""{00AA35D8-2242-43ae-8596-33CAF8284AB3}"">
            <Description></Description>
            <Attribute Name=""IRDI"" RefAttributeType=""Types/String"">
                <Value>0112/2///61987#ABB593#004</Value>
            </Attribute>
            <Attribute Name=""definition"" RefAttributeType=""Types/String"">
                <Value>time that elapses between the activation of a control and the response of the
                    device or measuring assembly</Value>
            </Attribute>
        </RoleClass>
        <RoleClass Name=""FailSafePosition"" ID=""{430F8AFC-5388-4622-B3B1-D0EA490C8118}"">
            <Description></Description>
            <Attribute Name=""IRDI"" RefAttributeType=""Types/String"">
                <Value> 0112/2///61987#ABE648#002</Value>
            </Attribute>
            <Attribute Name=""definition"" RefAttributeType=""Types/String"">
                <Value>position of a valve in de-energized state</Value>
            </Attribute>
        </RoleClass>
        <RoleClass Name=""ModeOfOperation"" ID=""{CCA653D2-B72D-4f86-9B2B-1B276BDD5D74}"">
            <Description></Description>
            <Attribute Name=""IRDI"" RefAttributeType=""Types/String"">
                <Value>ABB910</Value>
            </Attribute>
            <Attribute Name=""definition"" RefAttributeType=""Types/String"">
                <Value>way in which a safety-related system is intended to be used, with respect to
                    the frequency of demands made upon it, which may be either low demand mode,
                    where the frequency of demands for operation made on a safety related, system is
                    no greater than one per year and no greater than twice the proof-test, frequency</Value>
            </Attribute>
        </RoleClass>
        <RoleClass Name=""ProofTestInterval"" ID=""{17A4F558-2B47-41c4-B5D1-A3C2E8D4D2BE}"">
            <Description></Description>
            <Attribute Name=""definition"" RefAttributeType=""Types/String"">
                <Value>interval between tests for failures which are not automatically revealed</Value>
            </Attribute>
            <Attribute Name=""IRDI"" RefAttributeType=""Types/String"">
                <Value>0112/2///61987#ABB911#005</Value>
            </Attribute>
        </RoleClass>
        <RoleClass Name=""SafetyIntegrityLevel"" ID=""{A4F4C523-7478-4928-B5A6-2E67CEFB1941}"">
            <Description></Description>
            <Attribute Name=""IRDI"" RefAttributeType=""Types/String"">
                <Value>0112/2///61987#ABB202#007</Value>
            </Attribute>
            <Attribute Name=""definition"" RefAttributeType=""Types/String"">
                <Value>discrete level (one out of a possible four), corresponding to a range of
                    safety integrity values, where safety integrity level 4 has the highest level of
                    safety integrity and safety integrity level 1 has the lowest</Value>
            </Attribute>
        </RoleClass>
        <RoleClass Name=""TagName"" ID=""{9C3C0638-F8B6-40c2-9BF0-E0B061B2C374}"">
            <Description></Description>
            <Attribute Name=""IRDI"" RefAttributeType=""Types/String"">
                <Value>0112/2///61987#ABB271#008</Value>
            </Attribute>
            <Attribute Name=""definition"" RefAttributeType=""Types/String"">
                <Value>alphanumeric character sequence uniquely identifying a measuring or control
                    point</Value>
            </Attribute>
        </RoleClass>
        <RoleClass Name=""ValueOfTripPoint"" ID=""{B92D1719-AE81-4c81-8360-A137CF22AFD2}"">
            <Description></Description>
            <Attribute Name=""IRDI"" RefAttributeType=""Types/String"">
                <Value>ABH706</Value>
            </Attribute>
            <Attribute Name=""definition"" RefAttributeType=""Types/String"">
                <Value>value of a trip point variable
                    (standardized unit of measure for the value of a trip point)</Value>
            </Attribute>
        </RoleClass>
    </RoleClassLib>
    <SystemUnitClassLib Name=""CE Unit Classes"">
        <Description></Description>
        <SystemUnitClass Name=""CESIS"" ID=""{ED0ED9D9-1395-4e4d-9AD9-40159675B208}"">
            <Description>Make vendor, client, builder more explicit</Description>
            <Attribute Name=""ApprovedBy"" ID=""{17834909-4510-4a10-8CE9-8D81ADA7B39F}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""CheckedBy"" ID=""{D61B0279-1701-4bec-AB60-E583747B181E}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FacilityName"" ID=""{DAB5A8B6-4E12-4e1d-994A-A6301972883D}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""IssuedBy"" ID=""{D6EC148A-C77F-4ca4-B25C-0982D37807FC}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Vendor"" ID=""{A7079785-058B-4313-A476-020F9E91BEEB}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <InternalElement Name=""CELevel"" ID=""{B18952CE-5580-4aaa-9036-4041F3EBE8DE}"">
                <Description></Description>
                <Attribute Name=""ApprovedBy"" ID=""{2A4B91EC-879B-4a22-A854-BE7E1FDC0439}""
                    RefAttributeType=""Types/String"">
                    <Description>Approver</Description>
                </Attribute>
                <Attribute Name=""AreaDescription"" ID=""{B4A71E56-3AB7-468f-960E-B42329A1849C}""
                    RefAttributeType=""Types/String"">
                    <Description>Description of the area</Description>
                </Attribute>
                <Attribute Name=""CheckedBy"" ID=""{82AEB5A5-5FA9-477c-A793-6949AE9C59D0}""
                    RefAttributeType=""Types/String"">
                    <Description>Checker</Description>
                </Attribute>
                <Attribute Name=""IssuedBy"" ID=""{1BC022C2-4F73-40bd-8F1F-95C16BC3E6C7}""
                    RefAttributeType=""Types/String"">
                    <Description>Issuer</Description>
                </Attribute>
                <InternalElement Name=""CauseGroup"" ID=""{B5E9D115-160C-43df-B5BC-569C96AFC264}"">
                    <Description>Solve voting between CauseGroups</Description>
                    <InternalElement Name=""InitiatorReference""
                        ID=""{F92DFC59-D31F-407f-9226-73503E3BB7FA}"">
                        <Description></Description>
                    </InternalElement>
                    <InternalElement Name=""Cause"" ID=""{C986E7A2-2C6D-44a7-832B-A6131F76FDA4}"">
                        <Description></Description>
                        <Attribute Name=""AreaDescription""
                            ID=""{B8E5BBCA-A775-4d45-9711-7E322EF08A65}""
                            RefAttributeType=""Types/String"">
                            <Description></Description>
                        </Attribute>
                        <Attribute Name=""DocumentRef"" ID=""{53917589-D98D-4b93-9177-221FAAAB7267}""
                            RefAttributeType=""Types/String"">
                            <Description></Description>
                        </Attribute>
                        <InternalElement Name=""LocalEffectReference""
                            ID=""{E307E716-A9C2-49ca-946B-04C062EBF2D1}"">
                            <Description></Description>
                            <Attribute Name=""EffectID"" ID=""{4A42B33A-449F-4fe8-805A-AE2059C5D855}""
                                RefAttributeType=""Types/Integer"">
                                <Description></Description>
                            </Attribute>
                        </InternalElement>
                        <InternalElement Name=""GlobalEffectReference""
                            ID=""{94CB94ED-ECF6-4064-AC0F-B552D100E2AB}"">
                            <Description></Description>
                            <Attribute Name=""EffectTag"" ID=""{2094D02A-5A5C-4afa-A577-D2803D176378}""
                                RefAttributeType=""Types/String"">
                                <Description></Description>
                            </Attribute>
                        </InternalElement>
                    </InternalElement>
                </InternalElement>
                <InternalElement Name=""EffectGroup"" ID=""{A36698C3-84CA-43f4-A07E-3B9142DE24CC}"">
                    <Description></Description>
                    <InternalElement Name=""Effect"" ID=""{150BC1AB-3F4D-4c15-BF17-F5F3EFBEAFB9}"">
                        <Description></Description>
                        <Attribute Name=""DocumentRef"" ID=""{1226B4FB-0DA6-46db-A01E-C7D58CFCFA4E}""
                            RefAttributeType=""Types/String"">
                            <Description></Description>
                        </Attribute>
                        <Attribute Name=""Tag"" ID=""{B9037E88-21CD-42c7-8D0D-44479C5D4CD9}""
                            RefAttributeType=""Types/String"">
                            <Description></Description>
                        </Attribute>
                        <InternalElement Name=""FinalElementReference""
                            ID=""{2147C8FA-2973-4b06-84A5-45AFFE224069}"">
                            <Description></Description>
                        </InternalElement>
                    </InternalElement>
                </InternalElement>
            </InternalElement>
        </SystemUnitClass>
    </SystemUnitClassLib>
    <SystemUnitClassLib Name=""Operation Data Classes"">
        <Description></Description>
        <SystemUnitClass Name=""ComponentInOperation"" ID=""{F915EDE1-F272-4ff9-AC36-05C412156BF7}"">
            <Description></Description>
            <Attribute Name=""OperatingTime"" ID=""{0074FA08-95A3-4e4d-8804-70CE8E4E4876}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ProofTestInterval"" ID=""{CFFCA0C4-2590-4951-A7F5-C75B89A0D9FF}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PFD"" ID=""{CD4D2518-5E6E-4788-8E4A-4F1D41CEF057}""
                RefAttributeType=""Types/Decimal"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""lambda_DU"" ID=""{BDFD46FE-7039-47ba-BFFB-025C942850DC}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""lambda_DD"" ID=""{60D6CDC9-14B5-4a98-B238-F13CC9DDDA7A}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""lambda_SU"" ID=""{BEACC22B-8B9F-436d-83F1-0FC5346CADC4}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""lambda_SD"" ID=""{A0A7937B-45C0-42a0-BE0F-0D12D6989360}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""MeanRepairTime"" ID=""{5756BDA2-8F6C-4ac5-BD82-A3271CDF6635}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""NumberOfDUfailures"" ID=""{1EE0A1B8-9CCB-4ee0-8AD0-AA4AFD699DB0}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""NumberOfTests"" ID=""{AF391C25-A9A4-49ab-BC1F-1E449DF9E9D8}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""NumberOfFailedTests"" ID=""{D9190AB6-BBDE-4b40-8497-F2BA38536288}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""EquipmentNumber"" ID=""{A453DA32-3ABF-40f4-A361-C80463EA39BF}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SerialNumber"" ID=""{C3932DDC-51F9-4aca-999D-DD40C9907F17}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Start_date"" ID=""{A3E2C239-9DC3-4778-999B-30AC8099BF2A}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""DemandEvent"" ID=""{6507F986-D7E8-4f8a-BB61-BCB11587F4F9}"">
            <Description></Description>
            <Attribute Name=""TimeStamp"" ID=""{F076F997-2F8F-458b-A261-FA2725D04599}""
                RefAttributeType=""Types/Timestamp"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""DemandDescription"" ID=""{CFCFFB85-6BBA-42fb-B981-D0002A461D65}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""FailureEvent"" ID=""{AC3605C4-1215-6BF7-AD9E-AEDA94B070B7}"">
            <Description></Description>
            <Attribute Name=""TimeStamp"" ID=""{DEECD583-FADC-4287-B763-1817436759E7}""
                RefAttributeType=""Types/Timestamp"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""DetectionMethod"" ID=""{8F2BF4EF-3B55-4151-A0D1-9AD674E67F28}""
                RefAttributeType=""Types/DetectionMethods"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""DetectedOrUndetected"" ID=""{DFEDA5F8-95AB-4c2f-875B-2FD8584E86C0}""
                RefAttributeType=""Types/DetectedOrUndetected"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailureMode"" ID=""{D4B25CDB-8B80-4590-A46E-C20748E90349}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailureType"" ID=""{7E4403AF-1D74-4643-AA80-B94752AD4419}""
                RefAttributeType=""Types/FailureTypes"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailureMechanism"" ID=""{DBAB41F9-4DFA-41af-9453-801B9844EC46}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SystematicOrRHF"" ID=""{8A6CCFA2-6327-4890-8C81-3B38F6C4DDAF}""
                RefAttributeType=""Types/SystematicOrRHF"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailureDescription"" ID=""{4F4EF3A2-FB09-4d29-889C-7B98C046F415}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailureCause"" ID=""{6FF42B9C-9AF1-4313-B147-8806C8040E5F}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ActualRepairTime"" ID=""{77D17C21-56D3-45c9-9587-263A8AF630A7}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Notification"" ID=""{16E3B88B-EC31-4d17-94A5-E111B894990C}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""WorkOrder"" ID=""{F1480AC1-8E33-453b-888D-B4CB8677A1D0}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Measure"" ID=""{76880351-7B3E-4b20-8D7E-CECA2F886C71}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ExpectedRepairTime"" ID=""{B8C089A7-2552-4247-AC48-4E07F5B347FE}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""SIFOperationData"" ID=""{BB61BCB1-1587-F4F9-9AE4-5AAD19D3BC48}"">
            <Description></Description>
            <Attribute Name=""OperationalDemandRate"" ID=""{97192F4A-C290-4340-B76E-C3C122A90587}""
                RefAttributeType=""Types/PerYear"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""NumberOfDemands"" ID=""{6568E93D-F14B-4217-8846-F1A00B56876C}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""OperatingTime"" ID=""{BD5B4E80-DDB0-4bde-BCFA-8315916D1264}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PFD"" ID=""{1A9A9C91-8F41-4881-B828-3FDAC3BE0DD6}""
                RefAttributeType=""Types/Decimal"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SIL"" ID=""{A4FEC8F5-ECC3-4988-91B6-967AE5508575}""
                RefAttributeType=""Types/ILLevel"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PFH"" ID=""{4F955A02-0946-4e6c-BB79-632868DB2625}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""TestData"" ID=""{BB61BCB1-1587-F4F9-9EEE-E9D42E89504B}"">
            <Description></Description>
            <Attribute Name=""TimeStamp"" ID=""{603D0523-9CA6-41cb-9E68-63937FF12FBD}""
                RefAttributeType=""Types/Timestamp"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TestResult_FailedOrPassed"" ID=""{7482B415-E7B4-4bd7-9B00-CE1498FA4476}""
                RefAttributeType=""Types/FailPass"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TestResult_Comment"" ID=""{279E9694-9107-4d3e-A0D7-AB2FAEEC9E29}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TypeOfTest"" ID=""{5D33F689-E108-4355-820E-486A00558E79}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
    </SystemUnitClassLib>
    <SystemUnitClassLib Name=""SIF Unit Classes"">
        <Description></Description>
        <SystemUnitClass Name=""Group"" ID=""{91B2A42C-84AD-4c59-9520-E18974D4B24B}"">
            <Description>The Group is an abstract class holding common properties for a group.</Description>
            <Attribute Name=""VoteWithinGroup_K_in_KooN"" ID=""{9F5BA9BA-7FAA-45f9-910A-F5862E1695FB}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""NumberOfComponentsOrSubgroups_N""
                ID=""{F034ACC7-4B73-42c5-A89E-4EC61DCC639A}"" RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""AllowAnyComponents"" ID=""{ABC76BF3-4F15-4456-A05A-74C0E18A1ABB}""
                RefAttributeType=""Types/Boolean"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""SIF"" ID=""{33E2F2BF-C9F7-4ac5-8B0B-6C216BD4B771}"">
            <Description>The SIF represents a Safety Instrumented System.</Description>
            <Attribute Name=""DemandRate"" ID=""{85F435A5-3D42-443b-8CF0-164DAE95CAE9}""
                RefAttributeType=""Types/PerYear"">
                <Description>Maximum allowable demand rate</Description>
            </Attribute>
            <Attribute Name=""MaxAllowableResponseTime"" ID=""{029EE774-2441-495d-9620-3C0DAF70D95C}""
                RefAttributeType=""Types/Seconds"">
                <Description>Maximum allowable response time

                    SIF Response Time is the interval between the SIF set-point being reached, and
                    the hazardous event occurring. The SIF must detect and then operate its primary
                    end elements, usually shutdown valves (SDVs) within this interval and do this
                    quickly enough to keep the process variable below the design value.</Description>
            </Attribute>
            <Attribute Name=""SafeProcessState"" ID=""{DEBD4AAE-D31F-4e6b-BD46-C3A8AEEE0BEB}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SIFID"" ID=""{6D45C9AF-E507-4f24-9174-73086558496E}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SILLevel"" ID=""{78A8B42B-6001-4f06-9CED-F5E7210A85E1}""
                RefAttributeType=""Types/SILLevel"">
                <Description>SIL – Safety Integrity Level – is a quantitative target for measuring
                    the level of performance needed for a safety function to achieve a tolerable
                    risk for a process hazard. It is defined in both . Defining a target SIL level
                    for the process should be based on the assessment of the likelihood that an
                    incident will occur and the consequences of the incident.

                    It is a discrete level (one out of four) for specifying the safety integrity
                    requirements of the safety instrumented functions to be allocated to the safety
                    instrumented systems. SIL 4 has the highest safety integrity and SIL 1 the
                    lowest.

                    IEC 61511 only defines requirements for SIL 1 to SIL 3, as it is expected that
                    SIL 3 will be a maximum level in the process sector (excepting Nuclear). For SIL
                    4, IEC 61511 refers the user back to the detail in IEC 61508.

                    IEC 61508 and IEC 61511 both have tables with the Safety Integrity Levels
                    associated with Probability of Failure, Risk Reduction Factor, Hardware Fault
                    Tolerance and Architecture. </Description>
            </Attribute>
            <Attribute Name=""SILAllocationMethod"" ID=""{91D515DA-1C3E-428e-8820-45A9DC5C7DA1}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SIFDescription"" ID=""{29F0BCBD-8483-48fa-8630-D3F32FB47D0B}""
                RefAttributeType=""Types/String"">
                <Description>Design intent</Description>
            </Attribute>
            <Attribute Name=""SIFName"" ID=""{A0E538F3-6F27-476d-8FDF-8FD31444441A}""
                RefAttributeType=""Types/String"">
                <Description>Short name</Description>
            </Attribute>
            <Attribute Name=""SIFType"" ID=""{8E9F05B7-CAEB-420e-B5EA-8E41099A501B}""
                RefAttributeType=""Types/SIFType"">
                <Description>Local or global</Description>
            </Attribute>
            <Attribute Name=""ModeOfOperation"" ID=""{DCFB6E80-4EC8-4f55-989F-2A01B6AD7087}""
                RefAttributeType=""Types/ModeOfOperation"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Cause"" ID=""{CE8E154B-674E-40a1-9BD4-9E9F4CBD4CD3}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Effect"" ID=""{4B7289A7-2C11-4737-BDDF-1CD5520016FF}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""QuantificationMethodOrTool"" ID=""{60B82A16-13F8-48e5-A993-E429C5DB87FC}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""EILLevel"" ID=""{6A963F8D-A12D-4a29-9C85-5FEF7CB75A2A}""
                RefAttributeType=""Types/ILLevel"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""AILLevel"" ID=""{40CEA4E0-D76E-45c5-8600-C08F387F130A}""
                RefAttributeType=""Types/ILLevel"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PFDRequirement"" ID=""{4BE0501A-3E0F-4c2d-93E9-9EECF215F1DB}""
                RefAttributeType=""Types/Probability"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PlantOperatingMode"" ID=""{17C19438-2B6D-4407-8700-631E7695C6AA}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PFHRequirement"" ID=""{7CBCC5DF-724B-433b-AA4B-B5893A9BE9E5}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""DemandSource"" ID=""{AB1AFDE9-3A3F-4607-B1E0-C0363EB54B2A}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SpuriousTripRate"" ID=""{CE771A94-AB81-4417-90BE-6D274B48A3E2}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ManualActivation"" ID=""{28168C89-5378-40be-841B-4058965B39F0}""
                RefAttributeType=""Types/ManualActivation"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""MeasureToAvoidCCF"" ID=""{FAFD4587-C5E2-45ac-BBC1-4B8A4CB2368B}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SurvaivabilityRequirement"" ID=""{039EE23D-5153-488d-8930-62F1FFA8D4BB}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""EUCReference"" ID=""{3590BE2B-BA33-40d1-843C-4F6D3F44CA1E}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SIFTypicalID"" ID=""{70F9AE68-B985-47db-853C-93D2D2A02A1F}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <InternalElement Name=""InputDeviceSubsystem"" ID=""{63D8A4F8-DEF5-4ead-BA95-B3F8399A1B77}""
                RefBaseSystemUnitPath=""SIF Unit Classes/SIFSubsystem"">
                <Description>The Initiator monitors some process parameter or presence of
                    a command.</Description>
            </InternalElement>
            <InternalElement Name=""LogicSolverSubsystem"" ID=""{E8D9FD92-23B6-4ed8-8E97-088523E92953}""
                RefBaseSystemUnitPath=""SIF Unit Classes/SIFSubsystem"">
                <Description>The Solver decides if it is necessary to act upon the monitored
                    signals.</Description>
            </InternalElement>
            <InternalElement Name=""FinalElementSubsystem""
                ID=""{8A647C27-BADF-4690-85BC-4FE8DE6B7178}""
                RefBaseSystemUnitPath=""SIF Unit Classes/SIFSubsystem"">
                <Description>The FinalElement carries out the
                    necessary tasks, if decided to act.</Description>
            </InternalElement>
        </SystemUnitClass>
        <SystemUnitClass Name=""SIFSubsystem"" ID=""{CF723864-9C1F-4347-98E5-1F4F1044E931}"">
            <Description>The SIFSubsystem a sub system within a SIF. This could be a part of an ESD,
                PSD, FG or HIPPS SIS.</Description>
            <Attribute Name=""PFDBudget"" ID=""{659C7645-93DB-438d-961E-A514F4E9722F}""
                RefAttributeType=""Types/Percent"">
                <Description>Probability of Dangerous Failure on Demand</Description>
            </Attribute>
            <Attribute Name=""VoteBetweenGroups_M_in_MooN""
                ID=""{67DD139F-A86B-45c5-ABD8-92801CAACD51}"" RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""NumberOfGroups_N"" ID=""{A89B3118-0E72-4a5b-AA43-83064A7CB188}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
    </SystemUnitClassLib>
    <SystemUnitClassLib Name=""SIS Operation Classes"">
        <Description></Description>
        <SystemUnitClass Name=""AImodule"" ID=""{93E11327-DD1B-D2B1-A43D-32C0A1AA9C0E}""
            RefBaseClassPath=""SIS Operation Classes/IOmodule"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""CommunicationModule"" ID=""{93E11327-DD1B-D2B1-BBED-ED92DDFE4610}""
            RefBaseClassPath=""SIS Operation Classes/OperationalLogicSolver"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""ConbustionEngine"" ID=""{97F73871-5C2E-F867-9F8E-8C342BFC5F9E}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""Damper"" ID=""{97F73871-5C2E-F867-A1FF-D924BB65EF2F}""
            RefBaseClassPath=""SIS Operation Classes/Door"">
            <Description>Fire and Gas</Description>
            <Attribute Name=""APOS_length"" ID=""{B4CC5551-6E59-4a0e-BEA9-A33114A62FD2}""
                RefAttributeType=""Types/cm"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""APOS_width"" ID=""{B40AE91E-E4F9-4e1c-9099-DEEE22735FC6}""
                RefAttributeType=""Types/cm"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""DHSV"" ID=""{97F73871-5C2E-F867-8ABA-CCEFE30D0B1C}""
            RefBaseClassPath=""SIS Operation Classes/Valve"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""DOmodule"" ID=""{93E11327-DD1B-D2B1-9C52-1F7DF4F137E7}""
            RefBaseClassPath=""SIS Operation Classes/IOmodule"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""Door"" ID=""{97F73871-5C2E-F867-93E1-1327DD1BD2B1}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""ElectricalGenerator"" ID=""{97F73871-5C2E-F867-80E8-FAA73FC83614}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""EmergencyLight"" ID=""{A1FFD924-BB65-EF2F-B841-D002853EC0FF}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""FireandGasDetector"" ID=""{93E11327-DD1B-D2B1-B813-34D9E9870B87}""
            RefBaseClassPath=""SIS Operation Classes/OperationalInputDevice"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""FWmonitor"" ID=""{A1FFD924-BB65-EF2F-9433-2310C0F1C926}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""IOmodule"" ID=""{93E11327-DD1B-D2B1-B823-96C02C1576DD}""
            RefBaseClassPath=""SIS Operation Classes/OperationalLogicSolver"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""LevelSwitch"" ID=""{93E11327-DD1B-D2B1-A0B5-00DCEEDF4CF1}""
            RefBaseClassPath=""SIS Operation Classes/ProcessSwitch"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""LevelTransmitter"" ID=""{93E11327-DD1B-D2B1-B72E-EFF5D8E0FA1E}""
            RefBaseClassPath=""SIS Operation Classes/Transmitter"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""LogicSolver"" ID=""{93E11327-DD1B-D2B1-BC79-B87086CAC23C}""
            RefBaseClassPath=""SIS Operation Classes/OperationalLogicSolver"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""Nozzle"" ID=""{97F73871-5C2E-F867-8E60-2D85800997B7}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""OperationalFinalElement"" ID=""{39D0B4B5-B158-48f1-A486-947F208B90EB}""
            RefBaseClassPath=""SIS Operation Classes/SISOperationalComponent"">
            <Description>Fire and Gas</Description>
            <Attribute Name=""EnergizeSource"" ID=""{208890BE-68E0-41f0-B519-9C13EC59DEC5}""
                RefAttributeType=""Types/EnergizeSource"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""OperationalInputDevice"" ID=""{32DF2F76-9D52-4f0d-9BE5-CEB9B17C6705}""
            RefBaseClassPath=""SIS Operation Classes/SISOperationalComponent"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""OperationalLogicSolver"" ID=""{51C9FF01-D912-4ea2-8428-EBA83F1D284E}""
            RefBaseClassPath=""SIS Operation Classes/SISOperationalComponent"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""PositionSwitch"" ID=""{93E11327-DD1B-D2B1-8B2F-5F0A6F8B83E8}""
            RefBaseClassPath=""SIS Operation Classes/ProcessSwitch"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""PressureTransmitter"" ID=""{93E11327-DD1B-D2B1-9B6E-0BC2D895DEE9}""
            RefBaseClassPath=""SIS Operation Classes/Transmitter"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""ProcessSwitch"" ID=""{93E11327-DD1B-D2B1-BCB7-6FF9808F3EE0}""
            RefBaseClassPath=""SIS Operation Classes/OperationalInputDevice"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""Pump"" ID=""{97F73871-5C2E-F867-9282-73AE5C848743}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""PushButton"" ID=""{93E11327-DD1B-D2B1-89C9-CFA9AAB125AC}""
            RefBaseClassPath=""SIS Operation Classes/OperationalInputDevice"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""SISOperationalComponent"" ID=""{4108A192-0390-4d5b-B5F2-7D50E873140F}""
            RefBaseClassPath=""SIS Unit Classes/SISComponent"">
            <Description>Fire and Gas</Description>
            <Attribute Name=""ModelNumber"" ID=""{970681E4-8BA3-4a66-B973-90BA955A13E5}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""NameOfManufacturer"" ID=""{4BC8A623-7F0B-4c8b-812B-35F2847BBAC6}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ComponentName"" ID=""{6D51BA11-32CF-4c3c-95BF-3814A25C9CC2}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ComponentDescription"" ID=""{8CBCABD5-5056-4241-9A18-D965FB1E1AEA}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ComponentBoundary"" ID=""{E451D2FB-11B3-417b-BC1A-58490C02CEA2}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""MaintainableItem"" ID=""{693FF1BA-978D-4745-B8FB-788F7122E5AB}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Systematic capability"" ID=""{9E183356-7850-4036-BCE8-BE2B4FDDE8EE}""
                RefAttributeType=""Types/SILLevel"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""InternalHFT"" ID=""{2CFBC2AB-43E1-4be8-BA00-70BEE78A798A}""
                RefAttributeType=""Types/Integer"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""MeanTimeToRestoration"" ID=""{65261B03-7B50-417f-B09A-215898847033}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""MTBF"" ID=""{2AEB4307-1FF9-49e9-84D2-8F7BFE4EF711}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""lambdaDU"" ID=""{AD39F7A2-6405-4153-A048-8D966EA61F13}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""lambdaDD"" ID=""{010BA155-5DC4-4f5d-ACAC-A5753F9BB3AE}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""lambdaSU"" ID=""{919A3C0C-5D04-4a69-9AF7-D35FA398A3BE}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""lambdaSD"" ID=""{866CEA69-817B-49bd-9EF0-D6AA4C8C79D0}""
                RefAttributeType=""Types/PerHour"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""beta"" ID=""{5DCE6F24-0901-4b3e-AA4D-D886A38DF7D3}""
                RefAttributeType=""Types/Percent"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SFF"" ID=""{FB9DE65D-D6BA-4cd5-9C30-3F8A8DDD93BA}""
                RefAttributeType=""Types/Percent"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""DC"" ID=""{05509320-7947-4571-8D15-9464F1757B62}""
                RefAttributeType=""Types/Percent"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""DataSource"" ID=""{B16B5DB4-B00B-4b08-9914-F0D5C363EC8A}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""APOS_Application"" ID=""{F5FDE52A-6717-45ba-B869-0EAD9DE0AB10}""
                RefAttributeType=""Types/Application"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""APOS_ExternalExposure"" ID=""{0EF74738-302F-4a92-8688-9B1BCD3720C5}""
                RefAttributeType=""Types/ExternalExposure"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""APOS_FluidSeverity"" ID=""{89F5A49C-E3E6-4292-BDA3-458C8259EBF4}""
                RefAttributeType=""Types/FluidSeverity"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""APOS_Fluid"" ID=""{1C339F26-79E0-4947-8557-3953FC3033E0}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PhysicalLocation"" ID=""{1AFE8778-32E0-4d3e-B4CE-74116C7181F6}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""SubseaDetector"" ID=""{93E11327-DD1B-D2B1-87EC-C12DFC34442F}""
            RefBaseClassPath=""SIS Operation Classes/OperationalInputDevice"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""SubseaSandDetector"" ID=""{93E11327-DD1B-D2B1-A125-00EFC68DE819}""
            RefBaseClassPath=""SIS Operation Classes/SubseaDetector"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""Switchgear"" ID=""{97F73871-5C2E-F867-911F-A86F40F748A0}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""Transmitter"" ID=""{93E11327-DD1B-D2B1-A405-42DB3BBDE3A7}""
            RefBaseClassPath=""SIS Operation Classes/OperationalInputDevice"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""UPS"" ID=""{A1FFD924-BB65-EF2F-982A-B6C66E123D73}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""Valve"" ID=""{93E11327-DD1B-D2B1-97DB-4F5E7EE5060E}""
            RefBaseClassPath=""SIS Operation Classes/OperationalFinalElement"">
            <Description>Fire and Gas</Description>
            <Attribute Name=""APOS_size"" ID=""{32BE9171-BCAA-4ba9-BDD8-A6F44588F66A}""
                RefAttributeType=""Types/inches"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""ValveSubsea"" ID=""{97F73871-5C2E-F867-8012-AEE3EF673C22}""
            RefBaseClassPath=""SIS Operation Classes/Valve"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""ValveTopside"" ID=""{A486947F-208B-90EB-97F7-38715C2EF867}""
            RefBaseClassPath=""SIS Operation Classes/Valve"">
            <Description>Fire and Gas</Description>
            <Attribute Name=""APOS_DeviceType"" ID=""{D662E4D0-D26A-45d9-B9AB-866CDEA44A10}""
                RefAttributeType=""Types/DeviceType_ValveTopside"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
    </SystemUnitClassLib>
    <SystemUnitClassLib Name=""SIS Unit Classes"">
        <Description></Description>
        <SystemUnitClass Name=""ESD"" ID=""{908C6D3A-3EF5-4a72-9E3C-235EF45A45BA}""
            RefBaseClassPath=""SIS Unit Classes/SIS"">
            <Description>Emergency Shutdown Safety Instrumented System</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""FG"" ID=""{FB636EC0-2CEC-47e0-9A7A-E22E64FE0F23}""
            RefBaseClassPath=""SIS Unit Classes/SIS"">
            <Description>Fire and Gas</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""FinalElementComponent"" ID=""{F3E586A2-A7B6-4d03-A990-887E933A10A3}""
            RefBaseClassPath=""SIS Unit Classes/SISComponent"">
            <Description></Description>
            <Attribute Name=""MaximumAllowableLeakageRate""
                ID=""{2301FBEE-D799-4265-9ADE-5BEC70CF354B}"" RefAttributeType=""Types/kg_s"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TightShutOff"" ID=""{A6B5AF49-3226-415f-9712-87ABDD52524C}""
                RefAttributeType=""Types/Boolean"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ResetAfterShutdown"" ID=""{04ABDB88-2FA3-458b-859C-ADC31E3E5137}""
                RefAttributeType=""Types/ResetAfterShutdown_FinalElement"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ManualOperation"" ID=""{58DAA35B-8C15-4a86-ACE3-98F52292E12E}""
                RefAttributeType=""Types/Boolean"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PartialStrokeTestInterval"" ID=""{A3C0D27B-A8AA-4d66-9A11-96B0BDA5859B}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PartialStrokeTestCoverage"" ID=""{362DCA97-B3ED-4ce3-BBBB-C1EAB3BD310A}""
                RefAttributeType=""Types/Percent"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""HIPPS"" ID=""{16A6B9E3-5548-4db6-B275-5D9807E07F09}""
            RefBaseClassPath=""SIS Unit Classes/SIS"">
            <Description>High Integrity Preassure Protection System</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""InputDeviceComponent"" ID=""{D0101401-669D-4476-8960-CB1FE6BE378E}""
            RefBaseClassPath=""SIS Unit Classes/SISComponent"">
            <Description></Description>
            <Attribute Name=""TripPointLevel"" ID=""{685D2522-9C9F-4060-AFD9-F84E36A4E4E9}""
                RefAttributeType=""Types/String"">
                <Description>Trigger criteria for this Initiator</Description>
            </Attribute>
            <Attribute Name=""UnitOfMeasure"" ID=""{BAD5EF7D-EE9D-40f0-9ECD-A9F2887BAF60}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TripPointValue"" ID=""{34C1AF7D-5E24-4492-A9DD-CA143D3C4187}""
                RefAttributeType=""Types/Decimal"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""RangeMin"" ID=""{D1E09A88-A0E8-4dfd-BF70-E1C1B1542059}""
                RefAttributeType=""Types/Decimal"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""RangeMax"" ID=""{73CCE009-7DA4-40b1-AD33-77A897C92233}""
                RefAttributeType=""Types/Decimal"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Accuracy"" ID=""{705C2451-F8CF-4cc1-A1B5-8B47035CD0E7}""
                RefAttributeType=""Types/Decimal"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""AlarmResetAfterShutdown"" ID=""{3B46276C-6321-4911-AA17-5A2D088F1B05}""
                RefAttributeType=""Types/Boolean"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""AlarmPriority"" ID=""{CC32188F-2441-4364-AE6F-8CF8613B2015}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""AlarmOrWarningText"" ID=""{2FA0F75E-4D8B-4747-8F48-3CCECDA61079}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""AlarmOrWarning"" ID=""{24284B48-0E60-411a-B09E-2CD7F6755CCF}""
                RefAttributeType=""Types/AlarmOrWarning"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""MeasurementComparison"" ID=""{32EC43E3-435F-46d4-A585-B93201B4E69C}""
                RefAttributeType=""Types/Boolean"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""LogicSolverComponent"" ID=""{9E3D868A-21B5-479a-B2D1-BB6AFAAB9383}""
            RefBaseClassPath=""SIS Unit Classes/SISComponent"">
            <Description></Description>
            <Attribute Name=""ResetAfterShutdown"" ID=""{B3285EDD-F85E-46b1-BF8A-1B8763D8A65E}""
                RefAttributeType=""Types/Boolean"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""PSD"" ID=""{43DB928B-A6AC-4e8b-B1D1-4A6044E0B735}""
            RefBaseClassPath=""SIS Unit Classes/SIS"">
            <Description>Process Shutdown</Description>
        </SystemUnitClass>
        <SystemUnitClass Name=""SIS"" ID=""{C4C0E4FC-4D62-47a1-8458-FCA48F4F945D}"">
            <Description>Safety Instrumented System abstract class that holds the common properties</Description>
            <Attribute Name=""SISID"" ID=""{85E6E820-BA06-4596-98D8-3586736B4079}""
                RefAttributeType=""Types/String"">
                <Description>Identifier of the Safety Instrumented System</Description>
            </Attribute>
        </SystemUnitClass>
        <SystemUnitClass Name=""SISComponent"" ID=""{FF6852CF-0D99-4105-9F7C-8C12781E0A0C}"">
            <Description></Description>
            <Attribute Name=""ProofTestIntervalSILCompliance""
                ID=""{75F443E8-85CF-4769-9A6F-61DDD6C8B208}"" RefAttributeType=""Types/Hours"">
                <Description>Proof testing is a routine action that is critical to ensuring the
                    integrity of a safety instrumented system (SIS) throughout its lifecycle. For
                    any SIS proof testing must be performed at a specified interval, known as the
                    proof test interval (PTI).

                    A SIS with a long proof test interval is a system that remains safe to operate
                    over a longer period of time, or in other words, the integrity of the safety
                    system degrades more slowly over time. This is a very important consideration
                    for owners and operators of machinery because it means that their
                    operational/production processes can continue to operate for longer without
                    being interrupted for proof testing.""</Description>
            </Attribute>
            <Attribute Name=""TagName"" ID=""{907808CA-CE26-4d4a-8055-3E9B66B6D7C6}""
                RefAttributeType=""Types/String"">
                <Description>Unique identifier of this SISComponent</Description>
            </Attribute>
            <Attribute Name=""PFDBudget"" ID=""{5948DF31-E6BF-433b-92F9-F04BF89BD48D}""
                RefAttributeType=""Types/Percent"">
                <Description>PFD contribution of an individual element to the SIF.</Description>
            </Attribute>
            <Attribute Name=""TagNumber"" ID=""{BF330542-95C1-45ff-891A-C325EF1E24FD}""
                RefAttributeType=""Types/TagNumber"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TagDescription"" ID=""{179BFC6E-EA57-4b2b-A82D-848425B076B9}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SafeState"" ID=""{B17188F0-9532-4e38-A9BB-D0D5BBF3D735}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TypeAB"" ID=""{78567E3D-737F-48c6-AF69-600EF2910A94}""
                RefAttributeType=""Types/TypeAB"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""APOSL2Group"" ID=""{888C8FD7-934A-403a-8592-3E5B92266376}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""PFHBudget"" ID=""{9498669D-53B7-469f-ACAF-ABE4B9A3AC42}""
                RefAttributeType=""Types/Percent"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SystematicCapability"" ID=""{19A99F65-098C-43c3-A130-3A7498E94EA5}""
                RefAttributeType=""Types/SILLevel"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ProofTestCoverage"" ID=""{FDE4FDDE-6355-4acb-A53F-E12CF32993FD}""
                RefAttributeType=""Types/Percent"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""MTTR"" ID=""{EC241F8B-A8A0-474f-8222-FCFDAC723872}""
                RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""ProofTestIntervalOperatorSpec""
                ID=""{7A6CC76B-D147-4459-9259-3A08824195B6}"" RefAttributeType=""Types/Hours"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""MaxAllowableResponseTime"" ID=""{E082AFFE-62D6-47ea-AD37-985ED15A3233}""
                RefAttributeType=""Types/Seconds"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailPass_Requirement"" ID=""{5E740012-D12C-43ae-A9BA-E6854C8A6A34}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailPass_Unit"" ID=""{EDD79564-6174-4c90-99B3-559C2E11A49C}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailPass_ComparisonToPass"" ID=""{C26F54AE-337D-4bb5-9D21-5D6BD12F0814}""
                RefAttributeType=""Types/Comparison"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""FailPass_Masurement"" ID=""{B30678A9-F14D-4c5b-A2B1-E10C333DB210}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Bypass_Approvement"" ID=""{1DAD57C3-4F48-4f41-8C13-EA4F82FE7E46}""
                RefAttributeType=""Types/Boolean"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Bypass_MaxAllowableBypassTime""
                ID=""{B5E92439-7FFD-4964-91DB-D4E5875E01DF}"" RefAttributeType=""Types/Seconds"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""Bypass_Control"" ID=""{D7CC04FB-37D0-480a-954A-FEF76FBD3A8A}""
                RefAttributeType=""Types/BypassControl"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TimeDelay"" ID=""{382D9BDA-D26E-422d-8297-5DEF5FA26AC1}""
                RefAttributeType=""Types/Seconds"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""E_DEToTrip"" ID=""{3FC7CB67-CDA0-47d0-9E39-9B2F610B0EA9}""
                RefAttributeType=""Types/E_DEToTrip"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""TripAction"" ID=""{536B57B1-4DA5-46eb-BD91-CAAC7E26567B}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
            <Attribute Name=""SurvaibabilityRequirement"" ID=""{0D4DE559-767E-4293-B5BB-224D2D1BE40B}""
                RefAttributeType=""Types/String"">
                <Description>e.g. required time to remain operational in fire</Description>
            </Attribute>
            <Attribute Name=""DiagnosticRequirementsForImplementation""
                ID=""{75D259C0-55B7-46cd-9D42-9A26C734D1DF}"" RefAttributeType=""Types/String"">
                <Description>(for implementation)</Description>
            </Attribute>
            <Attribute Name=""DiagnosticRequired"" ID=""{16B8DDD2-AF20-46cf-929A-3263D72D2572}""
                RefAttributeType=""Types/String"">
                <Description>e.g. comparison alarm, line monitoring, etc.</Description>
            </Attribute>
            <Attribute Name=""EnvironmentalExtremes"" ID=""{C5919075-1F2B-4878-B789-49D05707266E}""
                RefAttributeType=""Types/String"">
                <Description></Description>
            </Attribute>
        </SystemUnitClass>
    </SystemUnitClassLib>
    <SystemUnitClassLib Name=""Specification"">
        <Description></Description>
        <SystemUnitClass Name=""FunctionalRequirements"" ID=""{8467702F-4F6F-4490-B3EE-4B461261BD87}"">
            <Description></Description>
            <Attribute Name=""Required MTBF"" ID=""{7D9288E6-424C-4b7c-8850-DA0704FD92F1}""
                RefAttributeType=""Types/Hours"">
                <Description>Mean Time Before Failure</Description>
            </Attribute>
            <Attribute Name=""Required PST"" ID=""{02AA37D8-9A8D-4939-A8F4-42F44D977936}""
                RefAttributeType=""Types/Hours"">
                <Description>Partial Stroke Test Interval</Description>
            </Attribute>
            <Attribute Name=""Required MaxAllowableResponseTime""
                ID=""{460BA3ED-616B-4cf2-B07B-329737E999EA}"" RefAttributeType=""Types/Seconds"">
                <Description>SIF Response Time is the interval between the SIF set-point being
                    reached, and the hazardous event occurring. The SIF must detect and then operate
                    its primary end elements, usually shutdown valves (SDVs) within this interval
                    and do this quickly enough to keep the process variable below the design value.</Description>
            </Attribute>
            <Attribute Name=""Required ProofTestInterval"" ID=""{E30EE2FB-93A3-4493-965F-63CBB4691A09}""
                RefAttributeType=""Types/Seconds"">
                <Description>Manual Proof Test Interval</Description>
            </Attribute>
            <Attribute Name=""Required SIL"" ID=""{957D2F68-41EF-4393-89D3-273AD4CE4143}""
                RefAttributeType=""Types/ILLevel"">
                <Description>SIL – Safety Integrity Level – is a quantitative target for measuring
                    the level of performance needed for a safety function to achieve a tolerable
                    risk for a process hazard. It is defined in both . Defining a target SIL level
                    for the process should be based on the assessment of the likelihood that an
                    incident will occur and the consequences of the incident.

                    It is a discrete level (one out of four) for specifying the safety integrity
                    requirements of the safety instrumented functions to be allocated to the safety
                    instrumented systems. SIL 4 has the highest safety integrity and SIL 1 the
                    lowest.

                    IEC 61511 only defines requirements for SIL 1 to SIL 3, as it is expected that
                    SIL 3 will be a maximum level in the process sector (excepting Nuclear). For SIL
                    4, IEC 61511 refers the user back to the detail in IEC 61508.

                    IEC 61508 and IEC 61511 both have tables with the Safety Integrity Levels
                    associated with Probability of Failure, Risk Reduction Factor, Hardware Fault
                    Tolerance and Architecture. </Description>
            </Attribute>
        </SystemUnitClass>
    </SystemUnitClassLib>
    <AttributeTypeLib Name=""Types"">
        <Description></Description>
        <AttributeType Name=""Boolean"" ID=""{E51A2A9B-C0D5-4abf-8DBC-70B496ECDC47}""
            AttributeDataType=""xs:boolean"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""cm"" ID=""{8DD8E408-B7D8-423E-8A61-83BD64D4CF70}""
            AttributeDataType=""xs:decimal"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""Decimal"" ID=""{B2DA91BF-710E-4ee5-9DB7-DF70CD001239}""
            AttributeDataType=""xs:decimal"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""FITs"" ID=""{C8850987-83A2-4c23-8527-09BD42D1AC6A}""
            AttributeDataType=""xs:decimal"">
            <Description>Failure In Time or Failure UnIT (λ) are failures per billion hours for a
                piece of equipment, expressed by 10-9 hours.</Description>
        </AttributeType>
        <AttributeType Name=""Frequency"" ID=""{9B4B2F2E-D56B-4377-B591-D7EEDCA0745E}""
            AttributeDataType=""xs:decimal"">
            <Description>Percent as a decimal number</Description>
        </AttributeType>
        <AttributeType Name=""Hours"" ID=""{3B360754-DE57-4e7c-BC69-EF2DCB603B16}""
            AttributeDataType=""xs:duration"">
            <Description>Hours as a decimal number</Description>
        </AttributeType>
        <AttributeType Name=""inches"" ID=""{8DD8E408-B7D8-423E-A68D-9A56CD215463}""
            AttributeDataType=""xs:decimal"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""Integer"" ID=""{A4662FA1-C30C-4dbb-8003-70950AEFC83C}""
            AttributeDataType=""xs:intenger"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""kg_s"" ID=""{A2546FE1-4ACA-47a5-A1AA-F913C06620CC}""
            AttributeDataType=""xs:decimal"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""MaxAllowableResponseTime"" ID=""{6F26A217-9D0C-4689-A6AA-3EBD629B3C57}""
            AttributeDataType=""xs:decimal"">
            <Description></Description>
            <RefSemantic CorrespondingAttributePath=""CDD/ControlCircuitResponseTime"" />
        </AttributeType>
        <AttributeType Name=""Percent"" ID=""{1E32BB38-B1A9-4276-BED7-BECC41E448FA}""
            AttributeDataType=""xs:decimal"">
            <Description>Percent as a decimal number</Description>
        </AttributeType>
        <AttributeType Name=""PerHour"" ID=""{680F155E-B352-423c-8DD8-E408B7D8423E}""
            AttributeDataType=""xs:decimal"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""PerYear"" ID=""{50D4F3B5-5ADD-41e7-BB29-4E7991FBA063}""
            AttributeDataType=""xs:decimal"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""Probability"" ID=""{BCC8F9DA-3CB4-F09A-AC4D-FF76977D9B72}""
            AttributeDataType=""xs:decimal"">
            <Description>Seconds as a decimal number</Description>
        </AttributeType>
        <AttributeType Name=""ProofTestInterval"" ID=""{E9D5A738-1D4D-48d8-A035-50AC4078C815}""
            AttributeDataType=""xs:decimal"">
            <Description></Description>
            <RefSemantic CorrespondingAttributePath=""CDD/ProofTestInterval"" />
        </AttributeType>
        <AttributeType Name=""Seconds"" ID=""{1B18EB3E-1E0D-47e5-BCC8-F9DA3CB4F09A}""
            AttributeDataType=""xs:decimal"">
            <Description>Seconds as a decimal number</Description>
        </AttributeType>
        <AttributeType Name=""String"" ID=""{A5BB6E10-32EC-4581-A89D-2ED558FFB8CB}""
            AttributeDataType=""xs:string"">
            <Description></Description>
        </AttributeType>
        <AttributeType Name=""TagNumber"" ID=""{1CCBE575-AEE3-40f0-991D-9E81E3D7A751}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <RefSemantic CorrespondingAttributePath=""CDD/TagName"" />
        </AttributeType>
        <AttributeType Name=""Timestamp"" ID=""{BC69EF2D-CB60-3B16-BB2B-A4D61B5B0BC0}""
            AttributeDataType=""xs:dateTime"">
            <Description>Hours as a decimal number</Description>
        </AttributeType>
        <AttributeType Name=""AlarmOrWarning"" ID=""{B0BDD8DE-DB79-4d55-AD2F-5DB980D60DC3}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>alarm</RequiredValue>
                    <RequiredValue>warning</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""Application"" ID=""{B1743A74-9F51-46bb-9883-3903BACEF378}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>ESD</RequiredValue>
                    <RequiredValue>PSD</RequiredValue>
                    <RequiredValue>ESDandPSD</RequiredValue>
                    <RequiredValue>FireAndGas</RequiredValue>
                    <RequiredValue>HVAC</RequiredValue>
                    <RequiredValue>EDP</RequiredValue>
                    <RequiredValue>BPCS</RequiredValue>
                    <RequiredValue>HIPPS</RequiredValue>
                    <RequiredValue>PPS</RequiredValue>
                    <RequiredValue>EquipmentProtection</RequiredValue>
                    <RequiredValue>Offloading</RequiredValue>
                    <RequiredValue>Monitoring</RequiredValue>
                    <RequiredValue>Combined</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""BypassControl"" ID=""{76DBD4CC-19E4-4a11-A4A4-C0E89A539F4E}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>approve</RequiredValue>
                    <RequiredValue>set</RequiredValue>
                    <RequiredValue>remove</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""CauseRole"" ID=""{519F254E-537A-4224-8A36-5C45E5088343}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>Voted 1ooN</RequiredValue>
                    <RequiredValue>Voted 2ooN</RequiredValue>
                    <RequiredValue>Voted 3ooN</RequiredValue>
                    <RequiredValue>Low</RequiredValue>
                    <RequiredValue>LowLow</RequiredValue>
                    <RequiredValue>High</RequiredValue>
                    <RequiredValue>HighHigh</RequiredValue>
                    <RequiredValue>DigitalHigh</RequiredValue>
                    <RequiredValue>DigitalLow</RequiredValue>
                    <RequiredValue>IO Fault 1ooN</RequiredValue>
                    <RequiredValue>IO Fault 2ooN</RequiredValue>
                    <RequiredValue>IO Fault NooN</RequiredValue>
                    <RequiredValue>IO Fault + Alarm</RequiredValue>
                    <RequiredValue>No Capability</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""Comparison"" ID=""{838053F1-DE1D-44ec-8925-9F868A53C72B}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>greater than</RequiredValue>
                    <RequiredValue>less than or equal</RequiredValue>
                    <RequiredValue>less than</RequiredValue>
                    <RequiredValue>greater than or equal</RequiredValue>
                    <RequiredValue>yes</RequiredValue>
                    <RequiredValue>no</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""DetectedOrUndetected"" ID=""{AD2F5DB9-80D6-0DC3-A43E-955EC5FE668C}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>undetected</RequiredValue>
                    <RequiredValue>detected</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""DetectionMethods"" ID=""{AD2F5DB9-80D6-0DC3-98B7-C2B34D5F065F}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>OtherPMactivity</RequiredValue>
                    <RequiredValue>ProofTest</RequiredValue>
                    <RequiredValue>Demand</RequiredValue>
                    <RequiredValue>CasualObservation</RequiredValue>
                    <RequiredValue>DiagnosticAlarm</RequiredValue>
                    <RequiredValue>DetectedUponOccurrence_NotAlarmed</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""DeviceType_ValveTopside"" ID=""{39228FB1-9E9B-46b0-9990-05FEEAE5F14D}""
            AttributeDataType=""xs:string"">
            <Description></Description>
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
        <AttributeType Name=""E_DEToTrip"" ID=""{02E17D1E-DBEF-4c28-9BCC-86B95C4A9328}""
            AttributeDataType=""xs:string"">
            <Description>States that can be applied to a device</Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>DE</RequiredValue>
                    <RequiredValue>E</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""EffectActivationMode"" ID=""{EF371AB3-0CFB-4de0-A68E-CE5EC70610FD}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>Automatic activation</RequiredValue>
                    <RequiredValue>Automatic activation. Reset allowed</RequiredValue>
                    <RequiredValue>Activation if not acknowledged</RequiredValue>
                    <RequiredValue>Activation force no delay</RequiredValue>
                    <RequiredValue>Suggested</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""EnergizeSource"" ID=""{CF88DB84-9D39-4dfc-B07F-7E497D26B055}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>electrical</RequiredValue>
                    <RequiredValue>pneumatic</RequiredValue>
                    <RequiredValue>hydraulic</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""ExternalExposure"" ID=""{4E49E2DB-EC59-4fdf-BABF-0B03A7D5D6B2}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>shielded</RequiredValue>
                    <RequiredValue>indoor</RequiredValue>
                    <RequiredValue>moderate</RequiredValue>
                    <RequiredValue>severe</RequiredValue>
                    <RequiredValue>outdoor</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""FailPass"" ID=""{AD2F5DB9-80D6-0DC3-AF5A-71335FA2D331}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>passed</RequiredValue>
                    <RequiredValue>failed</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""FailSafePosition"" ID=""{1D90A95A-6066-433e-B370-A3A429DCF9F8}""
            AttributeDataType=""xs:string"">
            <Description>States that can be applied to a device</Description>
            <RefSemantic CorrespondingAttributePath=""CDD/FailSafePosition"" />
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>NDE</RequiredValue>
                    <RequiredValue>NE</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""FailureTypes"" ID=""{AD2F5DB9-80D6-0DC3-91DC-46B8C3E9D1D1}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>DD</RequiredValue>
                    <RequiredValue>DU</RequiredValue>
                    <RequiredValue>Spurious</RequiredValue>
                    <RequiredValue>SU</RequiredValue>
                    <RequiredValue>SD</RequiredValue>
                    <RequiredValue>NONC</RequiredValue>
                    <RequiredValue>NA</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""FinalElementFunction"" ID=""{EB429728-AA30-45b1-B0C5-5AF1058912FD}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>Start</RequiredValue>
                    <RequiredValue>Stop</RequiredValue>
                    <RequiredValue>Open</RequiredValue>
                    <RequiredValue>Close</RequiredValue>
                    <RequiredValue>Activate</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""FluidSeverity"" ID=""{252F043C-CBAE-4aea-AAFC-2EB539742489}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>medium</RequiredValue>
                    <RequiredValue>dirty</RequiredValue>
                    <RequiredValue>clean</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""ILLevel"" ID=""{5E890B18-BC15-4c28-BFCC-9057FDA9E807}""
            AttributeDataType=""xs:string"">
            <Description>SIL – Safety Integrity Level – is a quantitative target for measuring the
                level of performance needed for a safety function to achieve a tolerable risk for a
                process hazard. It is defined in both . Defining a target SIL level for the process
                should be based on the assessment of the likelihood that an incident will occur and
                the consequences of the incident.

                It is a discrete level (one out of four) for specifying the safety integrity
                requirements of the safety instrumented functions to be allocated to the safety
                instrumented systems. SIL 4 has the highest safety integrity and SIL 1 the lowest.
                IEC 61511 only defines requirements for SIL 1 to SIL 3, as it is expected that SIL 3
                will be a maximum level in the process sector (excepting Nuclear). For SIL 4, IEC
                61511 refers the user back to the detail in IEC 61508.

                IEC 61508 and IEC 61511 both have tables with the Safety Integrity Levels associated
                with Probability of Failure, Risk Reduction Factor, Hardware Fault Tolerance and
                Architecture. </Description>
            <RefSemantic CorrespondingAttributePath=""CDD/SafetyIntegrityLevel"" />
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>IL1</RequiredValue>
                    <RequiredValue>IL2</RequiredValue>
                    <RequiredValue>IL3</RequiredValue>
                    <RequiredValue>IL4</RequiredValue>
                    <RequiredValue>no IL</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""InputDeviceTrigger"" ID=""{F6EE3886-84AA-48c6-B5C6-9FE733FAA597}""
            AttributeDataType=""xs:string"">
            <Description>Events that trigger the initiator</Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>AnalogHighHigh</RequiredValue>
                    <RequiredValue>AnalogHigh</RequiredValue>
                    <RequiredValue>AnalogLow</RequiredValue>
                    <RequiredValue>AnalogLowLow</RequiredValue>
                    <RequiredValue>DigitalHigh</RequiredValue>
                    <RequiredValue>DigitalLow</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""ManualActivation"" ID=""{2EA660A1-C6A6-4fb1-818D-369F311CDBAD}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>local activation</RequiredValue>
                    <RequiredValue>GUI_faceplate</RequiredValue>
                    <RequiredValue>CAP</RequiredValue>
                    <RequiredValue>none</RequiredValue>
                    <RequiredValue>other</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""ModeOfOperation"" ID=""{6F7C3CDE-30BC-4644-8C34-30BEE5823BA2}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>low demand</RequiredValue>
                    <RequiredValue>high demand</RequiredValue>
                    <RequiredValue>continuous demand</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""ResetAfterShutdown_FinalElement""
            ID=""{91AC4F6A-F734-4767-890D-755385E27876}"" AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>reset in logic</RequiredValue>
                    <RequiredValue>reset in field</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""SIFType"" ID=""{FD2354A7-D96F-49fc-9006-741440576B96}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>Global</RequiredValue>
                    <RequiredValue>Local</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""SILLevel"" ID=""{0FF0E7E0-9EA4-4700-AB4A-659F54DF1ADF}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>SIL 1</RequiredValue>
                    <RequiredValue>SIL 2</RequiredValue>
                    <RequiredValue>SIL 3</RequiredValue>
                    <RequiredValue>SIL 4</RequiredValue>
                    <RequiredValue>no SIL</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""SISType"" ID=""{FDDC5919-FDA7-4285-A98C-52D1E408D11D}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>ESD</RequiredValue>
                    <RequiredValue>PSD</RequiredValue>
                    <RequiredValue>HIPPS</RequiredValue>
                    <RequiredValue>FG</RequiredValue>
                    <RequiredValue>Other</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""SystematicOrRHF"" ID=""{AD2F5DB9-80D6-0DC3-91FA-E7967265FC30}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>random hardware failure</RequiredValue>
                    <RequiredValue>systematic failure</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
        <AttributeType Name=""TypeAB"" ID=""{BA6FCCBD-F15D-4500-8352-AC7E28102CC0}""
            AttributeDataType=""xs:string"">
            <Description></Description>
            <Constraint Name=""Enumeration"">
                <NominalScaledType>
                    <RequiredValue>A</RequiredValue>
                    <RequiredValue>B</RequiredValue>
                </NominalScaledType>
            </Constraint>
        </AttributeType>
    </AttributeTypeLib>
</CAEXFile>";
    }
}
