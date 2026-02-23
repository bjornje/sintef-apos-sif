namespace Sintef.Apos.Sif.Model.Attributes
{
    public class RangeMax : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public RangeMax(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new RangeMax(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }
}
