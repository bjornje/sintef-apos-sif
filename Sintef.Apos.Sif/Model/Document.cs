using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class Document
    {
        public const string RefBaseSystemUnitPath = "SIF Unit Classes/Group";


        public Document(Node parent, string name)
        {
        }

        public bool IsSameAs(Document other)
        {
            return false;
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

        public Document Append()
        {
            var group = new Document(_parent, "Document");

            _items.Add(group);
            return group;
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
    }
}
