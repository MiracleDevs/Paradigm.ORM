using System.Collections.Generic;
using System.Linq;
using System.Text;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Database.Schema;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.Data.Querying;
using Paradigm.ORM.Data.PostgreSql.Schema.Structure;

namespace Paradigm.ORM.Data.PostgreSql.Schema
{
    /// <summary>
    /// Provides a way to retrieve schema information from the database.
    /// </summary>
    public partial class PostgreSqlSchemaProvider : ISchemaProvider
    {
        #region String Constants

        /// <summary>
        /// Gets the column schema query string.
        /// </summary>
        private const string ColumnQueryString =
            @"SELECT  
                    ""column_name"", 
                    ""table_catalog"",
                    ""table_schema"",
                    ""table_name"", 
                    ""data_type"", 
                    ""character_maximum_length"", 
                    ""numeric_precision"", 
                    ""numeric_scale"", 
                    ""column_default"", 
                    ""is_nullable"", 
                    CASE WHEN ""relname"" IS NOT NULL THEN 1 
                    ELSE 0 
                    END as ""is_identity""
                 FROM 
                     ""information_schema"".""columns""
                 LEFT JOIN ""pg_class""
                     ON ""relname"" = (""table_name"" || '_' || ""column_name"" || '_seq') OR ""relname"" IS NULL ";

        /// <summary>
        /// Gets the constraint schema query string.
        /// </summary>
        private const string ConstraintQueryString =
            @"SELECT  
                    kcu.""table_catalog"",
                    kcu.""table_schema"",
                    kcu.""table_name"",
                    kcu.""column_name"", 
                    kcu.""constraint_name"",  
                    CASE WHEN 
                        tc.""constraint_type"" = 'PRIMARY KEY'      THEN 1
                        WHEN tc.""constraint_type"" = 'FOREIGN KEY' THEN 2
                        WHEN tc.""constraint_type"" = 'UNIQUE'      THEN 3
                        ELSE 0
                    END as ""constraint_type"", 
                    ccu.""table_name"" AS ""referenced_table_name"", 
                    ccu.""column_name"" AS ""referenced_column_name""
                 FROM
                   ""information_schema"".""table_constraints"" AS tc
                    JOIN ""information_schema"".""key_column_usage"" AS kcu
                      ON tc.""constraint_name"" = kcu.""constraint_name""
                    JOIN ""information_schema"".""constraint_column_usage"" AS ccu
                      ON (ccu.""constraint_name"" = kcu.""constraint_name"" and ccu.""column_name"" = kcu.""column_name"")
                      OR (tc.""constraint_type"" not in ('UNIQUE', 'PRIMARY KEY') and ccu.""constraint_name"" = kcu.""constraint_name"")";

