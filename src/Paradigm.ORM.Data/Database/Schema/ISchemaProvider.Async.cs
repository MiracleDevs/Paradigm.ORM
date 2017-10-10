using System.Collections.Generic;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Database.Schema
{
    public partial interface ISchemaProvider
    {
        /// <summary>
        /// Gets the schema of database tables specifying the database, and allowing to filter which tables to return. 
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of table names you want to retrieve.</param>
        /// <returns>A list of table schemas.</returns>
        Task<List<ITable>> GetTablesAsync(string database, params string[] filter);

        /// <summary>
        /// Gets the schema of database views specifying the database, and allowing to filter which views to return. 
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of view names you want to retrieve.</param>
        /// <returns>A list of view schemas.</returns>
        Task<List<IView>> GetViewsAsync(string database, params string[] filter);

        /// <summary>
        /// Gets the schema of stored procedures specifying the database, and allowing to filter which stored procedures to return. 
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of stored procedure names you want to retrieve.</param>
        /// <returns>A list of stored procedure schemas.</returns>
        Task<List<IStoredProcedure>> GetStoredProceduresAsync(string database, params string[] filter);

        /// <summary>
        /// Gets the schema of all the columns of a table.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>A list of column schemas.</returns>
        Task<List<IColumn>> GetColumnsAsync(string database, string tableName);

        /// <summary>
        /// Gets the schema of all the contraints of a table.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>A list of constraint schemas.</returns>
        Task<List<IConstraint>> GetConstraintsAsync(string database, string tableName);

        /// <summary>
        /// Gets the schema of all the parameters of a stored procedure.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="routineName">The routine name.</param>
        /// <returns>A list of parameter schemas.</returns>
        Task<List<IParameter>> GetParametersAsync(string database, string routineName);       
    }
}