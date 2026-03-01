using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Sintef.Apos.Sif.Model
{
    public class AttributeList<T> : IAttribute
    {
        public string Name { get; }
        public string Description { get; }
        public string StringValue { get => null; set => throw new NotSupportedException(); }
        public string RefAttributeType { get; }
        public string ItemRefAttributeType { get; }
        public bool IsMandatory { get; }
        public bool IsPartOfModel { get; }
        public bool IsOrderedList => true;
        public Node Owner { get; }
        public Collection<IAttribute> Items { get; }
        public object ObjectValue { get => null; set => throw new NotSupportedException(); }

        public AttributeList(string name, string description, string refAttributeType, string itemRefAttributeType, bool isMandatory, bool isPartOfModel, Node owner)
        {
            Name = name;
            Description = description;
            RefAttributeType = refAttributeType;
            ItemRefAttributeType = itemRefAttributeType;
            IsMandatory = isMandatory;
            IsPartOfModel = isPartOfModel;
            Owner = owner;
            Items = new Collection<IAttribute>();
        }

        public IAttribute Clone(Node owner)
        {
            return new AttributeList<T>(Name, Description, RefAttributeType, ItemRefAttributeType, IsMandatory, IsPartOfModel, owner);
        }
        public IAttribute CreateItem()
        {
            return new Attribute<T>(Name, Description, ItemRefAttributeType, true, IsPartOfModel, null);
        }

        public IEnumerable<T> Values
        {
            get => Items?.Select(x => (T)x.ObjectValue);
            set
            {
                Items?.Clear();
                foreach (var sValue in value)
                {
                    var item = CreateItem();
                    item.ObjectValue = sValue;
                    Items?.Add(item);
                }
            }
        }


        public bool IsValid(out Collection<ModelError> errors)
        {
            errors = new Collection<ModelError>();

            Validate(Owner, errors);

            return errors.Count == 0;
        }

        public void Validate(Node node, Collection<ModelError> errors)
        {
            if (Items.Count == 0)
            {
                if (IsMandatory)
                {
                    errors.Add(new ModelError(node, this, $"{Name} must have a value."));
                }

                return;
            }

            foreach (var item in Items)
            {
                item.Validate(node, errors);
            }
        }

        public bool TryGetValueOptions(out string[] valueOptions)
        {
            throw new NotImplementedException();
        }
    }
}