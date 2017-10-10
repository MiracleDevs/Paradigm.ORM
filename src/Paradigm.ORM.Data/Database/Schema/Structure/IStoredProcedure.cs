namespace Paradigm.ORM.Data.Database.Schema.Structure
{
    /// <summary>
    /// Provides an interface for a database stored procedure schema.
    /// </summary>
    public interface IStoredProcedure
    {
        /// <summary>
        /// Gets the name of the catalog where the view resides.
        /// </summary>
        string CatalogName { get; }

        /// <summary>
        /// Gets the name of the schema where the stored procedure resides.
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        /// Gets the name of the stored procedure.
        /// </summary>
        string Name { get; }
    }
}