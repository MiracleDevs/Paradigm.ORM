using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.SqlServer.Schema.Structure
{
    /// <summary>
    /// Provides a database stored procedure schema.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.Schema.Structure.IStoredProcedure" />
    [Table("routines", Catalog = "information_schema")]
    public class SqlStoredProcedure : IStoredProcedure
    {
        /// <summary>
        /// Gets the name of the stored procedure.
        /// </summary>
        [Column("routine_name", "text")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the name of the catalog where the routine resides.
        /// </summary>
        [Column("routine_catalog", "text")]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets the name of the schema where the view resides.
        /// </summary>
        [Column("routine_schema", "text")]
        public string SchemaName { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Stored Procedure [{this.SchemaName}].[{this.Name}]";
    }
}