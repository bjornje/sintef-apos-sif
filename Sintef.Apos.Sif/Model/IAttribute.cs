using System;
using System.Collections.ObjectModel;

namespace Sintef.Apos.Sif.Model
{
    public interface IAttribute
    {
        bool IsOrderedList { get; }
        bool IsMandatory { get; }
        bool IsPartOfModel { get; }
        string Name { get; }
        string Description { get; }
        string StringValue { get; set; }
        object ObjectValue { get; set; }
        string RefAttributeType { get; }
        Collection<IAttribute> Items { get; }
        Node Owner { get; }
        void Validate(Node node, Collection<ModelError> errors);
        IAttribute CreateItem();
        IAttribute Clone(Node owner);
        bool TryGetValueOptions(out string[] valueOptions);
    }
}
