namespace Sintef.Apos.Sif.Model.Attributes
{
    public class TagName : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public TagName(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new TagName(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
