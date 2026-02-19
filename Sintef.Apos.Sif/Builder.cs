using Aml.Engine.CAEX;
using Aml.Engine.CAEX.Extensions;
using Sintef.Apos.Sif.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
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

            return !_errors.Any();
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

                if (originVersion != Definition.Version) _errors.Add(new ModelError(root, $"Expected UML model version {Definition.Version}. Loaded document has UML model version {originVersion}."));

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
                case "SIF Unit Classes/SIF":
                    if (node is Root root)
                    {
                        childNode = root.SIFs.Append();
                    }
                    else errors.Add(new ModelError(node, $"A SIF can only be added at root level in the hierarchy.\n{ie.Node}"));
                    break;
                case "SIF Unit Classes/SIFComponent": // depricated, kept for backward compatibility
                case "SIF Unit Classes/SIFSubsystem":
                    if (node is SIF sif)
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
                        else errors.Add(new ModelError(node, $"Bad name for InternalElement: {ie.Name}. A SIF Unit Class of type SIFComponent must have name InputDevice, LogicSolver or FinalElement.\n{ie.Node}"));
                    }
                    else
                    {
                        if (ie.RefBaseSystemUnitPath == "SIF Unit Classes/SIFComponent") errors.Add(new ModelError(node, $"A SIFComponent can only be added to a SIFSubsystem in the hierarchy.\n{ie.Node}"));
                        else errors.Add(new ModelError(node, $"A SIFSubsystem can only be added to a SIF in the hierarchy.\n{ie.Node}"));
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
                    else errors.Add(new ModelError(node, $"An instance of Group can only be added to an instance of SIFSubsystem or Group.\n{ie.Node}"));
                    break;
                case "SIS Unit Classes/InitiatorComponent": //depricated, kept for backward compatibility
                case "SIS Unit Classes/InputDeviceComponent":
                    if (node is Group initiatorGroup)
                    {
                        childNode = initiatorGroup.Components.Append(ie.Name) as InputDeviceComponent;
                    }
                    else errors.Add(new ModelError(node, $"An instance of InitiatorDeviceComponent can only by added to an instance of Group.\n{ie.Node}"));
                    break;
                case "SIS Unit Classes/SolverComponent": // depricated, kept for backward compatibility
                case "SIS Unit Classes/LogicSolverComponent":
                    if (node is Group logicSolverGroup)
                    {
                        childNode = logicSolverGroup.Components.Append(ie.Name);
                    }
                    else errors.Add(new ModelError(node, $"An instance of LogicSolverComponent can only by added to an instance of Group.\n{ie.Node}"));
                    break;
                case "SIS Unit Classes/FinalComponent": // depricated, kept for backward compatibility
                case "SIS Unit Classes/FinalElementComponent":
                    if (node is Group finalElementGroup)
                    {
                        childNode = finalElementGroup.Components.Append(ie.Name) as FinalElementComponent;
                    }
                    else errors.Add(new ModelError(node, $"An instance of FinalElementComponent can only by added to an instance of Group.\n{ie.Node}"));
                    break;
                default:
                    if (node is SIF sif2)
                    {
                        childNode = sif2; // used to have a subsystem between sif and SIFComponent, skip to next internal element
                    }
                    else if (node is SIFSubsystem sifSubsystem && (ie.Name == "GroupVoter" || ie.Name == "ComponentVoter"))
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
                foreach (var item in childNode.Attributes) item.StringValue = item.StringValue ?? ReadAttribute(item.Name, ie);
            }

            foreach (var childIe in ie.InternalElement) ReadInternalElement(childNode, childIe, errors);
        }


        private static string ReadAttribute(string name, InternalElementType ie)
        {
            var attribute = ie.Attribute.SingleOrDefault(x => x.Name == name);
            if (attribute == null) return null;
            return attribute.Value;
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
                    sifElement.RefBaseSystemUnitPath = SIF.RefBaseSystemUnitPath;

                    foreach (var item in sif.Attributes) WriteAttribute(item, sifElement);
                    foreach (var subsystem in sif.Subsystems) WriteSubsystem(subsystem, sifElement.InternalElement.Append());
                }
            }

            return doc;
        }

        private static void WriteSubsystem(SIFSubsystem subsystem, InternalElementType subsystemElement)
        {
            subsystemElement.Name = subsystem.GetType().Name;
            subsystemElement.RefBaseSystemUnitPath = SIFSubsystem.RefBaseSystemUnitPath;

            foreach (var item in subsystem.Attributes) WriteAttribute(item, subsystemElement);
            foreach (var group in subsystem.Groups) WriteGroup(group, subsystemElement.InternalElement.Append());
        }

        private static void WriteGroup(Group group, InternalElementType groupElement)
        {
            groupElement.Name = group.GetType().Name;
            groupElement.RefBaseSystemUnitPath = Group.RefBaseSystemUnitPath;

            foreach (var item in group.Attributes) WriteAttribute(item, groupElement);
            foreach (var component in group.Components) WriteSISComponent(component, groupElement.InternalElement.Append());
            foreach (var subGroup in group.Groups) WriteGroup(subGroup, groupElement.InternalElement.Append());
        }

        private static void WriteSISComponent(SISComponent component, InternalElementType componentElement)
        {
            componentElement.Name = component.Name.StringValue;

            foreach (var item in component.Attributes) WriteAttribute(item, componentElement);

            if (component is InputDeviceComponent)
            {
                componentElement.RefBaseSystemUnitPath = InputDeviceComponent.RefBaseSystemUnitPath;
            }
            else if (component is LogicSolverComponent)
            {
                componentElement.RefBaseSystemUnitPath = LogicSolverComponent.RefBaseSystemUnitPath;
            }
            else if (component is FinalElementComponent)
            {
                componentElement.RefBaseSystemUnitPath = FinalElementComponent.RefBaseSystemUnitPath;
            }
        }

        private static void WriteAttribute(Model.AttributeType attribute, InternalElementType ie)
        {
            if (attribute == null) return;
            if (attribute.StringValue == null) return;

            var att = ie.Attribute.Append();
            att.Name = attribute.Name;
            att.ID = Guid.NewGuid().ToString();
            att.Value = attribute.StringValue;
            att.RefAttributeType = attribute.RefAttributeType;
        }

    }
}
