namespace Sintef.Apos.Sif.Model.Attributes
{
    public class SILLevel : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public SILLevel(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new SILLevel(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }
}
