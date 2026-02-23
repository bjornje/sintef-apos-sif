namespace Sintef.Apos.Sif.Model.Attributes
{
    public class ResetAfterShutdown_FinalElement : AttributeType
    {
        public string Value { get => StringValue; set => StringValue = value; }
        public ResetAfterShutdown_FinalElement(string name, string description, string refAttributeType, bool isMandatory, Node owner) : base(name, description, refAttributeType, isMandatory, typeof(string), owner)
        {

        }

        public override AttributeType Clone(Node owner)
        {
            return new ResetAfterShutdown_FinalElement(Name, Description, RefAttributeType, IsMandatory, owner);
        }
    }
}
