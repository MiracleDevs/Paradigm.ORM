using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Cassandra.Schema.Structure
{
    /// <summary>
    /// Provides a database column schema.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Database.Schema.Structure.IColumn" />
    [Table("schema_columns", Catalog = "information_schema")]
    public class CqlColumn : IColumn
    {
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        [Column("column_name", "text")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the name of the catalog where the parent table resides.
        /// </summary>
        [Column("keyspace_name", "text")]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets the name of the schema where the parent table resides.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets the name of the parent table or view.
        /// </summary>
        [Column("columnfamily_name", "text")]
        public string TableName { get; set; }

        /// <summary>
        /// Gets the data type of the column.
        /// </summary>
        [Column("validator", "text")]
        public string DataType { get; set; }

        /// <summary>
        /// Gets the maximum size of the field, or zero if the column doesn't have a variable size.
        /// </summary>
        public long MaxSize { get; set; }

        /// <summary>
        /// Gets the numeric precision of the column, or zero if the column data type is not numeric.
        /// </summary>
        public byte Precision { get; set; }

        /// <summary>
        /// Gets the numeric scale of the column, or zero if the column data type is not numeric or is not a decimal numeric type.
        /// </summary>
        public byte Scale { get; set; }

        /// <summary>
        /// Gets the default value of the column, or null if the column does not have a default value.
        /// </summary>
        /// <remarks>
        /// The default value is retrieved in string format, so you must cast to the proper type
        /// before using it.
        /// </remarks>
        public string DefaultValue { get; set; }

        /// <summary>
        /// Indicates if the column value accepts null values.
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Indicates if the column is an identity column, and auto-increments its value.
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Column [{this.TableName}].[{this.Name}]";
    }
}
