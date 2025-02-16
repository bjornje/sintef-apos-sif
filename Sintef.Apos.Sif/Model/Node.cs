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
        public IEnumerable<AttributeType> Attributes => _attributes ?? Enumerable.Empty<AttributeType>();
        public string PathStepX { get; }

        private Collection<AttributeType> _attributes;
        public Node(Node parent, string pathStep)
        {
            Parent = parent;
            PathStepX = pathStep;
        }

        public int GetNumberOfGroupsAndComponents()
        {
            if (this is Group group) return group.Components.Count() + group.Groups.Count();
            if (this is SIFSubsystem subsystem) return subsystem.Groups.Count();
            return 0;
        }
        protected void SetAttributes(Collection<AttributeType> attributes)
        {
            _attributes = attributes;
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

                    if (node is SIF sif) pathStep = sif.SIFID.StringValue;
                    else if (node is SISComponent sISComponent) pathStep = sISComponent.Name.StringValue;

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
                    if (string.IsNullOrWhiteSpace(sif.SIFID.StringValue)) return "<blank>";
                    return sif.SIFID.StringValue;
                }
                node = node.Parent;
            }

            return null;
        }


        public bool HaveSameAttributeValues(Node node)
        {
            if (node.Attributes.Count() != Attributes.Count()) return false;

            foreach(var attribute in node.Attributes)
            {
                var myAttribute = Attributes.FirstOrDefault(x => x.Name == attribute.Name);
                if (myAttribute == null)
                    return false;

                if (string.IsNullOrEmpty(myAttribute.StringValue) && !string.IsNullOrEmpty(attribute.StringValue))
                    return false;

                if (!string.IsNullOrEmpty(myAttribute.StringValue) && string.IsNullOrEmpty(attribute.StringValue))
                    return false;

                if (!string.IsNullOrEmpty(myAttribute.StringValue) && !string.IsNullOrEmpty(attribute.StringValue) && myAttribute.StringValue != attribute.StringValue)
                    return false;

            }

            return true;
        }

    }
}
