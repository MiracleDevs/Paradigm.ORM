using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.DataExport.Export
{
    /// <summary>
    /// Provides the means to describe a database table.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Descriptors.ITableTypeDescriptor" />
    public class CustomTableDescriptor : ITableDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the table schema.
        /// </summary>
        private ITable Table { get; }

        /// <summary>
        /// Gets the columns schema.
        /// </summary>
        private List<IColumn> Columns { get; }

        /// <summary>
        /// Gets the constraints schema.
        /// </summary>
        private List<IConstraint> Constraints { get; }

        /// <summary>
        /// Gets a list of column descriptors for all the simple columns.
        /// </summary>
        /// <remarks>
        /// Simple columns does not include the identity columns but will contain
        /// the primary keys.
        /// </remarks>
        public virtual List<IColumnDescriptor> SimpleColumns { get; private set; }

        /// <summary>
        /// Gets a list of all the columns.
        /// </summary>
        public virtual List<IColumnDescriptor> AllColumns { get; private set; }

        /// <summary>
        /// Gets a list of column descriptors for all the primary keys.
        /// </summary>
        public virtual List<IColumnDescriptor> PrimaryKeyColumns { get; private set; }

        /// <summary>
        /// Gets the identity column descriptor.
        /// </summary>
        public virtual IColumnDescriptor IdentityColumn { get; private set; }

        /// <summary>
        /// Gets the name of the database catalog.
        /// </summary>
        public virtual string CatalogName => null;

        /// <summary>
        /// Gets the name of the database schema.
        /// </summary>
        public virtual string SchemaName => null;

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public virtual string TableName => this.Table.Name;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TableTypeDescriptor"/> class.
        /// </summary>
        /// <param name="table">The table schema.</param>
        /// <param name="columns">The columns schema.</param>
        /// <param name="constraints">The constraints schema.</param>
        /// <seealso cref="TableAttribute"/>
        /// <seealso cref="TableTypeAttribute"/>
        internal CustomTableDescriptor(ITable table, List<IColumn> columns, List<IConstraint> constraints)
        {
            this.Table = table;
            this.Columns = columns;
            this.Constraints = constraints;

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
        public override string ToString() => $"Table Descriptor [{this.TableName}]";

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes the table type descriptor.
        /// </summary>
        private void Initialize()
        {
            this.AllColumns = this.Columns.Select(c => new CustomColumnDescriptor(c, this.Constraints.Where(x => x.FromColumnName == c.Name))).Cast<IColumnDescriptor>().ToList();
            this.SimpleColumns = this.AllColumns.Where(x => !x.IsIdentity).ToList();
            this.PrimaryKeyColumns = this.AllColumns.Where(x => x.IsPrimaryKey).ToList();
            this.IdentityColumn = this.AllColumns.FirstOrDefault(x => x.IsIdentity);
        }

        #endregion
    }
}