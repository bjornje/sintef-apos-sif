using Sintef.Apos.Sif.Model.Attributes;

namespace Sintef.Apos.Sif.Model
{
    public class ModelError
    {
        public Node Node { get; }
        public AttributeType Attribute { get; }
        public string Message { get; }

        public ModelError(Node node, string message)
        {
            Node = node;
            Message = message;
        }

        public ModelError(Node node, AttributeType attribute, string message)
        {
            Node = node;
            Attribute = attribute;
            Message = message;
        }
    }
}
