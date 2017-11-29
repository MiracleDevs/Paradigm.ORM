using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides the means to describe the mapping relationship between a table and a .NET type.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Descriptors.ITableTypeDescriptor" />
    public class TableTypeDescriptor : ITableTypeDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets or sets the default primary key values.
        /// </summary>
        /// <value>
        /// The default primary key values.
        /// </value>
        private List<object> DefaultPrimaryKeyValues { get; set; }

        /// <summary>
        /// Gets the mapping type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets a list of column property descriptors for all the properties.
        /// </summary>
        public virtual List<IColumnPropertyDescriptor> AllProperties { get; private set; }

        /// <summary>
        /// Gets a list of column property descriptors for all the properties.
        /// </summary>
        public virtual List<IColumnDescriptor> AllColumns => this.AllProperties.Cast<IColumnDescriptor>().ToList();

        /// <summary>
        /// Gets a list of navigation property descriptors for all the navigation properties.
        /// </summary>
        public virtual List<INavigationPropertyDescriptor> NavigationProperties { get; private set; }

        /// <summary>
        /// Gets a list of column property descriptors for all the simple properties.
        /// </summary>
        /// <remarks>
        /// Simple properties does not include the identity properties but will contain
        /// the primary keys.
        /// </remarks>
        public virtual List<IColumnPropertyDescriptor> SimpleProperties { get; }

        /// <summary>
        /// Gets a list of column descriptors for all the simple columns.
        /// </summary>
        /// <remarks>
        /// Simple columns does not include the identity columns but will contain
        /// the primary keys.
        /// </remarks>
        public virtual List<IColumnDescriptor> SimpleColumns => this.SimpleProperties.Cast<IColumnDescriptor>().ToList();

        /// <summary>
        /// Gets a list of column property descriptors for all the primary keys.
        /// </summary>
        public virtual List<IColumnPropertyDescriptor> PrimaryKeyProperties { get; }

        /// <summary>
        /// Gets a list of column descriptors for all the primary keys.
        /// </summary>
        public virtual List<IColumnDescriptor> PrimaryKeyColumns => this.PrimaryKeyProperties.Cast<IColumnDescriptor>().ToList();

        /// <summary>
        /// Gets the identity column property descriptor.
        /// </summary>
        public virtual IColumnPropertyDescriptor IdentityProperty { get; private set; }

        /// <summary>
        /// Gets the identity column descriptor.
        /// </summary>
        public virtual IColumnDescriptor IdentityColumn => IdentityProperty;

        /// <summary>
        /// Gets the name of the database catalog.
        /// </summary>
        public virtual string CatalogName { get; private set; }

        /// <summary>
        /// Gets the name of the database schema.
        /// </summary>
        public virtual string SchemaName { get; private set; }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public virtual string TableName { get; private set; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public virtual string TypeName { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TableTypeDescriptor"/> class.
        /// </summary>
        /// <param name="type">The type containing the mapping information, or the reference to the mapping information.</param>
        /// <seealso cref="TableAttribute"/>
        /// <seealso cref="TableTypeAttribute"/>
        internal TableTypeDescriptor(Type type)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type), $"The {nameof(type)} can not be null.");
            this.AllProperties = new List<IColumnPropertyDescriptor>();
            this.PrimaryKeyProperties = new List<IColumnPropertyDescriptor>();
            this.SimpleProperties = new List<IColumnPropertyDescriptor>();
            this.NavigationProperties = new List<INavigationPropertyDescriptor>();
            this.DefaultPrimaryKeyValues = new List<object>();

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
        public override string ToString() => $"Table Type Descriptor [{this.TypeName}]";

        /// <summary>
        /// Determines whether the specified entity is new.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// <c>true</c> if the specified entity is new; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNew(object entity)
        {
            return this.PrimaryKeyProperties.Where((t, i) => this.DefaultPrimaryKeyValues[i].Equals(t.PropertyInfo.GetValue(entity))).Any();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the table type descriptor.
        /// </summary>
        private void Initialize()
        {
            var typeInfo = this.Type.GetTypeInfo();
            var tableType = typeInfo.GetCustomAttribute<TableTypeAttribute>()?.Type ?? this.Type;

            if (tableType != this.Type)
                typeInfo = tableType.GetTypeInfo();

            var tableAttribute = typeInfo.GetCustomAttribute<TableAttribute>();

            if (tableAttribute == null)
                throw new OrmMissingTableMappingException($"The type '{this.Type.Name}' does not have table mapping information.");

            this.TypeName = this.Type.Name;
            this.CatalogName = tableAttribute.Catalog;
            this.SchemaName = tableAttribute.Schema;
            this.TableName = tableAttribute.Name ?? tableType.Name;

            var properties = this.GetProperties(this.Type, tableType);

            this.AllProperties = ColumnPropertyDescriptor.Create(properties);
            this.NavigationProperties = NavigationPropertyDescriptor.Create(this, properties);

            foreach (var property in this.AllProperties)
            {
                if (property.IsPrimaryKey)
                    this.PrimaryKeyProperties.Add(property);

                if (property.IsIdentity)
                    this.IdentityProperty = property;
                else
                    this.SimpleProperties.Add(property);
            }

            if (this.PrimaryKeyProperties.Any())
            {
                this.DefaultPrimaryKeyValues = this.PrimaryKeyProperties.Select(x => x.PropertyType.GetDefaultValue()).ToList();
            }
        }

        /// <summary>
        /// Gets a list of properties with its custom attributes.
        /// </summary>
        /// <param name="type">The <see cref="TableTypeDescriptor"/> type.</param>
        /// <param name="tableType">An optional type being referenced by the <see cref="TableTypeDescriptor"/> type using the <see cref="TableTypeAttribute"/>.</param>
        /// <returns>A list of properties and its attriubtes.</returns>
        private List<PropertyDecoration> GetProperties(Type type, Type tableType)
        {
            var properties = new Dictionary<string, PropertyDecoration>();

            // get parent types and referenced types.
            var parentTypes = type.GetParentTypes();

            if (!parentTypes.Contains(type))
                parentTypes.Add(type);

            // if the main type references another type using the TableTypeAttribute,
            // merges both in one hashset to avoid repeated types.
            if (tableType != null && tableType != type)
            {
                if (!parentTypes.Contains(tableType))
                    parentTypes.Add(tableType);

                var parentTableTypes = tableType.GetParentTypes();

                foreach (var parentTableType in parentTableTypes)
                {
                    if (!parentTypes.Contains(parentTableType))
                    {
                        parentTypes.Add(parentTableType);
                    }
                }
            }

            // get a list of all properties in all the types.
            var allProperties = parentTypes.Select(x => x.GetTypeInfo()).SelectMany(x => x.DeclaredProperties).ToList();

            // for all the properties in the main type and all the
            // inherited properties, try to obtain the property info
            // and all the related decorations by name.
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();

                foreach (var property in typeInfo.DeclaredProperties)
                {
                    if (properties.ContainsKey(property.Name))
                        continue;

                    properties.Add(property.Name, new PropertyDecoration(
                        property,
                        allProperties.Where(p => p.Name == property.Name).SelectMany(p => p.GetCustomAttributes()).ToList()
                    ));
                }

                type = typeInfo.BaseType;
            }

            return properties.Values.ToList();
        }

        #endregion
    }
}