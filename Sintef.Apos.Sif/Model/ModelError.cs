using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sintef.Apos.Sif.Model
{
    public class ModelError
    {
        public Node Node { get; }
        public string Message { get; }

        public ModelError(Node node, string message)
        {
            Node = node;
            Message = message;
        }
    }
}
