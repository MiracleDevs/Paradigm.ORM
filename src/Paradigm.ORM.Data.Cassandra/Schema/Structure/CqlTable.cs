using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Cassandra.Schema.Structure
{
    /// <summary>
    /// Provides a database table schema.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.Schema.Structure.ITable" />
    [Table("schema_columnfamilies", Catalog = "system")]
    public class CqlTable : ITable
    {
        /// <summary>
        /// Gets the name of the constraint.
        /// </summary>
        [Column("type", "text")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        [Column("columnfamily_name", "text")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the name of the catalog where the table resides.
        /// </summary>
        [Column("keyspace_name", "text")]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets or sets the name of the schema.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Table [{this.SchemaName}].[{this.Name}]";
    }
}