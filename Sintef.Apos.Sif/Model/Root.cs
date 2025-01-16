using Aml.Engine.Resources.Catalogue;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.SymbolStore;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class Root : Node
    {

        public SIFs SIFs { get; }
        public Root(Roots roots) : base(null, roots.Any() ? $"SIFs{roots.Count() + 1}" : "SIFs")
        {
            SIFs = new SIFs(this);
        }

        public Root(Roots roots, string pathStep) : base(null, pathStep)
        {
            SIFs = new SIFs(this);
        }

        public bool IsSameAs(Root root)
        {
            if (!HaveSameAttributeValues(root)) return false;

            if (!SIFs.IsSameAs(root.SIFs)) return false;

            return true;
        }



        public IEnumerable<Node> GetLeafs()
        {
            var list = new List<Node>();

            if (!SIFs.Any()) list.Add(this);

            foreach(var sif in SIFs)
            {
                if (!sif.Subsystems.Any()) list.Add(sif);

                foreach(var subsystem in sif.Subsystems)
                {
                    if (!subsystem.Components().Any()) list.Add(subsystem);

                    foreach(var component in subsystem.Components())
                    {
                        if (!component.Groups.Any()) list.Add(component);

                        foreach(var group in component.Groups)
                        {
                            if (!group.Components.Any()) list.Add(group);

                            foreach(var sisComponent in group.Components)
                            {
                                list.Add(sisComponent);
                            }
                        }
                    }
                }
            }

            return list;
        }

        public IEnumerable<SISComponent> GetSISComponents()
        {
            var list = new List<SISComponent>();
            foreach(var component in GetSIFComponents())
            {
                foreach (var group in component.Groups) list.AddRange(group.Components);
            }

            return list;
        }

        public IEnumerable<SIFSubsystem> GetSIFSubsystems()
        {
            var list = new List<SIFSubsystem>();

            foreach(var sif in SIFs)
            {
                list.AddRange(sif.Subsystems);
            }

            return list;
        }

        public IEnumerable<SIFComponent> GetSIFComponents()
        {
            var list = new List<SIFComponent>();
            foreach (var subsystem in GetSIFSubsystems())
            {
                if (subsystem.InputDevice != null) list.Add(subsystem.InputDevice);
                if (subsystem.LogicSolver != null) list.Add(subsystem.LogicSolver);
                if (subsystem.FinalElement != null) list.Add(subsystem.FinalElement);
            }

            return list;
        }

        public IEnumerable<ModelError> GetModelErrors()
        {
            var list = new List<ModelError>();

            foreach(var sif in SIFs)
            {
                if (!sif.Subsystems.Any()) list.Add(new ModelError(sif, "Missing SIFSubsystem."));

                foreach(var subsystem in sif.Subsystems)
                {
                    if (subsystem.LogicSolver == null) list.Add(new ModelError(subsystem, "Missing LogicSolver."));

                    if (subsystem.InputDevice != null) GetSIFComponentModelErrors(subsystem.InputDevice, list);
                    if (subsystem.LogicSolver != null) GetSIFComponentModelErrors(subsystem.LogicSolver, list);
                    if (subsystem.FinalElement != null) GetSIFComponentModelErrors(subsystem.FinalElement, list);
                }
            }

            return list;
        }

        private void GetSIFComponentModelErrors(SIFComponent sifComponent, List<ModelError> list)
        {
            if (sifComponent.GroupVoter == null) list.Add(new ModelError(sifComponent, "Missing GroupVoter."));
            if (!sifComponent.Groups.Any()) list.Add(new ModelError(sifComponent, $"Missing {sifComponent.GetType().Name}Group."));

            foreach(var group in sifComponent.Groups)
            {
                if (group.ComponentVoter == null) list.Add(new ModelError(group, "Missing ComponentVoter."));
                if (!group.Components.Any()) list.Add(new ModelError(group, $"Missing Component."));

                foreach(var sisComponent in group.Components)
                {

                }
            }
        }

        public void Validate(Collection<ModelError> errors)
        {
            SIFs.Validate(errors);
        }

    }

    public class Roots : IEnumerable<Root>
    {
        private readonly Collection<Root> _items = new Collection<Root>();

        
        public Root Append()
        {
            var root = new Root(this);
            _items.Add(root); 
            return root;
        }

        public IEnumerator<Root> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public void Clear()
        {
            _items.Clear();
        }

        public Roots Clone()
        {
            var clonedRoots = new Roots();

            foreach(var root in this)
            {
                var clonedRoot = clonedRoots.Append();

                foreach (var sif in root.SIFs) CloneSIF(clonedRoot.SIFs, sif);
            }

            return clonedRoots;
        }

        private static void CloneSIF(SIFs clonedSIFs, SIF sif)
        {
            var clonedSif = clonedSIFs.Append("New SIF");
            CopyAttributeValues(sif, clonedSif);

            foreach(var subsystem in sif.Subsystems)
            {
                var clonedSubsystem = clonedSif.Subsystems.Append();
                CopyAttributeValues(subsystem, clonedSubsystem);

                if (subsystem.InputDevice != null)
                {
                    var clonedInitiator = clonedSubsystem.CreateInputDevice();
                    CopyAttributeValues(subsystem.InputDevice, clonedInitiator);
                    CopyAttributeValues(subsystem.InputDevice.GroupVoter, clonedInitiator.GroupVoter);
                    foreach (var group in subsystem.InputDevice.Groups) CloneGroup(clonedInitiator.Groups, group);
                }

                if (subsystem.LogicSolver != null)
                {
                    var clonedSolver = clonedSubsystem.CreateLogicSolver();
                    CopyAttributeValues(subsystem.LogicSolver, clonedSolver);
                    CopyAttributeValues(subsystem.LogicSolver.GroupVoter, clonedSolver.GroupVoter);
                    foreach (var group in subsystem.LogicSolver.Groups) CloneGroup(clonedSolver.Groups, group);
                }

                if (subsystem.FinalElement != null)
                {
                    var clonedFinalElement = clonedSubsystem.CreateFinalElement();
                    CopyAttributeValues(subsystem.FinalElement, clonedFinalElement);
                    CopyAttributeValues(subsystem.FinalElement.GroupVoter, clonedFinalElement.GroupVoter);
                    foreach (var group in subsystem.FinalElement.Groups) CloneGroup(clonedFinalElement.Groups, group);
                }
            }
        }

        private static void CloneGroup(Groups clonedGroups, Group group)
        {
            var clonedGroup = clonedGroups.Append();
            CopyAttributeValues(group, clonedGroup);
            CopyAttributeValues(group.ComponentVoter, clonedGroup.ComponentVoter);

            foreach (var component in group.Components)
            {
                var clonedComponent = clonedGroup.Components.Append(component.Name.Value);
                CopyAttributeValues(component, clonedComponent);
            }

            foreach (var subGroup in group.Groups)
            {
                CloneGroup(clonedGroup.Groups, subGroup);
            }
        }

        private static void CopyAttributeValues(Node fromNode, Node toNode)
        {
            if (fromNode.Attributes.Count() != toNode.Attributes.Count()) throw new System.Exception("Nuber of properties differ in fromNode and toNode.");

            foreach(var fromProperty in fromNode.Attributes)
            {
                var toProperty = toNode.Attributes.Single(x => x.Name == fromProperty.Name);
                toProperty.Value = fromProperty.Value;
            }
        }

        public bool IsSameAs(Roots roots)
        {
            if (_items.Count != roots.Count()) return false;

            var alreadyMatchedRoots = new List<Root>();

            foreach (var root in roots)
            {
                var myRoot = _items.FirstOrDefault(x => !alreadyMatchedRoots.Contains(x) && x.IsSameAs(root));
                if (myRoot == null) return false;
                alreadyMatchedRoots.Add(myRoot);
            }

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach(var root in _items)
            {
                root.Validate(errors);
            }
        }
    }
}
