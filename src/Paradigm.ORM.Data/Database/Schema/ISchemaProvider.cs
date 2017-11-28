using System.Collections.Generic;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Database.Schema
{
    /// <summary>
    /// Provides an interface to retrieve schema information from the database.
    /// </summary>
    public partial interface ISchemaProvider
    {
        /// <summary>
        /// Gets the schema of database tables specifying the database, and allowing to filter which tables to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of table names you want to retrieve.</param>
        /// <returns>A list of table schemas.</returns>
        List<ITable> GetTables(string database, params string[] filter);

        /// <summary>
        /// Gets the schema of database views specifying the database, and allowing to filter which views to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of view names you want to retrieve.</param>
        /// <returns>A list of view schemas.</returns>
        List<IView> GetViews(string database, params string[] filter);

        /// <summary>
        /// Gets the schema of stored procedures specifying the database, and allowing to filter which stored procedures to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of stored procedure names you want to retrieve.</param>
        /// <returns>A list of stored procedure schemas.</returns>
        List<IStoredProcedure> GetStoredProcedures(string database, params string[] filter);

        /// <summary>
        /// Gets the schema of all the columns of a table.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>A list of column schemas.</returns>
        List<IColumn> GetColumns(string database, string tableName);

        /// <summary>
        /// Gets the schema of all the contraints of a table.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>A list of constraint schemas.</returns>
        List<IConstraint> GetConstraints(string database, string tableName);

        /// <summary>
        /// Gets the schema of all the parameters of a stored procedure.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="routineName">The routine name.</param>
        /// <returns>A list of parameter schemas.</returns>
        List<IParameter> GetParameters(string database, string routineName);
    }
}