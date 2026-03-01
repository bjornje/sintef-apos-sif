using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class Document : Node
    {
        public Attribute<string> Name { get; }
        public Attribute<string> Info { get; }
        public Attribute<string> Info2 { get; }
        public Attribute<string> Language { get; }
        public Attribute<string> MIMEType => ExternalInterface.MIMEType;
        public Attribute<string> RefURI => ExternalInterface.RefURI;

        public ExternalInterface ExternalInterface { get; }

        public Document(Node parent, string name, ExternalInterfaceTemplate template) : base(parent, name)
        {
            Name = new Attribute<string>("Name", "The name of the document", "xs:string", true, true, this);
            Info = new Attribute<string>("ExternalInterface", template.RefBaseClassPath, "info", false, true, this);
            Info.Value = template.Name;
            Info2 = new Attribute<string>("RoleRquirements", template.RefBaseRoleClassPath, "info", false, true, this);
            Info2.Value = template.RoleRequirements;

            ExternalInterface = new ExternalInterface(this,  template);

            Language = new Attribute<string>("aml-DocLang", "The language of the document", "xs:string", true, true, this);
            Language.ValueOptions = new[] { "en-US", "de-DE" };

            var attributes = new Collection<IAttribute>
            {
                Name, Info, Info2, Language, MIMEType, RefURI
            };

            SetAttributes(attributes);
        }

        public bool IsSameAs(Document other)
        {
            return Name.Value == other.Name.Value;
        }


        public void Validate(Collection<ModelError> errors)
        {
            foreach (var property in Attributes)
            {
                property.Validate(this, errors);
            }
        }
    }

    public class Documents : Node, IEnumerable<Document>
    {
        private readonly Collection<Document> _items = new Collection<Document>();
        private readonly Node _parent;
        public Documents(Node parent) : base(parent, "Documents")
        {
            _parent = parent;
        }

        public Document Append(string name = null)
        {
            const string externalInterfaceName = "DocumentLink";
            if (!Definition.TryGetExternalInterface(externalInterfaceName, out var externalInterface))
            {
                throw new Exception($"External interface for {externalInterfaceName}");
            }

            var document = new Document(this, name, externalInterface);

            document.Name.StringValue = name ?? "New Document";
            document.Language.StringValue = "en-US";

            _items.Add(document);
            return document;
        }

        public bool Remove(Document item)
        {
            return _items.Remove(item);
        }

        public IEnumerator<Document> GetEnumerator()
        {
            return _items.GetEnumerator();
    
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        public bool IsSameAs(Documents documents)
        {
            if (_items.Count != documents.Count()) return false;

            var alreadyMatchedGroups = new List<Document>();

            foreach (var document in documents)
            {
                var myDocument = _items.FirstOrDefault(x => !alreadyMatchedGroups.Contains(x) && x.IsSameAs(document));
                if (myDocument == null) return false;
                alreadyMatchedGroups.Add(myDocument);
            }

            return true;
        }

        public void Validate(Collection<ModelError> errors)
        {
            foreach (var item in _items)
            {
                item.Validate(errors);
            }
        }

    }
}
