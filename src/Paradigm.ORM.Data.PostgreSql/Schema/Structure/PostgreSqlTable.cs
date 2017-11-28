using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.PostgreSql.Schema.Structure
{
    /// <summary>
    /// Provides a database table schema.
    /// </summary>
    /// <seealso cref="ITable" />
    [Table("tables", Catalog = "information_schema")]
    public class PostgreSqlTable : ITable
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("table_name", "text")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the name of the catalog where the table resides.
        /// </summary>
        [Column("table_catalog", "text")]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets or sets the name of the schema.
        /// </summary>
        [Column("table_schema", "text")]
        public string SchemaName { get; set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Table {this.SchemaName}.{this.Name}";
    }
}