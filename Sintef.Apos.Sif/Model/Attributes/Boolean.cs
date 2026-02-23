namespace Sintef.Apos.Sif.Model.Attributes
{
    public class Boolean : AttributeType
    {
        public bool? Value { get => ObjectValue as bool?; set => ObjectValue = value; }
        public Boolean(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(bool), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Boolean(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
