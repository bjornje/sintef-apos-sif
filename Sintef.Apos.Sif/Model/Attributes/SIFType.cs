namespace Sintef.Apos.Sif.Model.Attributes
{
    public class SIFType : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public SIFType(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new SIFType(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
