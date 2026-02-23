namespace Sintef.Apos.Sif.Model.Attributes
{

    public class Percent : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public Percent(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new Percent(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }
}
