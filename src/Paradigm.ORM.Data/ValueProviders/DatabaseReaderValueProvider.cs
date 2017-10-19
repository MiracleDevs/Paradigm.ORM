using System;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.ValueProviders
{
    /// <summary>
    /// Provides column values from a database reader.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.ValueProviders.IValueProvider" />
    public partial class DatabaseReaderValueProvider : IValueProvider
    {
        #region Properties

        /// <summary>
        /// Gets the database connector.
        /// </summary>
        private IDatabaseConnector Connector { get; }

        /// <summary>
        /// Gets the database value converter.
        /// </summary>
        private IValueConverter ValueConverter { get; }

        /// <summary>
        /// Gets a reference to a database reader.
        /// </summary>
        private IDatabaseReader Reader { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseReaderValueProvider"/> class.
        /// </summary>
        /// <param name="connector">A reference to a database connector.</param>
        /// <param name="reader">The reader.</param>
        /// <exception cref="ArgumentNullException">reader</exception>
        public DatabaseReaderValueProvider(IDatabaseConnector connector, IDatabaseReader reader)
        {
            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector));
            this.Reader = reader ?? throw new ArgumentNullException(nameof(reader));
            this.ValueConverter = this.Connector.GetValueConverter();
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        /// <summary>
        /// Moves the reading cursor to the next entity or row.
        /// </summary>
        /// <returns>True if can move to another row, false otherwise.</returns>
        public bool MoveNext()
        {
            return this.Reader.Read();
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the value related to the descriptor.
        /// </summary>
        /// <param name="descriptor">The descriptor.</param>
        /// <returns>
        /// Column descriptor value.
        /// </returns>
        public object GetValue(IColumnDescriptor descriptor)
        {
            return this.Reader.GetValue(descriptor.ColumnName);
        }

        #endregion
    }
}