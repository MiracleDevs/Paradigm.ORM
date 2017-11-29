using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides the means to describe the mapping relationship between a custom query and a .NET type.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Descriptors.ITableTypeDescriptor" />
    public class CustomTypeDescriptor : ICustomTypeDescriptor
    {
        #region Properties

        public Type Type { get; }

        public virtual string TypeName { get; }

        public virtual List<IColumnPropertyDescriptor> AllProperties { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTypeDescriptor"/> class.
        /// </summary>
        /// <param name="type">The type containing the mapping information, or the reference to the mapping information.</param>
        internal CustomTypeDescriptor(Type type)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type), $"The {nameof(type)} can not be null.");
            this.TypeName = this.Type.Name;
            this.AllProperties = new List<IColumnPropertyDescriptor>();

            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Custom Type Descriptor [{this.TypeName}]";

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the table type descriptor.
        /// </summary>
        private void Initialize()
        {
            var typeInfo = this.Type.GetTypeInfo();
            this.AllProperties = ColumnPropertyDescriptor.Create(typeInfo.DeclaredProperties.Select(x => new PropertyDecoration(x, x.GetCustomAttributes().ToList())));
        }

        #endregion
    }
}