namespace Sintef.Apos.Sif.Model.Attributes
{
    public class Integer : AttributeType
    {
        public int? Value { get => ObjectValue as int?; set => ObjectValue = value; }
        public Integer(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(long), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Integer(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
