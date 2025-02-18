using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class Node
    {
        public Node Parent { get; }
        public IEnumerable<AttributeType> Attributes => _attributes ?? Enumerable.Empty<AttributeType>();
        public string PathStepX { get; }

        public virtual string GetPathStep()
        {
            return GetType().Name;
        }

        public virtual int GetNumberOfElements()
        {
            return 0;
        }

        private Collection<AttributeType> _attributes;
        public Node(Node parent, string pathStep)
        {
            Parent = parent;
            PathStepX = pathStep;
        }

        protected void SetAttributes(Collection<AttributeType> attributes)
        {
            _attributes = attributes;
        }

        public string Path => GetPath(null);

        public string GetPath(Node stopAtNode)
        {
            string path = "";
            var node = this;

            while (node != null && node != stopAtNode)
            {
                path = $"/{node.GetPathStep()}{path}";
                node = node.Parent;
            }

            return path.TrimStart('/');
        }

        public string GetSIFID()
        {
            var node = this;

            while (node != null)
            {
                if (node is SIF sif)
                {
                    if (string.IsNullOrWhiteSpace(sif.SIFID.Value)) return "<blank>";
                    return sif.SIFID.Value;
                }
                node = node.Parent;
            }

            return null;
        }

        public SIF GetSIF()
        {
            var node = this;

            while (node != null)
            {
                if (node is SIF sif) return sif;
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
