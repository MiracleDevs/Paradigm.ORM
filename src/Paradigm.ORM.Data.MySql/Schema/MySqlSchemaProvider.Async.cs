using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.MySql.Schema
{
    public partial class MySqlSchemaProvider 
    {
        #region Public Methods

        /// <summary>
        /// Gets the schema of database tables specifying the database, and allowing to filter which tables to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of table names you want to retrieve.</param>
        /// <returns>
        /// A list of table schemas.
        /// </returns>
        public async Task<List<ITable>> GetTablesAsync(string database, params string[] filter)
        {
            return (await this.TableQuery.ExecuteAsync(this.GetTableWhere(database, TableType, filter))).Cast<ITable>().ToList();
        }

        /// <summary>
        /// Gets the schema of database views specifying the database, and allowing to filter which views to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of view names you want to retrieve.</param>
        /// <returns>
        /// A list of view schemas.
        /// </returns>
        public async Task<List<IView>> GetViewsAsync(string database, params string[] filter)
        {
            return (await this.ViewQuery.ExecuteAsync(this.GetTableWhere(database, ViewType, filter))).Cast<IView>().ToList();
        }

        /// <summary>
        /// Gets the schema of stored procedures specifying the database, and allowing to filter which stored procedures to return.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="filter">An array of stored procedure names you want to retrieve.</param>
        /// <returns>
        /// A list of stored procedure schemas.
        /// </returns>
        public async Task<List<IStoredProcedure>> GetStoredProceduresAsync(string database, params string[] filter)
        {
            return (await this.StoredProcedureQuery.ExecuteAsync(this.GetRoutineWhere(database, StoredProcedureType, filter))).Cast<IStoredProcedure>().ToList();
        }

        /// <summary>
        /// Gets the schema of all the columns of a table.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>
        /// A list of column schemas.
        /// </returns>
        public async Task<List<IColumn>> GetColumnsAsync(string database, string tableName)
        {
            return (await this.ColumnQuery.ExecuteAsync($"`TABLE_SCHEMA`='{database}' AND `TABLE_NAME`='{tableName}'")).Cast<IColumn>().ToList();
        }

        /// <summary>
        /// Gets the schema of all the contraints of a table.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="tableName">The table name.</param>
        /// <returns>
        /// A list of constraint schemas.
        /// </returns>
        public async Task<List<IConstraint>> GetConstraintsAsync(string database, string tableName)
        {
            return (await this.ConstraintQuery.ExecuteAsync($"kcu.`TABLE_SCHEMA`='{database}' AND kcu.`TABLE_NAME`='{tableName}'")).Cast<IConstraint>().ToList();
        }

        /// <summary>
        /// Gets the schema of all the parameters of a stored procedure.
        /// </summary>
        /// <param name="database">The database name.</param>
        /// <param name="routineName">The routine name.</param>
        /// <returns>
        /// A list of parameter schemas.
        /// </returns>
        public async Task<List<IParameter>> GetParametersAsync(string database, string routineName)
        {
            return (await this.ParameterQuery.ExecuteAsync($"`SPECIFIC_SCHEMA`='{database}' AND `SPECIFIC_NAME`='{routineName}'")).Cast<IParameter>().ToList();
        }

        #endregion
    }
}