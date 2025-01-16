using Aml.Engine.Adapter;
using Aml.Engine.CAEX;
using Sintef.Apos.Sif.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sintef.Apos.Sif
{
    public static class Definition
    {
        static Definition()
        {
            try
            {
                ModelCAEX = CAEXDocument.LoadFromString(Model);
                Version = ModelCAEX.CAEXFile.SourceDocumentInformation.Select(x => x.OriginVersion).SingleOrDefault();

                var sifUnitClasses = ModelCAEX.CAEXFile.SystemUnitClassLib.FirstOrDefault(x => x.Name == "SIF Unit Classes");
                var sisUnitClasses = ModelCAEX.CAEXFile.SystemUnitClassLib.FirstOrDefault(x => x.Name == "SIS Unit Classes");
                var attributeTypes = ModelCAEX.CAEXFile.AttributeTypeLib.FirstOrDefault(x => x.Name == "Types");

                foreach (var item in attributeTypes)
                {
                    if (!string.IsNullOrEmpty(item.Description)) _typeDescriptions.Add(item.Name, item.Description);

                    foreach (var element in item.Constraint.Elements)
                    {
                        var values = element.Value.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
                        
                        _typeValues.Add(item.Name, values);
                    }
                }

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
                    foreach (var attr in item.Attribute) SetAttribute(item.Name, attr.Name, attr.Description, attr.Class?.Name);
                }

            }
            catch
            {
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
                    attributes.Add(new Model.String(attributeName, attiributeDescription));
                    break;
                case "Frequency":
                    attributes.Add(new Frequecy(attributeName, attiributeDescription));
                    break;
                case "Percent":
                    attributes.Add(new Percent(attributeName, attiributeDescription));
                    break;
                case "Hours":
                    attributes.Add(new Hours(attributeName, attiributeDescription));
                    break;
                case "Seconds":
                    attributes.Add(new Seconds(attributeName, attiributeDescription));
                    break;
                case "Integer":
                    attributes.Add(new Integer(attributeName, attiributeDescription));
                    break;
                case "SILLevel":
                    attributes.Add(new SILLevel(attributeName, attiributeDescription));
                    break;
                case "FITs":
                    attributes.Add(new FITs(attributeName, attiributeDescription));
                    break;
                case "E_DEToTrip":
                    attributes.Add(new E_DEToTrip(attributeName, attiributeDescription));
                    break;
                case "InputDeviceTrigger":
                    attributes.Add(new InputDeviceTrigger(attributeName, attiributeDescription));
                    break;
                case "FinalElementFunction":
                    attributes.Add(new FinalElementFunction(attributeName, attiributeDescription));
                    break;
                case "FailSafePosition":
                    attributes.Add(new FailSafePosition(attributeName, attiributeDescription));
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

            if (node is SIFComponent)
            {
                if (_attributes.TryGetValue("SIFComponent", out var sifComponentAttributes)) attributes.AddRange(sifComponentAttributes);
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

        public static string Model = @"<?xml version=""1.0"" encoding=""utf-8""?>
<CAEXFile SchemaVersion=""3.0"" FileName="""" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns=""http://www.dke.de/CAEX"" xsi:schemaLocation=""http://www.dke.de/CAEX CAEX_ClassModel_V.3.0.xsd"">
<Description>This document is generated from the AAS SIF Submodel [trunk] #18 [magicdraw02:3579] on 2024-11-11 18:11:35 using the UML2AML template.</Description>
<AdditionalInformation />
<SuperiorStandardVersion>AutomationML 2.10</SuperiorStandardVersion>
<SourceDocumentInformation OriginName=""AAS SIF Submodel [trunk] #18 [magicdraw02:3579] "" OriginID="""" OriginVersion=""18"" LastWritingDateTime=""2024-11-11T18:11:35"" OriginVendor=""KONGSBERG"" OriginVendorURL=""www.kongsberg.com"" />
<RoleClassLib Name=""CDD"">
<Description></Description>
<RoleClass Name=""ProofTestInterval"" ID=""b86e6bf0-6310-4095-a4bb-71c197e718e5"" >
<Description></Description>
<Attribute Name=""definition"" ID=""41875dbb-53a7-4d20-856c-cf887b89f94c"" RefAttributeType=""Types/String"">
<Value>interval between tests for failures which are not automatically revealed</Value> 
</Attribute>
<Attribute Name=""IRDI"" ID=""243ea485-d769-4415-a583-e6d419ee7e82"" RefAttributeType=""Types/String"">
<Value>0112/2///61987#ABB911#005</Value> 
</Attribute>
</RoleClass>
<RoleClass Name=""TagName"" ID=""726acb14-8ca5-41af-b20c-d18fb92a6d3c"" >
<Description></Description>
<Attribute Name=""IRDI"" ID=""243ea485-d769-4415-a583-e6d419ee7e82"" RefAttributeType=""Types/String"">
<Value>0112/2///61987#ABB271#008</Value> 
</Attribute>
<Attribute Name=""definition"" ID=""41875dbb-53a7-4d20-856c-cf887b89f94c"" RefAttributeType=""Types/String"">
<Value>alphanumeric character sequence uniquely identifying a measuring or control point</Value> 
</Attribute>
</RoleClass>
<RoleClass Name=""FailSafePosition"" ID=""174431a9-844e-4ee7-af84-9015dfb120aa"" >
<Description></Description>
<Attribute Name=""IRDI"" ID=""243ea485-d769-4415-a583-e6d419ee7e82"" RefAttributeType=""Types/String"">
<Value>	0112/2///61987#ABE648#002</Value> 
</Attribute>
<Attribute Name=""definition"" ID=""41875dbb-53a7-4d20-856c-cf887b89f94c"" RefAttributeType=""Types/String"">
<Value>position of a valve in de-energized state</Value> 
</Attribute>
</RoleClass>
<RoleClass Name=""SafetyIntegrityLevel"" ID=""b5cd1cc7-f7b8-48d0-9362-d2bda7f1fb35"" >
<Description></Description>
<Attribute Name=""IRDI"" ID=""243ea485-d769-4415-a583-e6d419ee7e82"" RefAttributeType=""Types/String"">
<Value>0112/2///61987#ABB202#007</Value> 
</Attribute>
<Attribute Name=""definition"" ID=""41875dbb-53a7-4d20-856c-cf887b89f94c"" RefAttributeType=""Types/String"">
<Value>discrete level (one out of a possible four), corresponding to a range of safety integrity values, where safety integrity level 4 has the highest level of safety integrity and safety integrity level 1 has the lowest</Value> 
</Attribute>
</RoleClass>
<RoleClass Name=""ValueOfTripPoint"" ID=""db57b7b8-e2de-466f-9ed1-4c8670192049"" >
<Description></Description>
<Attribute Name=""IRDI"" ID=""243ea485-d769-4415-a583-e6d419ee7e82"" RefAttributeType=""Types/String"">
<Value>ABH706</Value> 
</Attribute>
<Attribute Name=""definition"" ID=""41875dbb-53a7-4d20-856c-cf887b89f94c"" RefAttributeType=""Types/String"">
<Value>value of a trip point variable
(standardized unit of measure for the value of a trip point)</Value> 
</Attribute>
</RoleClass>
<RoleClass Name=""ModeOfOperation"" ID=""edb1d555-1c41-4fe9-9bba-9d7db0218614"" >
<Description></Description>
<Attribute Name=""IRDI"" ID=""243ea485-d769-4415-a583-e6d419ee7e82"" RefAttributeType=""Types/String"">
<Value>ABB910</Value> 
</Attribute>
<Attribute Name=""definition"" ID=""41875dbb-53a7-4d20-856c-cf887b89f94c"" RefAttributeType=""Types/String"">
<Value>way in which a safety-related system is intended to be used, with respect to the frequency of demands made upon it, which may be either low demand mode, where the frequency of demands for operation made on a safety related, system is no greater than one per year and no greater than twice the proof-test, frequency; or high demand or continuous mode, where the frequency of demands for operation made, on a safety-related system is greater than one per year or greater than twice the proof check frequency</Value> 
</Attribute>
</RoleClass>
<RoleClass Name=""ControlCircuitResponseTime"" ID=""0bc47736-e3d4-4137-8f83-f258dd38f75e"" >
<Description></Description>
<Attribute Name=""IRDI"" ID=""243ea485-d769-4415-a583-e6d419ee7e82"" RefAttributeType=""Types/String"">
<Value>0112/2///61987#ABB593#004</Value> 
</Attribute>
<Attribute Name=""definition"" ID=""41875dbb-53a7-4d20-856c-cf887b89f94c"" RefAttributeType=""Types/String"">
<Value>time that elapses between the activation of a control and the response of the device or measuring assembly</Value> 
</Attribute>
</RoleClass>
</RoleClassLib>
<SystemUnitClassLib Name=""SIF Unit Classes"">
<Description></Description>
<SystemUnitClass Name=""SIF"" ID=""7638158c-6cd7-45bb-9ebb-6ee7f584689f"" >
<Description>The SIF represents a Safety Instrumented System.</Description>
<Attribute Name=""SIFID"" ID=""de959eff-5da0-4096-8393-0f1b5d45771a"" RefAttributeType=""Types/String"">
<Description></Description>
</Attribute>
<Attribute Name=""DemandRate"" ID=""8dd5ecd0-1fd4-4ffd-a6d8-834c18e7a3b9"" RefAttributeType=""Types/Frequency"">
<Description></Description>
</Attribute>
<Attribute Name=""E_DEToTrip"" ID=""796c22db-bc26-46c0-800b-8021e703b3cd"" RefAttributeType=""Types/E_DEToTrip"">
<Description>SIL – Safety Integrity Level – is a quantitative target for measuring the level of performance needed for a safety function to achieve a tolerable risk for a process hazard. It is defined in both . Defining a target SIL level for the process should be based on the assessment of the likelihood that an incident will occur and the consequences of the incident. 

It is a discrete level (one out of four) for specifying the safety integrity requirements of the safety instrumented functions to be allocated to the safety instrumented systems. SIL 4 has the highest safety integrity and SIL 1 the lowest.

IEC 61511 only defines requirements for SIL 1 to SIL 3, as it is expected that SIL 3 will be a maximum level in the process sector (excepting Nuclear). For SIL 4, IEC 61511 refers the user back to the detail in IEC 61508.

IEC 61508 and IEC 61511 both have tables with the Safety Integrity Levels associated with Probability of Failure, Risk Reduction Factor, Hardware Fault Tolerance and Architecture. </Description>
</Attribute>
<Attribute Name=""SafeState"" ID=""a73172ec-894e-48b6-b1ee-032db8921e92"" RefAttributeType=""Types/String"">
<Description></Description>
</Attribute>
<Attribute Name=""SILAllocationMethod"" ID=""d1d9677b-9b1c-4ea1-b75e-09a5ea9ef84c"" RefAttributeType=""Types/String"">
<Description></Description>
</Attribute>
<InternalElement Name=""SIFSubsystem"" ID=""cc4ea85d-4109-4154-99a0-bcf006442e46"" >
<Description>The SIFSubsystem a sub system within a SIF. This could be a part of an ESD, PSD, FG or HIPPS SIS.</Description>
<Attribute Name=""PFDBudget"" ID=""eb026e59-c779-469b-bc2f-0ca2e8eaa206"" RefAttributeType=""Types/Percent"">
<Description>Probability of Dangerous Failure on Demand</Description>
</Attribute>
<Attribute Name=""ProcessSafetyTime"" ID=""769efdd4-e2f1-4146-8dfd-b7ead9bed208"" RefAttributeType=""Types/Hours"">
<Description>Partial Stroke Test Interval</Description>
</Attribute>
<Attribute Name=""MaxAllowableResponseTime"" ID=""8edd4311-65d9-466a-8986-954b81ccc803"" RefAttributeType=""Types/Seconds"">
<Description>SIF Response Time is the interval between the SIF set-point being reached, and the hazardous event occurring. The SIF must detect and then operate its primary end elements, usually shutdown valves (SDVs) within this interval and do this quickly enough to keep the process variable below the design value.</Description>
</Attribute>
<Attribute Name=""ProofTestInterval"" ID=""b2270eec-a6c3-42c1-8c50-0b9b91e0423e"" RefAttributeType=""Types/Hours"">
<Description>Manual Proof Test Interval</Description>
</Attribute>
<Attribute Name=""SIL"" ID=""d1721788-c044-44d3-88ec-812f76a429a7"" RefAttributeType=""Types/SILLevel"">
<Description>SIL – Safety Integrity Level – is a quantitative target for measuring the level of performance needed for a safety function to achieve a tolerable risk for a process hazard. It is defined in both . Defining a target SIL level for the process should be based on the assessment of the likelihood that an incident will occur and the consequences of the incident. 

It is a discrete level (one out of four) for specifying the safety integrity requirements of the safety instrumented functions to be allocated to the safety instrumented systems. SIL 4 has the highest safety integrity and SIL 1 the lowest.

IEC 61511 only defines requirements for SIL 1 to SIL 3, as it is expected that SIL 3 will be a maximum level in the process sector (excepting Nuclear). For SIL 4, IEC 61511 refers the user back to the detail in IEC 61508.

IEC 61508 and IEC 61511 both have tables with the Safety Integrity Levels associated with Probability of Failure, Risk Reduction Factor, Hardware Fault Tolerance and Architecture. </Description>
</Attribute>
<InternalElement Name=""InputDevice"" ID=""2c353186-d7c8-4162-abbb-56a487ad12e5""  RefBaseSystemUnitPath=""SIF Unit Classes/SIFComponent"" >
<Description>The Initiator monitors some process parameter or presence of
a command.</Description>
<InternalElement Name=""InputDeviceGroup"" ID=""3c6953e2-cfdf-4146-ab5d-60d4da487119""  RefBaseSystemUnitPath=""SIF Unit Classes/Group"" >
<Description>The InitiatorGroup groups initiators.</Description>
</InternalElement>
</InternalElement>
<InternalElement Name=""LogicSolver"" ID=""342ee984-ec76-4752-9d2f-b20bae66d903""  RefBaseSystemUnitPath=""SIF Unit Classes/SIFComponent"" >
<Description>The Solver decides if it is necessary to act upon the
monitored signals.
</Description>
<InternalElement Name=""LogicSolverGroup"" ID=""f12c2a2b-bb30-4d27-9a81-c7febad92f53""  RefBaseSystemUnitPath=""SIF Unit Classes/Group"" >
<Description>The SolverGroup groups solvers.</Description>
</InternalElement>
</InternalElement>
<InternalElement Name=""FinalElement"" ID=""1b937414-429d-4656-94ec-c2da4c14e1f5""  RefBaseSystemUnitPath=""SIF Unit Classes/SIFComponent"" >
<Description>The FinalElement carries out the
necessary tasks, if decided to act.</Description>
<Attribute Name=""FailSafePosition"" ID=""66277064-cf9f-400c-9ca4-4ba8ddf5530f"" RefAttributeType=""Types/FailSafePosition"">
<Description>The safe state of a controlled device is the device state that contributes to reach the SIF's safe state. The actual device state that is deemed safe depends on the device and it's responsibility within the SIF.   </Description>
</Attribute>
<InternalElement Name=""FinalElementGroup"" ID=""ae8d7c7e-86eb-4df6-84b1-b0e04b62a00a""  RefBaseSystemUnitPath=""SIF Unit Classes/Group"" >
<Description>The FinalGroup groups final elements.</Description>
</InternalElement>
</InternalElement>
</InternalElement>
</SystemUnitClass>
<SystemUnitClass Name=""SIFComponent"" ID=""2194945a-21e8-4090-896b-f25ced345cbf"" >
<Description>The SIFComponent  is an abstract class holding common properties. </Description>
<Attribute Name=""DC"" ID=""dfeba74f-be14-42fe-9014-89a8fb80e090"" RefAttributeType=""Types/Percent"">
<Description>Diagnostic coverage is a measure of effectiveness of the diagnostics implemented in the system.
DC = (Σ λDD)/(Σ λDD + Σ λDU)</Description>
</Attribute>
<Attribute Name=""LoopTypical"" ID=""89f7e9f2-dad5-4a1b-9e1f-8510a45a5b7a"" RefAttributeType=""Types/String"">
<Description>A  loop typical is a reference to the connection diagram used to implement the SIF loop.</Description>
</Attribute>
<Attribute Name=""UsefulLifetime"" ID=""d15933a6-5a8e-4493-a1d7-734f8527d3de"" RefAttributeType=""Types/Hours"">
<Description>Useful Life Time is the period of time after early life failures (i.e. burn-in, infant mortality) and before end-of-life failures (i.e. wear-out) during which the failure rate can be assumed to be relatively constant under certain conditions.</Description>
</Attribute>
<Attribute Name=""MTTR"" ID=""cac803b8-6e73-4f19-a49d-9b1c9c5ff301"" RefAttributeType=""Types/Hours"">
<Description>Mean Time To Repair</Description>
</Attribute>
<Attribute Name=""PFDBudget"" ID=""24b40c43-0cdc-426e-86f5-614f87a4a1fb"" RefAttributeType=""Types/Percent"">
<Description>PFD contribution of an individual element to the SIF.</Description>
</Attribute>
<Attribute Name=""PTC"" ID=""bafb5d3b-9c40-40d4-9e45-02460b59282e"" RefAttributeType=""Types/Percent"">
<Description>Proof Test Coverage (PTC) is the term given to the percentage of dangerous undetected failures that are exposed by a defined proof test procedure.</Description>
</Attribute>
<Attribute Name=""ProofTestInterval"" ID=""289bfe8d-4400-497b-bc57-1909486a51af"" RefAttributeType=""Types/Hours"">
<Description>Proof testing is a routine action that is critical to ensuring the integrity of a safety instrumented system (SIS) throughout its lifecycle. For any SIS proof testing must be performed at a specified interval, known as the proof test interval (PTI).

A SIS with a long proof test interval is a system that remains safe to operate over a longer period of time, or in other words, the integrity of the safety system degrades more slowly over time. This is a very important consideration for owners and operators of machinery because it means that their operational/production processes can continue to operate for longer without being interrupted for proof testing.""</Description>
</Attribute>
<Attribute Name=""ResponseTime"" ID=""2970271e-2997-4c0f-b091-ddb7b3eb8926"" RefAttributeType=""Types/Seconds"">
<Description>Response Time of a single SIF element (i.e. Initiator, Solver or FinalElement). This response time contributes to the overall SRT.</Description>
</Attribute>
<Attribute Name=""SFF"" ID=""7ef7cc60-1775-45ec-8b63-e08423f235b3"" RefAttributeType=""Types/Percent"">
<Description>Safe Failure Fraction (SFF) is defined as the ratio of the average rate of safe failures plus dangerous detected failures of the subsystem to the total average failure rate of the subsystem. It is defined for a single channel (no redundancy, 1oo1). It is a measurement of the likelihood of getting a dangerous failure that is NOT detected by automatic self diagnostics, shown in the folliowing equation:  SFF = (λSD + λSU + λDD) / (λSD + λSU + λDD+ λDU)</Description>
</Attribute>
<Attribute Name=""SIL"" ID=""940657d0-ca5d-4b08-8177-d4da66929580"" RefAttributeType=""Types/SILLevel"">
<Description>SIL – Safety Integrity Level – is a quantitative target for measuring the level of performance needed for a safety function to achieve a tolerable risk for a process hazard. It is defined in both . Defining a target SIL level for the process should be based on the assessment of the likelihood that an incident will occur and the consequences of the incident. 

It is a discrete level (one out of four) for specifying the safety integrity requirements of the safety instrumented functions to be allocated to the safety instrumented systems. SIL 4 has the highest safety integrity and SIL 1 the lowest.

IEC 61511 only defines requirements for SIL 1 to SIL 3, as it is expected that SIL 3 will be a maximum level in the process sector (excepting Nuclear). For SIL 4, IEC 61511 refers the user back to the detail in IEC 61508.

IEC 61508 and IEC 61511 both have tables with the Safety Integrity Levels associated with Probability of Failure, Risk Reduction Factor, Hardware Fault Tolerance and Architecture. </Description>
</Attribute>
<Attribute Name=""β"" ID=""8f944253-d615-42ea-ad6a-52a3ed2944aa"" RefAttributeType=""Types/Percent"">
<Description>Factor Beta quantifies possible common cause failures in redundant architectures such as 1oo2 or 2oo3.</Description>
</Attribute>
<Attribute Name=""λDD"" ID=""b35a5360-bbb4-4604-8ca2-cd2217f111ae"" RefAttributeType=""Types/FITs"">
<Description>Detectable dangerous failure rate in functional safety expressed in the unit of measurement of FITs which can be determined through FMEDA.</Description>
</Attribute>
<Attribute Name=""λDU"" ID=""9ad01db9-6dc7-4291-905a-d7cf5fa15d85"" RefAttributeType=""Types/FITs"">
<Description>Undetectable dangerous failure rate in functional safety expressed in the unit of measurement of FITs which can be determined through FMEDA.</Description>
</Attribute>
<Attribute Name=""λS"" ID=""286e1ce4-c801-40a2-9b8d-845578c5f711"" RefAttributeType=""Types/FITs"">
<Description>The number of safe of spurious failures per unit time for a piece of equipment. A spurious trip or safe failure would be a time when the process is in normal operation and the system acts as if there is a problem and goes to the safe state when it is not necessary.

λS = λSD + λSU</Description>
</Attribute>
<Attribute Name=""MRT"" ID=""455e0a3a-e570-4c30-9d22-e09a22c45243"" RefAttributeType=""Types/Hours"">
<Description></Description>
</Attribute>
<Attribute Name=""MaxAllowableResponseTime"" ID=""106ac893-ac42-47c5-aaec-6654e79f89b0"" RefAttributeType=""Types/Seconds"">
<Description></Description>
</Attribute>
<InternalElement Name=""GroupVoter"" ID=""17675e52-eb95-4d8a-b60b-2a8de40fcffc"" >
<Description>The GroupVoter votes between groups of Initiators, Solvers or Final Elements.</Description>
<Attribute Name=""M"" ID=""4806ab6d-7d9f-4c4a-ace5-7e2d379d5370"" RefAttributeType=""Types/Integer"">
<Description>MooN Groups</Description>
</Attribute>
</InternalElement>
</SystemUnitClass>
<SystemUnitClass Name=""Group"" ID=""81b2ed4b-ae97-4a32-b3f1-e2b677323a1a"" >
<Description>The Group is an abstract class holding common properties for a group.</Description>
<InternalElement Name=""ComponentVoter"" ID=""e14df429-c17d-4dab-aa84-73bc5224a6b3"" >
<Description>The ComponentVoter votes between components within a group.</Description>
<Attribute Name=""K"" ID=""7f98a2e5-d894-486f-910d-93e9277ec9d8"" RefAttributeType=""Types/Integer"">
<Description>MooN Components</Description>
</Attribute>
</InternalElement>
</SystemUnitClass>
</SystemUnitClassLib>
<SystemUnitClassLib Name=""SIS Unit Classes"">
<Description></Description>
<SystemUnitClass Name=""SISComponent"" ID=""30addbba-c8b2-49e7-bcba-162b4d8ccbe7"" >
<Description></Description>
<Attribute Name=""ProofTestInterval"" ID=""e35b518c-cbe8-4afe-9865-ab213b04d592"" RefAttributeType=""Types/Hours"">
<Description>Proof testing is a routine action that is critical to ensuring the integrity of a safety instrumented system (SIS) throughout its lifecycle. For any SIS proof testing must be performed at a specified interval, known as the proof test interval (PTI).

A SIS with a long proof test interval is a system that remains safe to operate over a longer period of time, or in other words, the integrity of the safety system degrades more slowly over time. This is a very important consideration for owners and operators of machinery because it means that their operational/production processes can continue to operate for longer without being interrupted for proof testing.""</Description>
</Attribute>
<Attribute Name=""SIL"" ID=""a4ae3489-1d21-45cb-af8e-1d27af85d8e3"" RefAttributeType=""Types/SILLevel"">
<Description>SIL – Safety Integrity Level – is a quantitative target for measuring the level of performance needed for a safety function to achieve a tolerable risk for a process hazard. It is defined in both . Defining a target SIL level for the process should be based on the assessment of the likelihood that an incident will occur and the consequences of the incident. 

It is a discrete level (one out of four) for specifying the safety integrity requirements of the safety instrumented functions to be allocated to the safety instrumented systems. SIL 4 has the highest safety integrity and SIL 1 the lowest.

IEC 61511 only defines requirements for SIL 1 to SIL 3, as it is expected that SIL 3 will be a maximum level in the process sector (excepting Nuclear). For SIL 4, IEC 61511 refers the user back to the detail in IEC 61508.

IEC 61508 and IEC 61511 both have tables with the Safety Integrity Levels associated with Probability of Failure, Risk Reduction Factor, Hardware Fault Tolerance and Architecture. </Description>
</Attribute>
</SystemUnitClass>
<SystemUnitClass Name=""SIS"" ID=""8da2b6d0-6660-4952-9f9b-3087dafd1568"" >
<Description>Safety Instrumented System abstract class that holds the common properties</Description>
<Attribute Name=""SISID"" ID=""2b905400-2540-48e8-bc4e-95b9d08d0ed5"" RefAttributeType=""Types/String"">
<Description>Identifier of the Safety Instrumented System</Description>
</Attribute>
</SystemUnitClass>
<SystemUnitClass Name=""ESD"" ID=""9014acf2-a437-4ffa-91b2-826bb5610ac3""  RefBaseClassPath=""SIS Unit Classes/SIS"" >
<Description>Emergency Shutdown Safety Instrumented System</Description>
</SystemUnitClass>
<SystemUnitClass Name=""PSD"" ID=""634cb918-ad57-45b1-937f-17e7f154c699""  RefBaseClassPath=""SIS Unit Classes/SIS"" >
<Description>Process Shutdown</Description>
</SystemUnitClass>
<SystemUnitClass Name=""FG"" ID=""534508a3-d97b-414a-b737-11d5d030dc72""  RefBaseClassPath=""SIS Unit Classes/SIS"" >
<Description>Fire and Gas</Description>
</SystemUnitClass>
<SystemUnitClass Name=""HIPPS"" ID=""174c3555-8282-48a4-85c5-e0b625c98216""  RefBaseClassPath=""SIS Unit Classes/SIS"" >
<Description>High Integrity Preassure Protection System</Description>
</SystemUnitClass>
<SystemUnitClass Name=""InitiatorComponent"" ID=""40c542cc-d475-4fdf-ac0c-d05f586a903d""  RefBaseClassPath=""SIS Unit Classes/SISComponent"" >
<Description></Description>
<Attribute Name=""Criteria"" ID=""13dc3bb1-cb2a-4f74-aafd-85d2db7711d9"" RefAttributeType=""Types/InputDeviceTrigger"">
<Description>Trigger criteria for this Initiator</Description>
<Constraint Name=""Criteria constraints"">
<OrdinalScaledType>
<RequiredMaxValue>-1</RequiredMaxValue> 
<RequiredMinValue>1</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""LoopTypical"" ID=""b5b22d90-25e3-42d9-bc37-f68102669e57"" RefAttributeType=""Types/String"">
<Description>A  loop typical is a reference to the connection diagram used to implement the SIF loop.</Description>
</Attribute>
</SystemUnitClass>
<SystemUnitClass Name=""LogicSolverComponent"" ID=""c14acf45-e4e9-414d-b71b-6338b052212c""  RefBaseClassPath=""SIS Unit Classes/SISComponent"" >
<Description></Description>
</SystemUnitClass>
<SystemUnitClass Name=""FinalElementComponent"" ID=""eb671eb0-e0de-4f8b-badf-bcf477484ca8""  RefBaseClassPath=""SIS Unit Classes/SISComponent"" >
<Description></Description>
<Attribute Name=""Function"" ID=""1c22cdfa-dc63-4c16-b14a-328cba7c115a"" RefAttributeType=""Types/FinalElementFunction"">
<Description>The function performed by this Final Element</Description>
</Attribute>
<Attribute Name=""LoopTypical"" ID=""78286292-f66b-4f5c-8525-4971517ce8fc"" RefAttributeType=""Types/String"">
<Description>A  loop typical is a reference to the connection diagram used to implement the SIF loop.</Description>
</Attribute>
<Attribute Name=""FailSafePosition"" ID=""a5a642d0-ffb1-4814-88a0-8173ad9b8517"" RefAttributeType=""Types/FailSafePosition"">
<Description>The safe state of a controlled device is the device state that contributes to reach the SIF's safe state. The actual device state that is deemed safe depends on the device and it's responsibility within the SIF.   </Description>
</Attribute>
</SystemUnitClass>
</SystemUnitClassLib>
<SystemUnitClassLib Name=""CE Unit Classes"">
<Description></Description>
<SystemUnitClass Name=""CESIS"" ID=""5a542e0e-44e4-405f-aced-2a7ce95ad760"" >
<Description></Description>
<Attribute Name=""ApprovedBy"" ID=""7a586804-3e1c-4c64-825b-5c94d8c7c52f"" RefAttributeType=""Types/String"">
<Description></Description>
</Attribute>
<Attribute Name=""Builder"" ID=""41918c23-0cd0-4b94-8f88-a5526d881752"" RefAttributeType=""Types/String"">
<Description>ProjectBuilder</Description>
<Constraint Name=""Builder constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""CheckedBy"" ID=""b6508723-2efe-4bf6-955b-40bf9940fad8"" RefAttributeType=""Types/String"">
<Description></Description>
</Attribute>
<Attribute Name=""Client"" ID=""4831345f-5f5b-4c3a-888b-a1b4d267d7b8"" RefAttributeType=""Types/String"">
<Description>ProjectClient</Description>
<Constraint Name=""Client constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""Description"" ID=""a8d5b667-d8a2-4010-ac12-28b675746479"" RefAttributeType=""Types/String"">
<Description>AppIDDesc</Description>
<Constraint Name=""Description constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""EngineeringCompany"" ID=""2eaee180-1278-4d28-9402-09ef38677706"" RefAttributeType=""Types/String"">
<Description>ProjectEngComp</Description>
<Constraint Name=""EngineeringCompany constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""FacilityName"" ID=""6a54ebf1-9a6e-4bfa-996a-018bc7d9ddb0"" RefAttributeType=""Types/String"">
<Description></Description>
<Constraint Name=""FacilityName constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""HullNumber"" ID=""7a011c63-f564-4eff-a482-13ba018dc65f"" RefAttributeType=""Types/String"">
<Description>ProjectHullNr</Description>
<Constraint Name=""HullNumber constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""IssuedBy"" ID=""4266297f-cf7b-46ef-87f4-c00e6d90cc8d"" RefAttributeType=""Types/String"">
<Description></Description>
</Attribute>
<Attribute Name=""ProjectDescription"" ID=""4edef152-2f78-4138-972c-f27b293a940a"" RefAttributeType=""Types/String"">
<Description>ProjectDesc</Description>
<Constraint Name=""ProjectDescription constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""ProjectName"" ID=""1eb2bd6a-8306-46c3-b790-d1913cfd0e93"" RefAttributeType=""Types/String"">
<Description>ProjectName</Description>
</Attribute>
<Attribute Name=""Type"" ID=""2daa3968-0bfe-4e8e-b330-a33f06ea1c0c"" RefAttributeType=""Types/SISType"">
<Description>AppID</Description>
</Attribute>
<Attribute Name=""Vendor"" ID=""68a5de0d-60b3-4828-9c39-42b4d644c19e"" RefAttributeType=""Types/String"">
<Description></Description>
<Constraint Name=""Vendor constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<InternalElement Name=""CELevel"" ID=""125f246c-bee0-4dfe-948a-4648ca8f3ff9"" >
<Description></Description>
<Attribute Name=""ApprovedBy"" ID=""52093c8e-aa01-44f5-bd45-353fc11eadc5"" RefAttributeType=""Types/String"">
<Description>Approver</Description>
</Attribute>
<Attribute Name=""AreaDescription"" ID=""01653b03-12c3-44c3-95de-63fd3bf37735"" RefAttributeType=""Types/String"">
<Description>Description of the area</Description>
<Constraint Name=""AreaDescription constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""CheckedBy"" ID=""03b9b898-c3b0-4ed5-90c8-b41983289762"" RefAttributeType=""Types/String"">
<Description>Checker</Description>
</Attribute>
<Attribute Name=""Description"" ID=""ccb48060-4409-481a-9105-c635898f6d78"" RefAttributeType=""Types/String"">
<Description>LevelDesc</Description>
</Attribute>
<Attribute Name=""ID"" ID=""73c62fe2-087b-4614-9123-08ef87762e69"" RefAttributeType=""Types/String"">
<Description></Description>
</Attribute>
<Attribute Name=""IssuedBy"" ID=""47ea2ae0-e5c6-4777-888f-423c37307ace"" RefAttributeType=""Types/String"">
<Description>Issuer</Description>
</Attribute>
<Attribute Name=""Note"" ID=""7d299379-953f-45fc-8eaa-6e41f2238583"" RefAttributeType=""Types/String"">
<Description>Note</Description>
<Constraint Name=""Note constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<InternalElement Name=""CauseGroup"" ID=""191e8ade-ce9e-4582-a181-ff7fb40ffed2"" >
<Description></Description>
<Attribute Name=""Description"" ID=""8b268bdb-02e2-42c8-993f-f0c0e78f0434"" RefAttributeType=""Types/String"">
<Description>TypicalDesc</Description>
<Constraint Name=""Description constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""LineNumber"" ID=""a369cf2b-18e8-42ad-af40-d0ce93a5effc"" RefAttributeType=""Types/Integer"">
<Description></Description>
</Attribute>
<InternalElement Name=""InitiatorReference"" ID=""19464b46-ae8b-4c53-8313-720db7cfa1d7"" >
<Description></Description>
<Attribute Name=""InitiatorTag"" ID=""5ecbe3f1-e987-45cd-8c63-efa5c9b31030"" RefAttributeType=""Types/String"">
<Description>Tags</Description>
</Attribute>
<Attribute Name=""Trigger"" ID=""0156683e-8203-4333-a414-0b3f70e90c74"" RefAttributeType=""Types/InputDeviceTrigger"">
<Description>FuncCode</Description>
</Attribute>
</InternalElement>
<InternalElement Name=""Cause"" ID=""aefecca5-91f6-4e37-9613-de65febd3950"" >
<Description></Description>
<Attribute Name=""AreaDescription"" ID=""1da66830-cfb4-49cd-b7f1-f5c2f35ac4cb"" RefAttributeType=""Types/String"">
<Description></Description>
<Constraint Name=""AreaDescription constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""Delay"" ID=""fd402761-785e-41fd-872e-4a48e5dc050c"" RefAttributeType=""Types/Seconds"">
<Description>CauseTimeDel</Description>
<Constraint Name=""Delay constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""Description"" ID=""895888a2-2ec0-4dac-84c4-eb285dada422"" RefAttributeType=""Types/String"">
<Description>CauseDesc</Description>
</Attribute>
<Attribute Name=""DocumentRef"" ID=""1fdabe90-e06f-46fe-852e-78a81742b743"" RefAttributeType=""Types/String"">
<Description></Description>
<Constraint Name=""DocumentRef constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""ID"" ID=""1b55ba82-eb86-4f85-8a15-b9587c69e05b"" RefAttributeType=""Types/Integer"">
<Description>CauseNr</Description>
</Attribute>
<Attribute Name=""LineNumber"" ID=""84134d45-c7c9-4402-9031-c5634eb7f8f5"" RefAttributeType=""Types/Integer"">
<Description>CauseLineNr</Description>
</Attribute>
<Attribute Name=""Note"" ID=""ab6ee1a8-c067-4ce2-adcf-1c5c24d46498"" RefAttributeType=""Types/String"">
<Description></Description>
<Constraint Name=""Note constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""Role"" ID=""f7599429-3a11-483a-96a5-843cfebd5ed3"" RefAttributeType=""Types/CauseRole"">
<Description></Description>
</Attribute>
<InternalElement Name=""LocalEffectReference"" ID=""4385cf77-f038-4d42-a8b3-36e5fc9f0929"" >
<Description></Description>
<Attribute Name=""ActivationMode"" ID=""17db15b1-4a0f-46f6-bd8a-cfe1b7e7eaba"" RefAttributeType=""Types/EffectActivationMode"">
<Description></Description>
</Attribute>
<Attribute Name=""EffectID"" ID=""776dc377-bd02-477c-9f1c-ef05470d51bd"" RefAttributeType=""Types/Integer"">
<Description></Description>
</Attribute>
</InternalElement>
<InternalElement Name=""GlobalEffectReference"" ID=""65f45632-8589-45d4-9dac-b99175518786"" >
<Description></Description>
<Attribute Name=""EffectTag"" ID=""3e6c752e-d5d9-43f5-ab18-d5d30aa51ea1"" RefAttributeType=""Types/String"">
<Description></Description>
</Attribute>
</InternalElement>
</InternalElement>
</InternalElement>
<InternalElement Name=""EffectGroup"" ID=""196cebfb-c1fa-4ecb-81a2-29f047078ced"" >
<Description></Description>
<Attribute Name=""Description"" ID=""5257bb3b-687c-425b-a7d9-c5c811eea041"" RefAttributeType=""Types/String"">
<Description>EffectTypDesc</Description>
<Constraint Name=""Description constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""LineNumber"" ID=""bea7dec5-d3c3-4a30-8b0a-0a03e2b32752"" RefAttributeType=""Types/Integer"">
<Description>EffectTypLine</Description>
</Attribute>
<InternalElement Name=""Effect"" ID=""29cdd5b2-b309-4d9d-bc73-de01d0d82902"" >
<Description></Description>
<Attribute Name=""Description"" ID=""48dec9eb-3c6c-488f-8658-7f225b5c7ebb"" RefAttributeType=""Types/String"">
<Description>EffectDesc</Description>
</Attribute>
<Attribute Name=""DocumentRef"" ID=""e27fca5b-cb4b-40ce-950d-a67bac88156c"" RefAttributeType=""Types/String"">
<Description></Description>
<Constraint Name=""DocumentRef constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""ID"" ID=""89972577-7a78-4b84-9c56-76e324bc81bf"" RefAttributeType=""Types/Integer"">
<Description>EffectNr</Description>
</Attribute>
<Attribute Name=""LineNumber"" ID=""c9d7633f-432d-40a9-b6fb-56a0ec2b9738"" RefAttributeType=""Types/Integer"">
<Description>EffectLineNr</Description>
</Attribute>
<Attribute Name=""Note"" ID=""787260c0-ca68-4e91-a37b-8ad7148bc4eb"" RefAttributeType=""Types/String"">
<Description></Description>
<Constraint Name=""Note constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<Attribute Name=""Tag"" ID=""0208aaf5-85fa-4990-b929-868c9fcc588d"" RefAttributeType=""Types/String"">
<Description></Description>
<Constraint Name=""Tag constraints"">
<OrdinalScaledType>
<RequiredMaxValue>1</RequiredMaxValue> 
<RequiredMinValue>0</RequiredMinValue> 
</OrdinalScaledType>
</Constraint>
</Attribute>
<InternalElement Name=""FinalElementReference"" ID=""e1739a9f-8a69-4478-b6af-0266e49e0290"" >
<Description></Description>
<Attribute Name=""FinalElementTag"" ID=""038dcff1-1d8b-4665-abac-7728666abd59"" RefAttributeType=""Types/String"">
<Description>Tags</Description>
</Attribute>
<Attribute Name=""Function"" ID=""c3b974b8-26f7-4477-88ff-714be18ab8e8"" RefAttributeType=""Types/FinalElementFunction"">
<Description>FuncCode</Description>
</Attribute>
</InternalElement>
</InternalElement>
</InternalElement>
</InternalElement>
</SystemUnitClass>
</SystemUnitClassLib>
<AttributeTypeLib Name=""Types"">
<Description></Description>
<AttributeType Name=""SILLevel"" ID=""318f8c3b-9c2e-49ca-8dce-f84cff571253"" AttributeDataType=""xs:string"">
<Description>SIL – Safety Integrity Level – is a quantitative target for measuring the level of performance needed for a safety function to achieve a tolerable risk for a process hazard. It is defined in both . Defining a target SIL level for the process should be based on the assessment of the likelihood that an incident will occur and the consequences of the incident. 

It is a discrete level (one out of four) for specifying the safety integrity requirements of the safety instrumented functions to be allocated to the safety instrumented systems. SIL 4 has the highest safety integrity and SIL 1 the lowest.

IEC 61511 only defines requirements for SIL 1 to SIL 3, as it is expected that SIL 3 will be a maximum level in the process sector (excepting Nuclear). For SIL 4, IEC 61511 refers the user back to the detail in IEC 61508.

IEC 61508 and IEC 61511 both have tables with the Safety Integrity Levels associated with Probability of Failure, Risk Reduction Factor, Hardware Fault Tolerance and Architecture. </Description>
<RefSemantic CorrespondingAttributePath=""CDD/SafetyIntegrityLevel"" />
<Constraint Name=""Enumeration"">
<NominalScaledType>
<RequiredValue>SIL1</RequiredValue>
<RequiredValue>SIL2</RequiredValue>
<RequiredValue>SIL3</RequiredValue>
<RequiredValue>SIL4</RequiredValue>
</NominalScaledType>
</Constraint>
</AttributeType>
<AttributeType Name=""Hours"" ID=""d50cc174-51c7-45df-8e1b-7c223fe8f572""  AttributeDataType=""xs:duration"" >
<Description>Hours as a decimal number</Description>
</AttributeType>
<AttributeType Name=""Integer"" ID=""33dea04c-7ffa-456e-942a-abe165083b87""  AttributeDataType=""xs:intenger"" >
<Description></Description>
</AttributeType>
<AttributeType Name=""SISType"" ID=""5cb16b6c-c197-4b0d-b0bf-25c3677589e9"" AttributeDataType=""xs:string"">
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
<AttributeType Name=""FITs"" ID=""29ab4f46-7a63-4083-a8cb-de39034c5669""  AttributeDataType=""xs:decimal"" >
<Description>Failure In Time or Failure UnIT (λ) are failures per billion hours for a piece of equipment, expressed by 10-9 hours.</Description>
</AttributeType>
<AttributeType Name=""String"" ID=""8c164521-a0c7-4e5f-a8d1-735ee262e554""  AttributeDataType=""xs:string"" >
<Description></Description>
</AttributeType>
<AttributeType Name=""EffectActivationMode"" ID=""a8988cb4-d877-425f-ba01-dc102c19e9fd"" AttributeDataType=""xs:string"">
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
<AttributeType Name=""Percent"" ID=""0f142297-4f05-424b-9f5b-927293d172a3""  AttributeDataType=""xs:decimal"" >
<Description>Percent as a decimal number</Description>
</AttributeType>
<AttributeType Name=""InputDeviceTrigger"" ID=""300b041f-653b-4386-b46a-02c1bff04d65"" AttributeDataType=""xs:string"">
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
<AttributeType Name=""TagName"" ID=""08f0612a-4f1b-4d50-9394-4d743e39c308""  AttributeDataType=""xs:string"" >
<Description></Description>
<RefSemantic CorrespondingAttributePath=""CDD/TagName"" />
</AttributeType>
<AttributeType Name=""Seconds"" ID=""0eab77a1-612d-445d-b9aa-a44226531d8d""  AttributeDataType=""xs:decimal"" >
<Description>Seconds as a decimal number</Description>
</AttributeType>
<AttributeType Name=""FinalElementFunction"" ID=""8037a974-16b8-41b8-9457-451b195cd8f0"" AttributeDataType=""xs:string"">
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
<AttributeType Name=""ProofTestInterval"" ID=""b1f3417e-999f-46e2-bb5c-84bfd156ac7d""  AttributeDataType=""xs:decimal"" >
<Description></Description>
<RefSemantic CorrespondingAttributePath=""CDD/ProofTestInterval"" />
</AttributeType>
<AttributeType Name=""CauseRole"" ID=""db6e6965-a032-4f9b-aa09-55306f92d3a3"" AttributeDataType=""xs:string"">
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
<AttributeType Name=""FailSafePosition"" ID=""cfb06118-9cc1-47f7-b4bd-ab5823ebf399"" AttributeDataType=""xs:string"">
<Description>States that can be applied to a device</Description>
<RefSemantic CorrespondingAttributePath=""CDD/FailSafePosition"" />
<Constraint Name=""Enumeration"">
<NominalScaledType>
<RequiredValue>NDE</RequiredValue>
<RequiredValue>NE</RequiredValue>
</NominalScaledType>
</Constraint>
</AttributeType>
<AttributeType Name=""MaxAllowableResponseTime"" ID=""1b53ab6f-3b74-47f6-a727-4a83432d7499""  AttributeDataType=""xs:decimal"" >
<Description></Description>
<RefSemantic CorrespondingAttributePath=""CDD/ControlCircuitResponseTime"" />
</AttributeType>
<AttributeType Name=""E_DEToTrip"" ID=""356fecfe-ad51-4064-9d56-22ca44fa9dfd"" AttributeDataType=""xs:string"">
<Description>States that can be applied to a device</Description>
<Constraint Name=""Enumeration"">
<NominalScaledType>
<RequiredValue>DE</RequiredValue>
<RequiredValue>E</RequiredValue>
</NominalScaledType>
</Constraint>
</AttributeType>
<AttributeType Name=""Frequency"" ID=""fe215ed3-3c7f-4d3e-8056-ed05b0e18101""  AttributeDataType=""xs:decimal"" >
<Description>Percent as a decimal number</Description>
</AttributeType>
</AttributeTypeLib>
</CAEXFile>";
    }
}
