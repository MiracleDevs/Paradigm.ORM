namespace Paradigm.ORM.Data.Database.Schema.Structure
{
    /// <summary>
    /// Provides an interface for a database constraint schema.
    /// </summary>
    public interface IConstraint
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
        /// Gets the name of the constraint.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the type of the constraint.
        /// </summary>
        ConstraintType Type { get; }

        /// <summary>
        /// Gets the name of the source table of the constraint.
        /// </summary>
        string FromTableName { get; }

        /// <summary>
        /// Gets the name of the source column of the constraint.
        /// </summary>
        string FromColumnName { get; }

        /// <summary>
        /// Gets the name of the referenced table name.
        /// </summary>
        string ToTableName { get; }

        /// <summary>
        /// Gets the name of the referenced table column.
        /// </summary>
        string ToColumnName { get; }
    }
}