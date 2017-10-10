namespace Paradigm.ORM.Data.Database.Schema.Structure
{
    /// <summary>
    /// Provides an interface for a database parameter schema.
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Gets the name of the catalog where the parent routine resides.
        /// </summary>
        string CatalogName { get; }

        /// <summary>
        /// Gets the name of the schema where the parent routine resides.
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        /// Gets the name of the parent routine.
        /// </summary>
        string StoredProcedureName { get; }

        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the data type of the parameter.
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
        /// Indicates if the parameter value will be passed as input, or will be an output 
        /// parameter and should be return a value after the execution of the routine.
        /// </summary>
        bool IsInput { get; }
    }
}