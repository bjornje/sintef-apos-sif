using Aml.Engine.CAEX;
using Sintef.Apos.Sif.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Sintef.Apos.Sif
{
    public class Builder
    {
        public Roots Roots { get; } = new Roots();
        private Roots _rootsClone;

        public IEnumerable<ModelError> Errors => _errors;
        private Collection<ModelError> _errors { get; } = new Collection<ModelError>();

        public bool HasChanges => !Roots.IsSameAs(_rootsClone);

        public Builder() 
        {
            Roots.Append();
            _rootsClone = Roots.Clone();
        }


        public SIFs SIFs => Roots.First().SIFs;

        public bool Validate()
        {
            _errors.Clear();

            Roots.Validate(_errors);

            return _errors.Count == 0;
        }

        public void PushAttributes()
        {
            Roots.PushAttributes();
        }
        public void LoadFromFile(string fileName)
        {
            Roots.Clear();
            _errors.Clear();

            var doc = CAEXDocument.LoadFromFile(fileName);
            WriteModel(doc);
        }

        public void LoadFromStream(Stream inputStream)
        {
            Roots.Clear();
            _errors.Clear();

            var doc = CAEXDocument.LoadFromStream(inputStream);
            WriteModel(doc);
        }

        private void WriteModel(CAEXDocument doc)
        {
            var originVersion = doc.CAEXFile.SourceDocumentInformation.Select(x => x.OriginVersion).SingleOrDefault();


            foreach (var ih in doc.CAEXFile.InstanceHierarchy)
            {
                var root = Roots.Append();

                if (originVersion != Definition.Version)
                {
                    _errors.Add(new ModelError(root, $"Expected UML model version {Definition.Version}. Loaded document has UML model version {originVersion}."));
                }

                foreach (var ie in ih.InternalElement)
                {
                    ReadInternalElement(root, ie, _errors);
                }
            }

            Roots.SetCrossVotingGroups();

            Roots.Validate(_errors);

            _rootsClone = Roots.Clone();
        }

        private static void ReadInternalElement(Node node, InternalElementType ie, Collection<ModelError> errors)
        {
            Node childNode = null;

            switch (ie.RefBaseSystemUnitPath)
            {
                case "SIF Unit Classes/SIF": // depricated, kept for backward compatibility
                case "SIF Unit Classes/SafetyInstrumentedFunction":
                    if (node is Root root)
                    {
                        childNode = root.SIFs.Append();
                    }
                    else
                    {
                        errors.Add(new ModelError(node, $"A SafetyInstrumentedFunction can only be added at root level in the hierarchy.\n{ie.Node}"));
                    }
                    break;
                case "SIF Unit Classes/SIFComponent": // depricated, kept for backward compatibility
                case "SIF Unit Classes/SIFSubsystem": // depricated, kept for backward compatibility
                case "SIF Unit Classes/Subsystem":
                    if (node is SafetyInstrumentedFunction sif)
                    {
                        if (ie.Name == "InputDeviceSubsystem" || ie.Name == "InputDevice" || ie.Name == "Initiator")
                        {
                            childNode = sif.Subsystems.AppendInputDevice();
                        }
                        else if (ie.Name == "LogicSolverSubsystem" || ie.Name == "LogicSolver" || ie.Name == "Solver")
                        {
                            childNode = sif.Subsystems.AppendLogicSolver();
                        }
                        else if (ie.Name == "FinalElementSubsystem" || ie.Name == "FinalElement")
                        {
                            childNode = sif.Subsystems.AppendFinalElement();
                        }
                        else
                        {
                            errors.Add(new ModelError(node, $"Bad name for Subsystem: {ie.Name}. A SIF Unit Class of type Subsystem must have name InputDeviceSubsystem, LogicSolverSubsystem or FinalElementSubsystem.\n{ie.Node}"));
                        }
                    }
                    else
                    {
                        errors.Add(new ModelError(node, $"A Subsystem can only be added to a SafetyInstrumentedFunction in the hierarchy.\n{ie.Node}"));
                    }
                    break;
                case "SIF Unit Classes/Group":
                    if (node is InputDeviceSubsystem inputDevice)
                    {
                        childNode = inputDevice.Groups.Append();
                    }
                    else if (node is LogicSolverSubsystem logicSolver)
                    {
                        childNode = logicSolver.Groups.Append();
                    }
                    else if (node is FinalElementSubsystem finalElement)
                    {
                        childNode = finalElement.Groups.Append();
                    }
                    else if (node is Group group)
                    {
                        childNode = group.Groups.Append();
                    }
                    else
                    {
                        errors.Add(new ModelError(node, $"An instance of Group can only be added to an instance of Subsystem or Group.\n{ie.Node}"));
                    }
                    break;
                case "SIS Unit Classes/InitiatorComponent": //depricated, kept for backward compatibility
                case "SIS Unit Classes/InputDeviceComponent": //depricated, kept for backward compatibility
                case "SIS Unit Classes/InputDeviceRequirements":
                    if (node is Group initiatorGroup)
                    {
                        childNode = initiatorGroup.Components.Append(ie.Name) as InputDeviceRequirements;
                    }
                    else
                    {
                        errors.Add(new ModelError(node, $"An instance of InputDeviceRequirements can only by added to an instance of Group.\n{ie.Node}"));
                    }
                    break;
                case "SIS Unit Classes/SolverComponent": // depricated, kept for backward compatibility
                case "SIS Unit Classes/LogicSolverComponent": // depricated, kept for backward compatibility
                case "SIS Unit Classes/LogicSolverRequirements":
                    if (node is Group logicSolverGroup)
                    {
                        childNode = logicSolverGroup.Components.Append(ie.Name);
                    }
                    else
                    {
                        errors.Add(new ModelError(node, $"An instance of LogicSolverRequirements can only by added to an instance of Group.\n{ie.Node}"));
                    }
                    break;
                case "SIS Unit Classes/FinalComponent": // depricated, kept for backward compatibility
                case "SIS Unit Classes/FinalElementComponent": // depricated, kept for backward compatibility
                case "SIS Unit Classes/FinalElementRequirements":
                    if (node is Group finalElementGroup)
                    {
                        childNode = finalElementGroup.Components.Append(ie.Name) as FinalElementRequirements;
                    }
                    else
                    {
                        errors.Add(new ModelError(node, $"An instance of FinalElementRequirements can only by added to an instance of Group.\n{ie.Node}"));
                    }
                    break;
                default: //for backward compatibility and documents
                    if (node is SafetyInstrumentedFunction sif2)
                    {
                        if (ie.Name == "Documents")
                        {
                            childNode = sif2.Documents;
                        }
                        else
                        {
                            childNode = sif2; // used to have a subsystem between sif and SIFComponent, skip to next internal element
                        }
                    }
                    else if (node is Documents documents)
                    {
                        childNode = documents.Append(ie.Name);
                    }
                    else if (node is Subsystem sifSubsystem && (ie.Name == "GroupVoter" || ie.Name == "ComponentVoter"))
                    {
                        var m = ReadAttribute("M", ie);

                        sifSubsystem.MInVotingMooN.StringValue = m;
                        return;
                    }
                    else if (node is Group group && (ie.Name == "ComponentVoter" || ie.Name == "GroupVoter"))
                    {
                        var k = ReadAttribute("K", ie);

                        if (k == null) k = ReadAttribute("M", ie); // for backward compatibility

                        group.MInVotingMooN.StringValue = k;
                        return;
                    }
                    break;
            }

            if (childNode == null)
            {
                errors.Add(new ModelError(node, $"Unexpected InternalElement.\n{ie.Node}"));
                return;
            }
            else
            {
                foreach(var att in ie.Attribute)
                {
                    var attributeName = att.Name;

                    switch(att.Name)  //for backward compatibility
                    {
                        case "NumberOfGroups_N":
                            attributeName = "NumberOfGroups";
                            break;
                        case "NumberOfComponentsOrSubgroups_N":
                            attributeName = "NumberOfDevicesWithinGroup";
                            break;
                        case "VoteWithinGroup_K_in_KooN":
                        case "VoteBetweenGroups_M_in_MooN":
                            attributeName = "MInVotingMooN";
                            break;
                    }

                    var nameIsDepricated = attributeName != att.Name;

                    var attribute = childNode.Attributes.FirstOrDefault(x => x.Name == attributeName);

                    if (attribute == null && !nameIsDepricated)
                    {    
                        if (Definition.IsOrdereListType(att.RefAttributeType))
                        {
                            var itemAttribute = att.Attribute.FirstOrDefault();
                            if (itemAttribute != null && Definition.TryAddAttribute(att.Name, itemAttribute.RefAttributeType, true, childNode.GetType().Name))
                            {
                                childNode.PushAttributes();
                                attribute = childNode.Attributes.FirstOrDefault(x => x.Name == att.Name);
                            }
                        }
                        else if (Definition.TryAddAttribute(att.Name, att.RefAttributeType, false, childNode.GetType().Name))
                        {
                            childNode.PushAttributes();
                            attribute = childNode.Attributes.FirstOrDefault(x => x.Name == att.Name);
                        }
                    }

                    if (attribute == null)
                    {
                        continue;
                    }

                    if (attribute.IsOrderedList)
                    {
                        var stringValues = ReadOrderedListAttribute(att.Name, ie);
                        foreach(var value in stringValues)
                        {
                            var listItem = attribute.CreateItem();
                            listItem.StringValue = value;
                            attribute.Items.Add(listItem);
                        }

                        continue;
                    }

                    attribute.StringValue = ReadAttribute(att.Name, ie);
                }
            }

            foreach (var childIe in ie.InternalElement)
            {
                ReadInternalElement(childNode, childIe, errors);
            }
        }


        private static string ReadAttribute(string name, InternalElementType ie)
        {
            var attribute = ie.Attribute.SingleOrDefault(x => x.Name == name);
            if (attribute == null)
            {
                return null;
            }

            return attribute.Value;
        }

        private static IEnumerable<string> ReadOrderedListAttribute(string name, InternalElementType ie)
        {
            var attribute = ie.Attribute.SingleOrDefault(x => x.Name == name);
            if (attribute == null)
            {
                return new string[0];
            }

            var values = new List<string>();

            foreach (var itemAttribute in attribute.Attribute)
            {
                values.Add(itemAttribute.Value);
            }

            return values;
        }

        public void SaveToFile(string outputFileName)
        {
            var doc = ReadModel();
            var xDoc = XDocument.Load(doc.SaveToStream(false));
            xDoc.Save(outputFileName, SaveOptions.None);
            _rootsClone = Roots.Clone();
        }

        public void SaveToStream(Stream outputStream)
        {
            var doc = ReadModel();
            var xDoc = XDocument.Load(doc.SaveToStream(false));
            xDoc.Save(outputStream, SaveOptions.None);
            _rootsClone = Roots.Clone();
        }

        private CAEXDocument ReadModel()
        {
            var doc = CAEXDocument.LoadFromString(Definition.Model);

            foreach (var root in Roots)
            {
                var ih = doc.CAEXFile.InstanceHierarchy.Append();
                ih.Name = "InstanceHierarchy";

                foreach (var sif in root.SIFs)
                {
                    var sifElement = ih.InternalElement.Append();

                    sifElement.Name = sif.GetType().Name;
                    sifElement.RefBaseSystemUnitPath = $"{Definition.SIFUnitClasses}/{typeof(SafetyInstrumentedFunction).Name}";

                    foreach (var item in sif.Attributes)
                    {
                        WriteAttribute(item, sifElement);
                    }

                    foreach (var subsystem in sif.Subsystems)
                    {
                        WriteSubsystem(subsystem, sifElement.InternalElement.Append());
                    }

                    if (sif.Documents.Any())
                    {
                        var documentsElement = sifElement.InternalElement.Append();
                        documentsElement.Name = "Documents";

                        foreach (var document in sif.Documents)
                        {
                            WriteDocument(document, documentsElement.InternalElement.Append());
                        }
                    }
                }
            }

            return doc;
        }

        private static void WriteSubsystem(Subsystem subsystem, InternalElementType subsystemElement)
        {
            subsystemElement.Name = subsystem.GetType().Name;
            subsystemElement.RefBaseSystemUnitPath = $"{Definition.SIFUnitClasses}/{typeof(Subsystem).Name}";

            foreach (var item in subsystem.Attributes)
            {
                WriteAttribute(item, subsystemElement);
            }

            foreach (var group in subsystem.Groups)
            {
                WriteGroup(group, subsystemElement.InternalElement.Append());
            }
        }

        private static void WriteDocument(Document document, InternalElementType documentElement)
        {
            documentElement.Name = document.Name.Value;

            WriteBasicAttribute(document.Language, documentElement);

            var externalInterface = documentElement.ExternalInterface.Append();
            externalInterface.Name = document.ExternalInterface.Template.Name;
            externalInterface.RefBaseClassPath = document.ExternalInterface.Template.RefBaseClassPath;

            WriteBasicAttribute(document.ExternalInterface.MIMEType, externalInterface);
            WriteBasicAttribute(document.ExternalInterface.RefURI, externalInterface);

            var roleRequirement = documentElement.RoleRequirements.Append();
            roleRequirement.RefBaseRoleClassPath = document.ExternalInterface.Template.RefBaseRoleClassPath;
        }

        private static void WriteGroup(Group group, InternalElementType groupElement)
        {
            groupElement.Name = group.GetType().Name;
            groupElement.RefBaseSystemUnitPath = $"{Definition.SIFUnitClasses}/{typeof(Group).Name}";

            foreach (var item in group.Attributes)
            {
                WriteAttribute(item, groupElement);
            }

            foreach (var component in group.Components)
            {
                WriteSISComponent(component, groupElement.InternalElement.Append());
            }

            foreach (var subGroup in group.Groups)
            {
                WriteGroup(subGroup, groupElement.InternalElement.Append());
            }
        }

        private static void WriteSISComponent(SISDeviceRequirements component, InternalElementType componentElement)
        {
            componentElement.Name = component.GetType().Name;

            if (component is InputDeviceRequirements)
            {
                componentElement.RefBaseSystemUnitPath = $"{Definition.SISUnitClasses}/{typeof(InputDeviceRequirements).Name}";
            }
            else if (component is LogicSolverRequirements)
            {
                componentElement.RefBaseSystemUnitPath = $"{Definition.SISUnitClasses}/{typeof(LogicSolverRequirements).Name}";
            }
            else if (component is FinalElementRequirements)
            {
                componentElement.RefBaseSystemUnitPath = $"{Definition.SISUnitClasses}/{typeof(FinalElementRequirements).Name}";
            }

            foreach (var item in component.Attributes)
            {
                WriteAttribute(item, componentElement);
            }
        }

        private static void WriteAttribute(IAttribute attribute, InternalElementType ie)
        {
            if (attribute == null)
            {
                return;
            }

            if ((attribute.Items != null && attribute.Items.Count == 0) || (attribute.Items == null && attribute.StringValue == null))
            {
                return;
            }

            var att = ie.Attribute.Append();
            att.Name = attribute.Name;
            att.ID = Guid.NewGuid().ToString();
            att.RefAttributeType = attribute.RefAttributeType;

            if (attribute.Items == null)
            {
                att.Value = attribute.StringValue;
            }
            else
            {
                var index = 1;
                foreach(var item in attribute.Items)
                {
                    WriteListItemAttribute(index, item, att);
                    index++;
                }
            }
        }

        private static void WriteBasicAttribute(IAttribute attribute, InternalElementType ie)
        {
            if (attribute == null)
            {
                return;
            }

            if (attribute.StringValue == null)
            {
                return;
            }

            var att = ie.Attribute.Append();
            att.Name = attribute.Name;
            att.ID = Guid.NewGuid().ToString();
            att.AttributeDataType = attribute.RefAttributeType;
            att.Value = attribute.StringValue;
        }

        private static void WriteBasicAttribute(IAttribute attribute, InterfaceClassType ie)
        {
            if (attribute == null)
            {
                return;
            }

            if (attribute.StringValue == null)
            {
                return;
            }

            var att = ie.Attribute.Append();
            att.Name = attribute.Name;
            att.ID = Guid.NewGuid().ToString();
            att.AttributeDataType = attribute.RefAttributeType;
            att.Value = attribute.StringValue;
        }

        private static void WriteListItemAttribute(int index, IAttribute attribute, AttributeType ownerAttribute)
        {
            var att = ownerAttribute.Attribute.Append();
            att.Name = index.ToString(CultureInfo.InvariantCulture);
            att.ID = Guid.NewGuid().ToString();
            att.Value = attribute.StringValue;
            att.RefAttributeType = attribute.RefAttributeType;
        }

    }
}