        /// <summary>
        /// Gets the parameter schema query string.
        /// </summary>
        private const string ParameterQueryString =
            @"SELECT
                    ""routines"".""specific_catalog"" as ""routine_catalog"",
                    ""routines"".""specific_schema"" as ""routine_schema"",
                    ""routine_name"",
                    CASE WHEN ""parameter_mode"" = 'IN' THEN 1 ELSE 0 END as ""is_input"",
                    ""parameter_name"",
                    ""parameters"".""data_type"",
                    ""parameters"".""character_maximum_length"",
                    ""parameters"".""numeric_precision"",
                    ""parameters"".""numeric_scale""
                FROM ""information_schema"".""routines""
                JOIN ""information_schema"".""parameters"" ON ""routines"".""specific_name""=""parameters"".""specific_name""";


        /// <summary>
        /// Gets the table type name.
        /// </summary>
        private const string TableType = "BASE TABLE";

        /// <summary>
        /// Gets the view type name.
        /// </summary>
        private const string ViewType = "VIEW";

        /// <summary>
        /// Gets the stored procedure type name.
        /// </summary>
        private const string StoredProcedureType = "FUNCTION";

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command format provider.
        /// </summary>
        private ICommandFormatProvider FormatProvider { get; }

        /// <summary>
        /// Gets or sets the column query.
        /// </summary>
        private CustomQuery<PostgreSqlColumn> ColumnQuery { get; }

        /// <summary>
        /// Gets or sets the constraint query.
        /// </summary>
        private CustomQuery<PostgreSqlConstraint> ConstraintQuery { get; }

        /// <summary>
        /// Gets or sets the parameter query.
        /// </summary>
        private CustomQuery<PostgreSqlParameter> ParameterQuery { get; }

        /// <summary>
        /// Gets or sets the view query.
        /// </summary>
        private Query<PostgreSqlView> ViewQuery { get; }

        /// <summary>
        /// Gets or sets the table query.
        /// </summary>
        private Query<PostgreSqlTable> TableQuery { get; }

        /// <summary>
        /// Gets or sets the stored procedure query.
        /// </summary>
        private Query<PostgreSqlStoredProcedure> StoredProcedureQuery { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PostgreSqlSchemaProvider"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public PostgreSqlSchemaProvider(IDatabaseConnector connector)
        {
            this.FormatProvider = connector.GetCommandFormatProvider();

            this.ColumnQuery = new CustomQuery<PostgreSqlColumn>(connector, ColumnQueryString);
            this.ConstraintQuery = new CustomQuery<PostgreSqlConstraint>(connector, ConstraintQueryString);
            this.ParameterQuery = new CustomQuery<PostgreSqlParameter>(connector, ParameterQueryString);
            this.ViewQuery = new Query<PostgreSqlView>(connector);
            this.TableQuery = new Query<PostgreSqlTable>(connector);
            this.StoredProcedureQuery = new Query<PostgreSqlStoredProcedure>(connector);
        }

        #endregion

        #region Public Methods

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
            return this.TableQuery.Execute(this.GetTableWhere(database, TableType, filter)).Cast<ITable>().ToList();
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
            return this.ViewQuery.Execute(this.GetTableWhere(database, ViewType, filter)).Cast<IView>().ToList();
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
            return this.StoredProcedureQuery.Execute(this.GetRoutineWhere(database, StoredProcedureType, filter)).Cast<IStoredProcedure>().ToList();
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
            return this.ColumnQuery.Execute($"\"table_catalog\"='{database}' AND \"table_name\"='{tableName}'").Cast<IColumn>().ToList();
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
            return this.ConstraintQuery.Execute($"kcu.\"table_catalog\"='{database}' AND kcu.\"table_name\"='{tableName}'").Cast<IConstraint>().ToList();
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
            return this.ParameterQuery.Execute($"routines.\"specific_catalog\"='{database}' AND \"routine_name\"='{routineName}'").Cast<IParameter>().ToList();
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
            var builder = new StringBuilder();
            builder.AppendFormat("\"table_type\"={0} AND ", this.FormatProvider.GetColumnValue(type.ToUpper(), typeof(string)));
            builder.AppendFormat("\"table_catalog\"={0}", this.FormatProvider.GetColumnValue(database, typeof(string)));

            if (filter != null && filter.Any())
                builder.AppendFormat(" AND \"table_name\" IN ({0})", this.GetStringInGroup(filter));

            return builder.ToString();
        }

        /// <summary>
        /// Gets the stored procedure WHERE clause.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="type">The routine type.</param>
        /// <param name="filter">The stored procedure names that need to be retireved.</param>
        /// <returns>A sql WHERE clause string.</returns>
        private string GetRoutineWhere(string database, string type, string[] filter)
        {
            var builder = new StringBuilder();
            builder.AppendFormat("\"routine_type\"={0} AND ", this.FormatProvider.GetColumnValue(type.ToUpper(), typeof(string)));
            builder.AppendFormat("\"routine_catalog\"={0}", this.FormatProvider.GetColumnValue(database, typeof(string)));

            if (filter != null && filter.Any())
                builder.AppendFormat(" AND \"routine_name\" IN ({0})", this.GetStringInGroup(filter));

            return builder.ToString();
        }

        #endregion
    }
}