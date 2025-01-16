using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class Node
    {
        public Node Parent { get; }
        public IEnumerable<AttributeType> Attributes => _attributes;
        public string PathStepX { get; }

        private readonly Collection<AttributeType> _attributes = new Collection<AttributeType>();
        public Node(Node parent, string pathStep)
        {
            Parent = parent;
            PathStepX = pathStep;
        }


        protected void AddAttribute(AttributeType attribute)
        {
            _attributes.Add(attribute.Clone());
        }

        public string Path
        {
            get
            {
                string path = "";
                var node = this;

                while(node != null)
                {
                    var pathStep = node.PathStepX;

                    if (node is SIF sif) pathStep = sif.SIFID.Value;
                    else if (node is SISComponent sISComponent) pathStep = sISComponent.Name.Value;

                    path = $"/{pathStep}{path}";
                    node = node.Parent;
                }

                return path.TrimStart('/');
            }
        }

        public string GetSIFID()
        {
            var node = this;

            while (node != null)
            {
                if (node is SIF sif)
                {
                    if (sif.SIFID == null) return "<missing>";
                    if (string.IsNullOrWhiteSpace(sif.SIFID.Value)) return "<blank>";
                    return sif.SIFID.Value;
                }
                node = node.Parent;
            }

            return null;
        }

        public ComponentVoter GetComponentVoter()
        {
            var node = this;

            while (node != null)
            {
                if (node is Group group) return group.ComponentVoter;
                node = node.Parent;
            }

            return null;
        }

        public GroupVoter GetGroupVoter()
        {
            var node = this;

            while (node != null)
            {
                if (node is SIFComponent sifComponent) return sifComponent.GroupVoter;
                node = node.Parent;
            }

            return null;
        }

        public bool HaveSameAttributeValues(Node node)
        {
            if (node.Attributes.Count() != _attributes.Count) return false;

            foreach(var attribute in node.Attributes)
            {
                var myAttribute = _attributes.FirstOrDefault(x => x.Name == attribute.Name);
                if (myAttribute == null)
                    return false;

                if (string.IsNullOrEmpty(myAttribute.Value) && !string.IsNullOrEmpty(attribute.Value))
                    return false;

                if (!string.IsNullOrEmpty(myAttribute.Value) && string.IsNullOrEmpty(attribute.Value))
                    return false;

                if (!string.IsNullOrEmpty(myAttribute.Value) && !string.IsNullOrEmpty(attribute.Value) && myAttribute.Value != attribute.Value)
                    return false;

            }

            return true;
        }

    }
}
