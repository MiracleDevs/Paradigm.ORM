namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides an interface to describe a database column.
    /// </summary>
    public interface IColumnDescriptor
    {
        /// <summary>
        /// Gets the name of the column.
        /// </summary>
        string ColumnName { get; }

        /// <summary>
        /// Gets the type of the data.
        /// </summary>
        string DataType { get; }

        /// <summary>
        /// Gets the maximum size.
        /// </summary>
        long MaxSize { get; }

        /// <summary>
        /// Gets the numeric precision.
        /// </summary>
        byte Precision { get; }

        /// <summary>
        /// Gets the numeric scale.
        /// </summary>
        byte Scale { get; }

        /// <summary>
        /// Indicates if the column is part of a primary key.
        /// </summary>
        bool IsPrimaryKey { get; }

        /// <summary>
        /// Indicates if the column is an identity.
        /// </summary>
        bool IsIdentity { get; }

        /// <summary>
        /// Indicates if the column is part of a foreign key.
        /// </summary>
        bool IsForeignKey { get; }

        /// <summary>
        /// Indicates if the column is part of a unique key.
        /// </summary>
        bool IsUniqueKey { get; }
    }
}