using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.MySql.Schema.Structure
{
    /// <summary>
    /// Provides a database constraint schema.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.Schema.Structure.IConstraint" />
    public class MySqlConstraint: IConstraint
    {
        /// <summary>
        /// Gets the name of the constraint.
        /// </summary>
        [Column("constraint_name", "text")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the type of the constraint.
        /// </summary>
        [Column("constraint_type", "int")]
        public ConstraintType Type { get; set; }

        /// <summary>
        /// Gets the name of the catalog where the parent table resides.
        /// </summary>
        [Column("table_catalog", "text")]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets the name of the schema where the parent table resides.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets the name of the source table of the constraint.
        /// </summary>
        [Column("table_name", "text")]
        public string FromTableName { get; set; }

        /// <summary>
        /// Gets the name of the source column of the constraint.
        /// </summary>
        [Column("column_name", "text")]
        public string FromColumnName { get; set; }

        /// <summary>
        /// Gets the name of the referenced table name.
        /// </summary>
        [Column("referenced_table_name", "text")]
        public string ToTableName { get; set; }

        /// <summary>
        /// Gets the name of the referenced table column.
        /// </summary>
        [Column("referenced_column_name", "text")]
        public string ToColumnName { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Constraint [{this.FromTableName}].[{this.Name}]";
    }
}