namespace Sintef.Apos.Sif.Model.Attributes
{
    public class EnvironmentalIntegrityLevel : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public EnvironmentalIntegrityLevel(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {
        }

        public override AttributeType Clone(Node owner)
        {
            return new EnvironmentalIntegrityLevel(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }

}
