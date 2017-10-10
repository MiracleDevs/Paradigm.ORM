using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Paradigm.ORM.Data.Descriptors
{
    internal class PropertyDecoration
    {
        internal PropertyInfo PropertyInfo { get; }

        internal List<Attribute> Attributes { get; }

        public PropertyDecoration(PropertyInfo propertyInfo, List<Attribute> attributes)
        {
            this.PropertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo), $"The {nameof(propertyInfo)} can not be null.");
            this.Attributes = attributes ?? throw new ArgumentNullException(nameof(attributes), $"The {nameof(attributes)} can not be null.");
        }

        public TAttribute GetAttribute<TAttribute>() where TAttribute : Attribute
        {
            return this.Attributes.FirstOrDefault(x => x is TAttribute) as TAttribute;
        }

        public IEnumerable<TAttribute> GetAttributes<TAttribute>() where TAttribute : Attribute
        {
            return this.Attributes.Where(x => x is TAttribute).Cast<TAttribute>();
        }
    }
}