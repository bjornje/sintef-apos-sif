namespace Sintef.Apos.Sif.Model.Attributes
{
    public class EnvironmentalExtremes : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public EnvironmentalExtremes(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {
        }

        public override AttributeType Clone(Node owner)
        {
            return new EnvironmentalExtremes(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
