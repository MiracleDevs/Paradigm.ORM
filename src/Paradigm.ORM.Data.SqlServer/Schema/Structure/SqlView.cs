using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.SqlServer.Schema.Structure
{
    /// <summary>
    /// Provides a database view schema.
    /// </summary>
    /// <seealso cref="IView" />
    [Table("tables", Catalog = "information_schema")]
    public class SqlView: IView
    {
        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        [Column("table_name", "text")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the name of the catalog where the view resides.
        /// </summary>
        [Column("table_catalog", "text")]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets the name of the schema where the view resides.
        /// </summary>
        [Column("table_schema", "text")]
        public string SchemaName { get; set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"View [{this.SchemaName}].[{this.Name}]";
    }
}