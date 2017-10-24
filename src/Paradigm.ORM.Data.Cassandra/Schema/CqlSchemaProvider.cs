using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.Cassandra.Converters;
using Paradigm.ORM.Data.Cassandra.Schema.Structure;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Database.Schema;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.Data.Querying;

namespace Paradigm.ORM.Data.Cassandra.Schema
{
    /// <summary>
    /// Provides a way to retrieve schema information from the database.
    /// </summary>
    public partial class CqlSchemaProvider : ISchemaProvider
    {
        #region String Constants        

        /// <summary>
        /// Gets the table type name.
        /// </summary>
        private const string TableType = "Standard";

        /// <summary>
        /// Gets the view type name.
        /// </summary>
        private const string ViewType = "View";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command format provider.
        /// </summary>
        private ICommandFormatProvider FormatProvider { get; }

        /// <summary>
        /// Gets or sets the column query.
        /// </summary>
        private Query<CqlColumn> ColumnQuery { get; set; }

        /// <summary>
        /// Gets or sets the constraint query.
        /// </summary>
        private Query<CqlConstraint> ConstraintQuery { get; set; }


        /// <summary>
        /// Gets or sets the view query.
        /// </summary>
        private Query<CqlView> ViewQuery { get; set; }

        /// <summary>
        /// Gets or sets the table query.
        /// </summary>
        private Query<CqlTable> TableQuery { get; set; }

        #endregion

        #region Constructor        

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlSchemaProvider"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public CqlSchemaProvider(IDatabaseConnector connector)
        {
            this.FormatProvider = connector.GetCommandFormatProvider();

            this.ConstraintQuery = new Query<CqlConstraint>(connector);
            this.ColumnQuery = new Query<CqlColumn>(connector);
            this.ViewQuery = new Query<CqlView>(connector);
            this.TableQuery = new Query<CqlTable>(connector);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.ConstraintQuery?.Dispose();
            this.ColumnQuery?.Dispose();
            this.ViewQuery?.Dispose();
            this.TableQuery?.Dispose();

            this.ConstraintQuery = null;
            this.ColumnQuery = null;
            this.ViewQuery = null;
            this.TableQuery = null;
        }

        /// <summary>
        /// Gets the schema of database tables specifying the database, and allowing to filter which tables to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of table names you want to retrieve.</param>
        /// <returns>
        /// A list of table schemas.
        /// </returns>
        public List<ITable> GetTables(string database, params string[] filter)
        {
            return this.TableQuery
                .Execute(this.GetTableWhere(database, TableType, filter))
                .Where(x => x.Type == "Standard" && (filter == null || filter.Length == 0 || filter.Contains(x.Name)))
                .Cast<ITable>().ToList();
        }

        /// <summary>
        /// Gets the schema of database views specifying the database, and allowing to filter which views to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of view names you want to retrieve.</param>
        /// <returns>
        /// A list of view schemas.
        /// </returns>
        public List<IView> GetViews(string database, params string[] filter)
        {
            return this.ViewQuery
                .Execute(this.GetTableWhere(database, ViewType, filter))
                .Where(x => x.Type == "View" && (filter == null || filter.Length == 0 || filter.Contains(x.Name)))
                .Cast<IView>().ToList();
        }

        /// <summary>
        /// Gets the schema of stored procedures specifying the database, and allowing to filter which stored procedures to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of stored procedure names you want to retrieve.</param>
        /// <returns>
        /// A list of stored procedure schemas.
        /// </returns>
        public List<IStoredProcedure> GetStoredProcedures(string database, params string[] filter)
        {
            return new List<IStoredProcedure>();
        }

        /// <summary>
        /// Gets the schema of all the columns of a table.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>
        /// A list of column schemas.
        /// </returns>
        public List<IColumn> GetColumns(string database, string tableName)
        {
            var columns = this.ColumnQuery.Execute($"\"keyspace_name\"='{database}' AND \"columnfamily_name\"='{tableName}'").ToList();

            foreach(var column in columns)
            {
                column.DataType = CqlDbStringTypeConverter.ValidatorToDbType(column.DataType);
            }

            return columns.Cast<IColumn>().ToList();
        }

        /// <summary>
        /// Gets the schema of all the contraints of a table.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>
        /// A list of constraint schemas.
        /// </returns>
        public List<IConstraint> GetConstraints(string database, string tableName)
        {
            var constraints = this.ConstraintQuery
                .Execute($"\"keyspace_name\"='{database}' AND \"columnfamily_name\"='{tableName}'")
                .Where(x => x.ColumnType == "partition_key")
                .ToList();

            foreach (var constraint in constraints)
            {
                constraint.Type = ConstraintType.PrimaryKey;
                constraint.FromColumnName = constraint.Name;
            }

            return constraints.Cast<IConstraint>().ToList();
        }

        /// <summary>
        /// Gets the schema of all the parameters of a stored procedure.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="routineName">The routine name.</param>
        /// <returns>
        /// A list of parameter schemas.
        /// </returns>
        public List<IParameter> GetParameters(string database, string routineName)
        {
            return new List<IParameter>();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Creates a IN (..,..,..) group string from a collection of values.
        /// </summary>
        /// <param name="values">Array of values to include inside the IN sentence.</param>
        /// <returns>A sql IN clause string.</returns>
        private string GetStringInGroup(IEnumerable<string> values)
        {
            return string.Join(", ", values.Select(x => this.FormatProvider.GetColumnValue(x, typeof(string))));
        }

        /// <summary>
        /// Gets the table WHERE clause.
        /// </summary>
        /// <param name="database">The name of the database.</param>
        /// <param name="type">The table type.</param>
        /// <param name="filter">The table names that need to be retrieved.</param>
        /// <returns>A sql WHERE clause string.</returns>
        private string GetTableWhere(string database, string type, string[] filter)
        {
            return $"\"keyspace_name\"={this.FormatProvider.GetColumnValue(database, typeof(string))}";
        }

        #endregion
    }
}