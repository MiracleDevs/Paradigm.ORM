using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides the means to describe the mapping relationship between a parameter and a property.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Descriptors.IColumnPropertyDescriptor" />
    internal class ParameterPropertyDescriptor : IParameterPropertyDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the property decoration.
        /// </summary>
        private PropertyDecoration PropertyDecoration { get; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public string ParameterName { get; private set; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        public string PropertyName { get; private set; }

        /// <summary>
        /// Gets the maximum size.
        /// </summary>
        public long MaxSize { get; private set; }

        /// <summary>
        /// Gets the numeric precision.
        /// </summary>
        public byte Precision { get; private set; }

        /// <summary>
        /// Gets the numeric scale.
        /// </summary>
        public byte Scale { get; private set; }

        /// <summary>
        /// Gets the property type.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Gets the inner type of the property if the property is nullable; will be the same as <see cref="P:Paradigm.ORM.Data.Descriptors.IParameterPropertyDescriptor.Type" /> if Type is not nullable.
        /// </summary>
        public Type NotNullableType { get; }

        /// <summary>
        /// Gets the property information.
        /// </summary>
        public PropertyInfo PropertyInfo { get; }

        /// <summary>
        /// Gets a value indicating whether this property is input or output property.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this property is an input property; otherwise, <c>false</c>.
        /// </value>
        public bool IsInput { get; private set; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        public string DataType { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ParameterPropertyDescriptor"/> class.
        /// </summary>
        /// <param name="property">The property information.</param>
        internal ParameterPropertyDescriptor(PropertyDecoration property)
        {
            this.PropertyDecoration = property;
            this.PropertyInfo = property.PropertyInfo;
            this.Type = this.PropertyInfo.PropertyType;
            this.NotNullableType = Nullable.GetUnderlyingType(this.Type) ?? this.Type;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Creates a new instance of <see cref="ParameterPropertyDescriptor"/> from a <see cref="PropertyDecoration"/> instance.
        /// </summary>
        /// <remarks>
        /// If the property is not decorated with the <see cref="ParameterAttribute"/> the method will return <c>null</c>.
        /// </remarks>
        /// <param name="property">The property info to use.</param>
        /// <returns>A Parameter property descriptor or null otherwise.</returns>
        internal static IParameterPropertyDescriptor Create(PropertyDecoration property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property), "The property collection can not be null.");

            var descriptor = new ParameterPropertyDescriptor(property);
            return descriptor.Initialize() ? descriptor : null;
        }

        /// <summary>
        /// Creates a list of <see cref="ParameterPropertyDescriptor"/> from a collection of <see cref="PropertyDecoration"/>.
        /// </summary>
        /// <remarks>
        /// This method will trim all null appearances if any.
        /// <see cref="Create(Descriptors.PropertyDecoration)"/>
        /// </remarks>
        /// <param name="properties">Collection of property info.</param>
        /// <returns>A list of <see cref="ParameterPropertyDescriptor"/></returns>
        internal static List<IParameterPropertyDescriptor> Create(IEnumerable<PropertyDecoration> properties)
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
        public override string ToString() => $"Parameter Property Descriptor [{this.PropertyName}]";

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the parameter property descriptor.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the initialization was successfull; <c>false</c> otherwise.
        /// </returns>
        private bool Initialize()
        {
            var parameterAttribute = this.PropertyDecoration.GetAttribute<ParameterAttribute>();

            if (parameterAttribute == null)
                return false;

            this.ParameterName = parameterAttribute.Name ?? this.PropertyInfo.Name;
            this.DataType = parameterAttribute.Type ?? this.PropertyInfo.DeclaringType.Name;
            this.IsInput = parameterAttribute.IsInput;
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

            return true;
        }

        #endregion
    }
}