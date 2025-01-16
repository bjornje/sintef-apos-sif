using Aml.Engine.CAEX;
using Sintef.Apos.Sif.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Linq;

namespace Sintef.Apos.Sif
{
    public class Builder
    {
        public Roots Roots { get; } = new Roots();
        private Roots _rootsClone = new Roots();

        public IEnumerable<ModelError> Errors => _errors;
        private Collection<ModelError> _errors { get; } = new Collection<ModelError>();

        public bool HasChanges => !Roots.IsSameAs(_rootsClone);

        public Builder() 
        {
            var x = SIFs;
            _rootsClone = Roots.Clone();
        }


        public SIFs SIFs
        { 
            get
            {
                if (!Roots.Any()) return Roots.Append().SIFs;
                return Roots.First().SIFs;
            }
        }

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
                        var newSif = root.SIFs.Append("New SIF");
                        childNode = newSif;

                        foreach(var item in newSif.Attributes)
                        {
                            item.Value = ReadAttribute(item.Name, ie);
                        }
                    }
                    else errors.Add(new ModelError(node, $"A SIF can only be added at root level in the hierarchy.\n{ie.Node}"));
                    break;
                case "SIF Unit Classes/SIFComponent":
                    if (node is SIFSubsystem subsystem)
                    {
                        if (ie.Name == "InputDevice" || ie.Name == "Initiator")
                        {
                            var newInputDevice = subsystem.CreateInputDevice();
                            childNode = newInputDevice;
                            foreach (var item in newInputDevice.Attributes)
                            {
                                item.Value = ReadAttribute(item.Name, ie);
                            }
                        }
                        else if (ie.Name == "LogicSolver" || ie.Name == "Solver")
                        {
                            var newLogicSolver = subsystem.CreateLogicSolver();
                            childNode = newLogicSolver;
                            foreach (var item in newLogicSolver.Attributes)
                            {
                                item.Value = ReadAttribute(item.Name, ie);
                            }
                        }
                        else if (ie.Name == "FinalElement")
                        {
                            var newFinalElement = subsystem.CreateFinalElement();
                            childNode = newFinalElement;
                            foreach (var item in newFinalElement.Attributes)
                            {
                                item.Value = ReadAttribute(item.Name, ie);
                            }
                        }
                        else errors.Add(new ModelError(node, $"Bad name for InternalElement: {ie.Name}. A SIF Unit Class of type SIFComponent must have name InputDevice, LogicSolver or FinalElement.\n{ie.Node}"));
                    }
                    else errors.Add(new ModelError(node, $"A SIFComponent can only be added to a SIFSubsystem in the hierarchy.\n{ie.Node}"));
                    break;
                case "SIF Unit Classes/Group":
                    if (node is InputDevice inputDevice)
                    {
                        var newInputDeviceGroup = inputDevice.Groups.Append();
                        childNode = newInputDeviceGroup;
                    }
                    else if (node is LogicSolver logicSolver)
                    {
                        var newLogicSolverGroup = logicSolver.Groups.Append();
                        childNode = newLogicSolverGroup;
                    }
                    else if (node is FinalElement finalElement)
                    {
                        var newFinalElementGroup = finalElement.Groups.Append();
                        childNode = newFinalElementGroup;
                    }
                    else if (node is Group group)
                    {
                        var newSubGroup = group.Groups.Append();
                        childNode = newSubGroup;
                    }
                    else errors.Add(new ModelError(node, $"An instance of Group can only be added to an instance of InputDevice, LogicSolver, FinalElement or Group.\n{ie.Node}"));
                    break;
                case "SIS Unit Classes/InitiatorComponent":
                    if (node is InputDeviceGroup initiatorGroup)
                    {
                        var newInitiatorComponent = initiatorGroup.Components.Append(ie.Name) as InitiatorComponent;
                        childNode = newInitiatorComponent;

                        foreach (var item in newInitiatorComponent.Attributes)
                        {
                            item.Value = ReadAttribute(item.Name, ie);
                        }
                    }
                    else errors.Add(new ModelError(node, $"An instance of InitiatorComponent can only by added to an instance of InitiatorGroup.\n{ie.Node}"));
                    break;
                case "SIS Unit Classes/SolverComponent":
                case "SIS Unit Classes/LogicSolverComponent":
                    if (node is LogicSolverGroup logicSolverGroup)
                    {
                        var newLogicSolverComponent = logicSolverGroup.Components.Append(ie.Name);
                        childNode = newLogicSolverComponent;

                        foreach (var item in newLogicSolverComponent.Attributes)
                        {
                            item.Value = ReadAttribute(item.Name, ie);
                        }
                    }
                    else errors.Add(new ModelError(node, $"An instance of LogicSolverComponent can only by added to an instance of LogicSolverGroup.\n{ie.Node}"));
                    break;
                case "SIS Unit Classes/FinalComponent":
                case "SIS Unit Classes/FinalElementComponent":
                    if (node is FinalElementGroup finalElementGroup)
                    {
                        var newFinalElementComponent = finalElementGroup.Components.Append(ie.Name) as FinalElementComponent;
                        childNode = newFinalElementComponent;

                        foreach (var item in newFinalElementComponent.Attributes)
                        {
                            item.Value = ReadAttribute(item.Name, ie);
                        }
                    }
                    else errors.Add(new ModelError(node, $"An instance of FinalElementComponent can only by added to an instance of FinalElementGroup.\n{ie.Node}"));
                    break;
                default:
                    if (node is SIF sif)
                    {
                        var newSubsystem = sif.Subsystems.Append();
                        childNode = newSubsystem;

                        foreach (var item in newSubsystem.Attributes)
                        {
                            item.Value = ReadAttribute(item.Name, ie);
                        }
                    }
                    else if (node is SIFComponent sifComponent && (ie.Name == "GroupVoter" || ie.Name == "ComponentVoter"))
                    {
                        var m = ReadAttribute("M", ie);
                        sifComponent.GroupVoter.M.Value = m;
                        childNode = sifComponent.GroupVoter;
                    }
                    else if (node is Group group && (ie.Name == "ComponentVoter" || ie.Name == "GroupVoter"))
                    {
                        var k = ReadAttribute("K", ie);

                        if (k == null) k = ReadAttribute("M", ie); // for backward compatibility

                        group.ComponentVoter.K.Value = k;
                        childNode = group.ComponentVoter;
                    }
                    break;
            }

            if (childNode == null)
            {
                errors.Add(new ModelError(node, $"Unexpected InternalElement.\n{ie.Node}"));
                return;
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

                    foreach(var item in sif.Attributes)
                    {
                        WriteAttribute(item, sifElement);
                    }

                    foreach (var subsystem in sif.Subsystems) WriteSubsystem(subsystem, sifElement.InternalElement.Append());
                }
            }

            var xDoc = XDocument.Load(doc.SaveToStream(false));
            xDoc.Save(outputFileName, SaveOptions.None);
            _rootsClone = Roots.Clone();
        }

        private void WriteSubsystem(SIFSubsystem subsystem, InternalElementType subsystemElement)
        {
            subsystemElement.Name = subsystem.GetType().Name;
            foreach (var item in subsystem.Attributes)
            {
                WriteAttribute(item, subsystemElement);
            }

            if (subsystem.InputDevice != null) WriteSIFComponent(subsystem.InputDevice, subsystemElement.InternalElement.Append());
            if (subsystem.LogicSolver != null) WriteSIFComponent(subsystem.LogicSolver, subsystemElement.InternalElement.Append());
            if (subsystem.FinalElement != null) WriteSIFComponent(subsystem.FinalElement, subsystemElement.InternalElement.Append());
        }

        private void WriteSIFComponent(SIFComponent component, InternalElementType componentElement)
        {
            componentElement.Name = component.GetType().Name;
            componentElement.RefBaseSystemUnitPath = SIFComponent.RefBaseSystemUnitPath;

            foreach (var item in component.Attributes)
            {
                WriteAttribute(item, componentElement);
            }

            if (component.GroupVoter != null)
            {
                var groupVoterElement = componentElement.InternalElement.Append();
                groupVoterElement.Name = component.GroupVoter.GetType().Name;
                WriteAttribute(component.GroupVoter.M, groupVoterElement);
            }

            foreach (var group in component.Groups) WriteGroup(group, componentElement.InternalElement.Append());
        }

        private void WriteGroup(Group group, InternalElementType groupElement)
        {
            groupElement.Name = group.GetType().Name;
            groupElement.RefBaseSystemUnitPath = Group.RefBaseSystemUnitPath;

            if (group.ComponentVoter != null)
            {
                var componentVoterElement = groupElement.InternalElement.Append();
                componentVoterElement.Name = group.ComponentVoter.GetType().Name;
                WriteAttribute(group.ComponentVoter.K, componentVoterElement);
            }

            foreach (var component in group.Components) WriteSISComponent(component, groupElement.InternalElement.Append());
            foreach (var subGroup in group.Groups) WriteGroup(subGroup, groupElement.InternalElement.Append());
        }

        private void WriteSISComponent(SISComponent component, InternalElementType componentElement)
        {
            componentElement.Name = component.Name.Value;

            foreach (var item in component.Attributes)
            {
                WriteAttribute(item, componentElement);
            }

            if (component is InitiatorComponent)
            {
                componentElement.RefBaseSystemUnitPath = InitiatorComponent.RefBaseSystemUnitPath;
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

        private void WriteAttribute(Model.AttributeType attribute, InternalElementType ie)
        {
            if (attribute == null) return;
            if (attribute.Value == null) return;

            var att = ie.Attribute.Append();
            att.Name = attribute.Name;
            att.ID = Guid.NewGuid().ToString();
            att.Value = attribute.Value;
            att.RefAttributeType = attribute.RefAttributeType;
        }

    }
}
