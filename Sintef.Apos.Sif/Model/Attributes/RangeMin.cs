namespace Sintef.Apos.Sif.Model.Attributes
{
    public class RangeMin : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public RangeMin(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new RangeMin(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
