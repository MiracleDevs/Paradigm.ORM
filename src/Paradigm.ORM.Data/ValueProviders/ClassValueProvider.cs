using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.Data.ValueProviders
{
    /// <summary>
    /// Provides property values from objects.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.ValueProviders.IValueProvider" />
    public partial class ClassValueProvider : IValueProvider
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
        /// Gets the entities.
        /// </summary>
        public List<object> Entities { get; }

        /// <summary>
        /// Gets or sets the current reading index.
        /// </summary>
        public int CurrentIndex { get; private set; }

        /// <summary>
        /// Gets or sets the current entity.
        /// </summary>
        public object CurrentEntity { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassValueProvider"/> class.
        /// </summary>
        /// <param name="connector">A reference to a database connector.</param>
        /// <param name="entities">The entities.</param>
        /// <exception cref="ArgumentNullException">entities</exception>
        public ClassValueProvider(IDatabaseConnector connector, List<object> entities)
        {
            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector));
            this.Entities = entities ?? throw new ArgumentNullException(nameof(entities));
            this.ValueConverter = this.Connector.GetValueConverter();
            this.CurrentIndex = -1;
        }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        /// <summary>
        /// Moves the reading cursor to the next entity or row.
        /// </summary>
        /// <returns>True if there are more entities to read, false otherwise.</returns>
        public bool MoveNext()
        {
            this.CurrentIndex++;

            if (this.CurrentIndex < this.Entities.Count)
            {
                this.CurrentEntity = this.Entities[this.CurrentIndex];
                return true;
            }

            this.CurrentEntity = null;
            return false;
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
            return this.CurrentEntity == null
                ? null
                : this.ValueConverter.ConvertFrom((descriptor as IColumnPropertyDescriptor)?.PropertyInfo.GetValue(this.CurrentEntity), descriptor.DataType);
        }

        #endregion
    }
}