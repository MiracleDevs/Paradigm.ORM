using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides the means to describe the mapping relationship between a column and a property.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Descriptors.IColumnPropertyDescriptor" />
    internal class ColumnPropertyDescriptor : IColumnPropertyDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the property decoration.
        /// </summary>
        private PropertyDecoration PropertyDecoration { get; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public virtual string ColumnName { get; private set; }

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        public virtual string PropertyName { get; private set; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        public virtual string DataType { get; private set; }

        /// <summary>
        /// Gets the maximum size.
        /// </summary>
        public virtual long MaxSize { get; private set; }

        /// <summary>
        /// Gets the numeric precision.
        /// </summary>
        public virtual byte Precision { get; private set; }

        /// <summary>
        /// Gets the numeric scale.
        /// </summary>
        public virtual byte Scale { get; private set; }

        /// <summary>
        /// Indicates if the column is part of a primary key.
        /// </summary>
        public virtual bool IsPrimaryKey { get; private set; }

        /// <summary>
        /// Indicates if the column is an identity.
        /// </summary>
        public virtual bool IsIdentity { get; private set; }

        /// <summary>
        /// Indicates if the column is part of a foreign key.
        /// </summary>
        public virtual bool IsForeignKey { get; private set; }

        /// <summary>
        /// Indicates if the column is part of a unique key.
        /// </summary>
        public virtual bool IsUniqueKey { get; private set; }

        /// <summary>
        /// Gets the property type.
        /// </summary>
        public Type PropertyType { get; }

        /// <summary>
        /// Gets the inner type of the property if the property is nullable; will be the same as <see cref="P:Paradigm.ORM.Data.Descriptors.IColumnPropertyDescriptor.Type" /> if Type is not nullable.
        /// </summary>
        public Type NotNullablePropertyType { get; }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="property">The property information.</param>
        private ColumnPropertyDescriptor(PropertyDecoration property)
        {
            this.PropertyDecoration = property;
            this.PropertyInfo = property.PropertyInfo;
            this.PropertyType = this.PropertyInfo.PropertyType;
            this.NotNullablePropertyType = Nullable.GetUnderlyingType(this.PropertyType) ?? this.PropertyType;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of <see cref="ColumnPropertyDescriptor"/> from a <see cref="PropertyDecoration"/> instance.
        /// </summary>
        /// <remarks>
        /// If the property is not decorated with the <see cref="ColumnAttribute"/> the method will return <c>null</c>.
        /// </remarks>
        /// <param name="property">The property info to use.</param>
        /// <returns>A column property descriptor or null otherwise.</returns>
        internal static IColumnPropertyDescriptor Create(PropertyDecoration property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property), "The property collection can not be null.");

            var descriptor = new ColumnPropertyDescriptor(property);
            return descriptor.Initialize() ? descriptor : null;
        }

        /// <summary>
        /// Creates a list of <see cref="ColumnPropertyDescriptor"/> from a collection of <see cref="PropertyDecoration"/>.
        /// </summary>
        /// <remarks>
        /// This method will trim all null appearances if any.
        /// <see cref="Create(Descriptors.PropertyDecoration)"/>
        /// </remarks>
        /// <param name="properties">Collection of property info.</param>
        /// <returns>A list of <see cref="ColumnPropertyDescriptor"/></returns>
        internal static List<IColumnPropertyDescriptor> Create(IEnumerable<PropertyDecoration> properties)
        {
            if (properties == null)
                throw new ArgumentNullException(nameof(properties), "The property collection can not be null.");

            return properties.Select(Create).Where(x => x != null).ToList();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Column Property Descriptor [{this.PropertyName}]";

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the column property descriptor.
        /// </summary>
        /// <returns><c>true</c> if the initialization was successfull; <c>false</c> otherwise.</returns>
        private bool Initialize()
        {
            var columnAttribute = this.PropertyDecoration.GetAttribute<ColumnAttribute>();

            if (columnAttribute == null)
                return false;

            this.ColumnName = columnAttribute.Name ?? this.PropertyInfo.Name;

            this.DataType = columnAttribute.Type ?? (this.PropertyInfo.PropertyType == typeof(Nullable<>)
                ? this.PropertyInfo.PropertyType.GetGenericArguments().First().Name
                : this.PropertyInfo.PropertyType.Name);

            this.PropertyName = this.PropertyInfo.Name;

            var sizeAttribute = this.PropertyDecoration.GetAttribute<SizeAttribute>();
            var numericAttribute = this.PropertyDecoration.GetAttribute<NumericAttribute>();

            if (sizeAttribute != null)
            {
                this.MaxSize = sizeAttribute.MaxSize;
            }

            if (numericAttribute != null)
            {
                this.Precision = numericAttribute.Precision;
                this.Scale = numericAttribute.Scale;
            }

            this.IsIdentity = this.PropertyDecoration.GetAttribute<IdentityAttribute>() != null;
            this.IsPrimaryKey = this.PropertyDecoration.GetAttribute<PrimaryKeyAttribute>() != null;
            this.IsForeignKey = this.PropertyDecoration.GetAttribute<ForeignKeyAttribute>() != null;
            this.IsUniqueKey = this.PropertyDecoration.GetAttribute<UniqueKeyAttribute>() != null;

            return true;
        }

        #endregion
    }
}