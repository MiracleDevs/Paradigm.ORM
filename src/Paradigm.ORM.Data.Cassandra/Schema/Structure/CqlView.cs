using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Cassandra.Schema.Structure
{
    /// <summary>
    /// Provides a database view schema.
    /// </summary>
    /// <seealso cref="IView" />
    [Table("views", Catalog = "system_schema")]
    public class CqlView : IView
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        [Column("view_name", "text")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the name of the catalog where the view resides.
        /// </summary>
        [Column("keyspace_name", "text")]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets the name of the schema where the view resides.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"View [{this.SchemaName}].[{this.Name}]";
    }
}