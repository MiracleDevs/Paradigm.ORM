using System;
using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.Mappers.Generic
{
    /// <summary>
    /// Provides a way to map from a <see cref="IDatabaseReader"/> to a known object type.
    /// </summary>
    /// <typeparam name="TEntity">A type containing mapping information.</typeparam>
    /// <seealso cref="DatabaseReaderMapper"/>
    /// <seealso cref="IDatabaseReaderMapper{TEntity}"/>
    public partial class DatabaseReaderMapper<TEntity> : DatabaseReaderMapper, IDatabaseReaderMapper<TEntity> where TEntity : new()
    {
        #region Constructor

        /// <summary>
        ///  Initializes a new instance of the <see cref="DatabaseReaderMapper"/> class.
        /// </summary>
        public DatabaseReaderMapper() : base(new CustomTypeDescriptor(typeof(TEntity)))
        {
        }

        /// <summary>
        ///  Initializes a new instance of the <see cref="DatabaseReaderMapper"/> class.
        /// </summary>
        /// <param name="descriptor">A column property descriptor collection to extract mapping information.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public DatabaseReaderMapper(IColumnPropertyDescriptorCollection descriptor) : base(descriptor)
        {
            if (descriptor == null)
                throw new ArgumentNullException(nameof(descriptor), $"{nameof(descriptor)} can not be null.");

            if (descriptor.Type != typeof(TEntity))
                throw new ArgumentException($"The {nameof(TableTypeDescriptor)} inner {nameof(Type)} is not {nameof(TEntity)}.");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a list of <see cref="TEntity"/> mapped from a database reader.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A list of <see cref="TEntity"/>.</returns>
        public new List<TEntity> Map(IDatabaseReader reader)
        {
            return base.Map(reader).Cast<TEntity>().ToList();
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Creates a new instance of <see cref="TEntity"/>.
        /// </summary>
        /// <returns>New instance of <see cref="TEntity"/>.</returns>
        protected override object CreateInstance()
        {
            return new TEntity();
        }

        #endregion
    }
}