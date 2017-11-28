using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.SqlServer.Schema.Structure
{
    /// <summary>
    /// Provides a database parameter schema.
    /// </summary>
    /// <seealso cref="IParameter" />
    public class SqlParameter : IParameter
    {
        /// <summary>
        /// Gets the name of the parameter.
        /// </summary>
        [Column("parameter_name", "text")]
        public string Name { get; set; }

        /// <summary>
        /// Gets the name of the catalog where the parent routine resides.
        /// </summary>
        [Column("routine_catalog", "text")]
        public string CatalogName { get; set; }

        /// <summary>
        /// Gets the name of the schema where the parent routine resides.
        /// </summary>
        [Column("routine_schema", "text")]
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets the name of the parent routine.
        /// </summary>
        [Column("routine_name", "text")]
        public string StoredProcedureName { get; set; }

        /// <summary>
        /// Gets the data type of the parameter.
        /// </summary>
        [Column("data_type", "text")]
        public string DataType { get; set; }

        /// <summary>
        /// Gets the maximum size of the field, or zero if the column doesn't have a variable size.
        /// </summary>
        [Column("character_maximum_length", "bigint")]
        public long MaxSize { get; set; }

        /// <summary>
        /// Gets the numeric precision of the column, or zero if the column data type is not numeric.
        /// </summary>
        [Column("numeric_precision", "int")]
        public byte Precision { get; set; }

        /// <summary>
        /// Gets the numeric scale of the column, or zero if the column data type is not numeric or is not a decimal numeric type.
        /// </summary>
        [Column("numeric_scale", "int")]
        public byte Scale { get; set; }

        /// <summary>
        /// Indicates if the parameter value will be passed as input, or will be an output
        /// parameter and should be return a value after the execution of the routine.
        /// </summary>
        [Column("is_input", "bit")]
        public bool IsInput { get; set; }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Parameter [{this.StoredProcedureName}].[{this.Name}]";
    }
}