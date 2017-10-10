namespace Paradigm.ORM.Data.Database.Schema.Structure
{
    /// <summary>
    /// Provides an interface for a database column schema.
    /// </summary>
    public interface IColumn
    {
        /// <summary>
        /// Gets the name of the catalog where the parent table resides.
        /// </summary>
        string CatalogName { get; }

        /// <summary>
        /// Gets the name of the schema where the parent table resides.
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        /// Gets the name of the parent table or view.
        /// </summary>
        string TableName { get; }

        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the data type of the column.
        /// </summary>
        string DataType { get; }

        /// <summary>
        /// Gets the maximum size of the field, or zero if the column doesn't have a variable size.
        /// </summary>
        long MaxSize { get; }

        /// <summary>
        /// Gets the numeric precision of the column, or zero if the column data type is not numeric.
        /// </summary>
        byte Precision { get; }

        /// <summary>
        /// Gets the numeric scale of the column, or zero if the column data type is not numeric or is not a decimal numeric type.
        /// </summary>
        byte Scale { get; }

        /// <summary>
        /// Gets the default value of the column, or null if the column does not have a default value.
        /// </summary>
        /// <remarks>
        /// The default value is retrieved in string format, so you must cast to the proper type 
        /// before using it.
        /// </remarks>
        string DefaultValue { get; }

        /// <summary>
        /// Indicates if the column value accepts null values.
        /// </summary>
        bool IsNullable { get; }

        /// <summary>
        /// Indicates if the column is an identity column, and auto-increments its value.
        /// </summary>
        bool IsIdentity { get; }
    }
}