namespace Sintef.Apos.Sif.Model
{
    public class ExternalInterface : Node
    {
        public Attribute<string> MIMEType { get; private set; }
        public Attribute<string> RefURI { get; private set; }

        public ExternalInterfaceTemplate Template { get; }

        public ExternalInterface(Node parent, ExternalInterfaceTemplate template) : base(parent, "ExternalInterface")
        {
            Template = template;

            MIMEType = new Attribute<string>("MIMEType", "Information about the document format of the file that is referenced by the URI that is stored within the attribute “refURI”", "xs:string", true, true, this);
            MIMEType.Value = "application/pdf";
            MIMEType.ValueOptions = new[] { "application/pdf", "application/msword", "application/AML" };

            RefURI = new Attribute<string>("refURI", "A URI (Uniform Resource Identifier) is a generic term used to identify resources either on the internet or a local network", "xs:anyURI", true, true, this);

        }
    }
}
