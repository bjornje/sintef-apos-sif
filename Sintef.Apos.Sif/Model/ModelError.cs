namespace Sintef.Apos.Sif.Model
{
    public class ModelError
    {
        public Node Node { get; }
        public IAttribute Attribute { get; }
        public string Message { get; }

        public ModelError(Node node, string message)
        {
            Node = node;
            Message = message;
        }

        public ModelError(Node node, IAttribute attribute, string message)
        {
            Node = node;
            Attribute = attribute;
            Message = message;
        }
    }
}
