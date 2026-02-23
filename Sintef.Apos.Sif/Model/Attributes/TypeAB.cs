namespace Sintef.Apos.Sif.Model.Attributes
{
    public class TypeAB : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public TypeAB(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new TypeAB(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
