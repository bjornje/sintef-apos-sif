namespace Sintef.Apos.Sif.Model.Attributes
{

    public class ModeOfOperation : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public ModeOfOperation(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new ModeOfOperation(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }
}
