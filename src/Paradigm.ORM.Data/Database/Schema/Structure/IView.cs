namespace Paradigm.ORM.Data.Database.Schema.Structure
{
    /// <summary>
    /// Provides an interface for a database view schema.
    /// </summary>
    public interface IView
    {
        /// <summary>
        /// Gets the name of the catalog where the view resides.
        /// </summary>
        string CatalogName { get; }

        /// <summary>
        /// Gets the name of the schema where the view resides.
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        /// Gets the name of the view.
        /// </summary>
        string Name { get; }
    }
}