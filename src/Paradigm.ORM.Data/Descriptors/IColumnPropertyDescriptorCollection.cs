using System;
using System.Collections.Generic;

namespace Paradigm.ORM.Data.Descriptors
{
    public interface IColumnPropertyDescriptorCollection
    {
        /// <summary>
        /// Gets the mapping type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Gets a list of column property descriptors for all the properties.
        /// </summary>
        List<IColumnPropertyDescriptor> AllProperties { get; }
    }
}