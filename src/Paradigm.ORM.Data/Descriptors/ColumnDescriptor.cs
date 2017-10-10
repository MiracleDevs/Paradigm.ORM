using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides the means to describe a database column.
    /// </summary>
    /// <seealso cref="IColumnDescriptor" />
    internal class ColumnDescriptor : IColumnDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the column schema.
        /// </summary>
        private IColumn Column { get; }

        /// <summary>
        /// Get the constraint schema.
        /// </summary>
        private List<IConstraint> Constraints { get; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        public virtual string ColumnName => this.Column.Name;


        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        public virtual string DataType => this.Column.DataType;

        /// <summary>
        /// Gets the maximum size.
        /// </summary>
        public virtual long MaxSize => this.Column.MaxSize;

        /// <summary>
        /// Gets the numeric precision.
        /// </summary>
        public virtual byte Precision => this.Column.Precision;

        /// <summary>
        /// Gets the numeric scale.
        /// </summary>
        public virtual byte Scale => this.Column.Scale;

        /// <summary>
        /// Indicates if the column is an identity.
        /// </summary>
        public virtual bool IsIdentity => this.Column.IsIdentity;

        /// <summary>
        /// Indicates if the column is part of a primary key.
        /// </summary>
        public virtual bool IsPrimaryKey { get; private set; }

        /// <summary>
        /// Indicates if the column is part of a foreign key.
        /// </summary>
        public virtual bool IsForeignKey { get; private set; }

        /// <summary>
        /// Indicates if the column is part of a unique key.
        /// </summary>
        public virtual bool IsUniqueKey { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ColumnDescriptor"/> class.
        /// </summary>
        /// <param name="column">The column schema.</param>
        /// <param name="constraints">The constraints schema.</param>
        internal ColumnDescriptor(IColumn column, IEnumerable<IConstraint> constraints)
        {
            this.Column = column;
            this.Constraints = constraints.ToList();
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
        public override string ToString() => $"Column Descriptor [{this.ColumnName}]";

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the column descriptor.
        /// </summary>
        /// <returns><c>true</c> if the initialization was successfull; <c>false</c> otherwise.</returns>
        private void Initialize()
        {
            this.IsPrimaryKey = this.Constraints.Any(x => x.Type == ConstraintType.PrimaryKey);
            this.IsForeignKey = this.Constraints.Any(x => x.Type == ConstraintType.ForeignKey);
            this.IsUniqueKey = this.Constraints.Any(x => x.Type == ConstraintType.UniqueKey);
        }

        #endregion
    }
}