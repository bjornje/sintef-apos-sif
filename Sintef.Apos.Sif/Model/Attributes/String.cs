namespace Sintef.Apos.Sif.Model.Attributes
{
    public class String : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public String(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {
        }

        public override AttributeType Clone(Node owner)
        {
            return new String(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
