using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.Mappers
{
    /// <summary>
    /// Provides a way to map from a <see cref="IDatabaseReader"/> to a known object type.
    /// </summary>
    /// <seealso cref="IDatabaseReaderMapper"/>
    public partial class DatabaseReaderMapper : IDatabaseReaderMapper
    {
        #region Properties
        
        /// <summary>
        /// Gets the table type descriptor used to extract the mapping information.
        /// </summary>
        protected IColumnPropertyDescriptorCollection Descriptor { get; }

        #endregion

        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="DatabaseReaderMapper"/> class.
        /// </summary>
        /// <param name="type">The type containing the mapping information, or the reference to the mapping information.</param>
        /// <exception cref="ArgumentNullException">descriptor can not be null.</exception>
        public DatabaseReaderMapper(Type type): this(new CustomTypeDescriptor(type))
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="DatabaseReaderMapper"/> class.
        /// </summary>
        /// <param name="descriptor">A column property descriptor collection to extract mapping information.</param>
        /// <exception cref="ArgumentNullException">descriptor can not be null.</exception>
        public DatabaseReaderMapper(IColumnPropertyDescriptorCollection descriptor)
        {
            this.Descriptor = descriptor ?? throw new ArgumentNullException(nameof(descriptor), $"{nameof(descriptor)} can not be null.");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a list of objects mapped from a database reader.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A list of mapped objects.</returns>
        public List<object> Map(IDatabaseReader reader)
        {
            var list = new List<object>();

            if (reader == null)
                throw new ArgumentNullException(nameof(reader), $"The {nameof(reader)} can not be null.");

            while (reader.Read())
            {
                list.Add(this.MapRow(reader));
            }

            return list;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates a new instance of the required type before mapping values.
        /// </summary>
        /// <returns></returns>
        protected virtual object CreateInstance()
        {
            var instance = Activator.CreateInstance(this.Descriptor.Type);

            if (instance == null)
                throw new Exception($"Couldn't instantiate type '{this.Descriptor.TypeName}'.");

            return instance;
        }

        /// <summary>
        /// Maps one single row from the <see cref="IDatabaseReader"/>.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A new instance of the known object type already mapped.</returns>
        /// <remarks>This method do not advance the reading cursor.</remarks>
        protected virtual object MapRow(IDatabaseReader reader)
        {
            // TODO: maybe we could add an option to allow map by index instead of name. Should be even faster.            
            var entity = Activator.CreateInstance(this.Descriptor.Type);

            foreach (var property in this.Descriptor.AllProperties)
            {
                var value = reader.GetValue(property.ColumnName);
                property.PropertyInfo.SetValue(entity, NativeTypeConverter.ConvertTo(value, property.NotNullablePropertyType));
            }

            return entity;
        }

        #endregion
    }
}