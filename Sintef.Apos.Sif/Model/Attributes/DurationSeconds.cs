namespace Sintef.Apos.Sif.Model.Attributes
{
    public class DurationSeconds : AttributeType
    {
        public double? Value { get => ObjectValue as double?; set => ObjectValue = value; }
        public DurationSeconds(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(decimal), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new DurationSeconds(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }
}
