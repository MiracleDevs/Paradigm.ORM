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
    /// Provides the means to describe the mapping relationship between a routine and a .NET type.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Descriptors.IRoutineTypeDescriptor" />
    public class RoutineTypeDescriptor : IRoutineTypeDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the mapping type.
        /// </summary>
        public virtual Type Type { get; }

        /// <summary>
        /// Gets a list of column parameter descriptors for all the parameters.
        /// </summary>
        public virtual List<IParameterPropertyDescriptor> Parameters { get; private set; }

        /// <summary>
        /// Gets the name of the database catalog.
        /// </summary>
        public virtual string CatalogName { get; private set; }

        /// <summary>
        /// Gets the name of the database schema.
        /// </summary>
        public virtual string SchemaName { get; private set; }

        /// <summary>
        /// Gets the name of the routine.
        /// </summary>
        public virtual string RoutineName { get; private set; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        public virtual string TypeName { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineTypeDescriptor"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        public RoutineTypeDescriptor(Type type)
        {
            this.Type = type ?? throw new ArgumentNullException(nameof(type), $"The {nameof(type)} can not be null.");
            this.Parameters = new List<IParameterPropertyDescriptor>();
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
        public override string ToString() => $"Routine Type Descriptor [{this.TypeName}]";

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the routine type descriptor.
        /// </summary>
        private void Initialize()
        {
            var typeInfo = this.Type.GetTypeInfo();
            var routineType = typeInfo.GetCustomAttribute<RoutineTypeAttribute>()?.Type ?? this.Type;

            if (routineType != this.Type)
                typeInfo = routineType.GetTypeInfo();

            var routineAttribute = typeInfo.GetCustomAttribute<RoutineAttribute>();

            if (routineAttribute == null)
                throw new OrmMissingRoutineMappingException($"The type '{routineType.Name}' does not have routine mapping information.");

            this.TypeName = this.Type.Name;
            this.CatalogName = routineAttribute.Catalog;
            this.SchemaName = routineAttribute.Schema;
            this.RoutineName = routineAttribute.Name ?? routineType.Name;

            var properties = this.GetProperties(this.Type, routineType);
            this.Parameters = ParameterPropertyDescriptor.Create(properties);
        }

        /// <summary>
        /// Gets a list of properties with its custom attributes.
        /// </summary>
        /// <param name="type">The <see cref="RoutineTypeDescriptor"/> type.</param>
        /// <param name="routineType">An optional type being referenced by the <see cref="RoutineTypeDescriptor"/> type using the <see cref="RoutineTypeAttribute"/>.</param>
        /// <returns>A list of properties and its attriubtes.</returns>
        private List<PropertyDecoration> GetProperties(Type type, Type routineType)
        {
            var properties = new Dictionary<string, PropertyDecoration>();

            // get parent types and referenced types.
            var parentTypes = type.GetParentTypes();

            if (!parentTypes.Contains(type))
                parentTypes.Add(type);

            // if the main type references another type using the RoutineTypeAttribute, 
            // merges both in one hashset to avoid repeated types.
            if (routineType != null && routineType != type)
            {
                if (!parentTypes.Contains(routineType))
                    parentTypes.Add(routineType);

                var parentRoutineTypes = routineType.GetParentTypes();

                foreach (var parentRoutineType in parentRoutineTypes)
                {
                    if (!parentTypes.Contains(parentRoutineType))
                    {
                        parentTypes.Add(parentRoutineType);
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