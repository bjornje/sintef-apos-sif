using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
            if (!HaveSameAttributeValues(root))
            {
                return false;
            }

            if (!SIFs.IsSameAs(root.SIFs))
            {
                return false;
            }

            return true;
        }



        public IEnumerable<Node> GetLeaves()
        {
            var list = new List<Node>();

            if (!SIFs.Any())
            {
                list.Add(this);
            }

            foreach(var sif in SIFs)
            {
                if (!sif.Subsystems.Any())
                {
                    list.Add(sif);
                }

                foreach(var subsystem in sif.Subsystems)
                {
                    if (!subsystem.Groups.Any())
                    {
                        list.Add(subsystem);
                    }

                    foreach(var group in subsystem.Groups)
                    {
                        if (!group.Components.Any())
                        {
                            list.Add(group);
                        }

                        foreach(var sisComponent in group.Components)
                        {
                            list.Add(sisComponent);
                        }
                    }
                    
                }
            }

            return list;
        }

        public IEnumerable<Subsystem> GetSIFSubsystems()
        {
            var list = new List<Subsystem>();

            foreach(var sif in SIFs)
            {
                list.AddRange(sif.Subsystems);
            }

            return list;
        }

        public IEnumerable<ModelError> GetModelErrors()
        {
            var list = new List<ModelError>();

            foreach(var sif in SIFs)
            {
                if (!sif.Subsystems.Any()) list.Add(new ModelError(sif, "Missing SIFSubsystem."));
            }

            return list;
        }

        public void Validate(Collection<ModelError> errors)
        {
            SIFs.Validate(errors);
        }

        public void PushAttributes()
        {
            SIFs.PushAttributes();
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

                foreach (var sif in root.SIFs)
                {
                    CloneSIF(clonedRoot.SIFs, sif);
                }
            }

            clonedRoots.SetCrossVotingGroups();

            return clonedRoots;
        }

        private static void CloneSIF(SIFs clonedSIFs, SafetyInstrumentedFunction sif)
        {
            var clonedSif = clonedSIFs.Append();
            CopyAttributeValues(sif, clonedSif);

            if (sif.InputDevice != null)
            {
                var clonedInitiator = clonedSif.Subsystems.AppendInputDevice();
                CopyAttributeValues(sif.InputDevice, clonedInitiator);

                foreach (var group in sif.InputDevice.Groups)
                {
                    CloneGroup(clonedInitiator.Groups, group);
                }
            }

            if (sif.LogicSolver != null)
            {
                var clonedSolver = clonedSif.Subsystems.AppendLogicSolver();
                CopyAttributeValues(sif.LogicSolver, clonedSolver);

                foreach (var group in sif.LogicSolver.Groups)
                {
                    CloneGroup(clonedSolver.Groups, group);
                }
            }

            if (sif.FinalElement != null)
            {
                var clonedFinalElement = clonedSif.Subsystems.AppendFinalElement();
                CopyAttributeValues(sif.FinalElement, clonedFinalElement);

                foreach (var group in sif.FinalElement.Groups)
                {
                    CloneGroup(clonedFinalElement.Groups, group);
                }
            }
        }

        private static void CloneGroup(Groups clonedGroups, Group group)
        {
            var clonedGroup = clonedGroups.Append();
            CopyAttributeValues(group, clonedGroup);

            foreach (var component in group.Components)
            {
                var clonedComponent = clonedGroup.Components.Append(component.TagName.StringValue);
                CopyAttributeValues(component, clonedComponent);
            }

            foreach (var subGroup in group.Groups)
            {
                CloneGroup(clonedGroup.Groups, subGroup);
            }
        }

        private static void CopyAttributeValues(Node fromNode, Node toNode)
        {
            toNode.PushAttributes();

            //if (fromNode.Attributes.Count() != toNode.Attributes.Count())
            //{
            //    throw new Exception("Number of properties differ in fromNode and toNode.");
            //}

            foreach (var fromProperty in fromNode.Attributes)
            {
                var toProperty = toNode.Attributes.FirstOrDefault(x => x.Name == fromProperty.Name);

                if (toProperty == null)
                {
                    continue;
                }

                if (fromProperty.IsOrderedList)
                {
                    foreach(var fromItem in fromProperty.Items)
                    {
                        var toItem = toProperty.CreateItem();
                        toItem.StringValue = fromItem.StringValue;
                        toProperty.Items.Add(toItem);
                    }
                }
                else
                {
                    toProperty.StringValue = fromProperty.StringValue;
                }
            }
        }

        public bool IsSameAs(Roots roots)
        {
            if (_items.Count != roots.Count())
            {
                return false;
            }

            var alreadyMatchedRoots = new List<Root>();

            foreach (var root in roots)
            {
                var myRoot = _items.FirstOrDefault(x => !alreadyMatchedRoots.Contains(x) && x.IsSameAs(root));

                if (myRoot == null)
                {
                    return false;
                }

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

        public void PushAttributes()
        {
            foreach (var root in _items)
            {
                root.PushAttributes();
            }
        }

        public void SetCrossVotingGroups()
        {
            foreach (var root in _items)
            {
                foreach(var sif in root.SIFs)
                {
                    sif.SetCrossVotingGroups();
                }
            }
        }
    }
}
