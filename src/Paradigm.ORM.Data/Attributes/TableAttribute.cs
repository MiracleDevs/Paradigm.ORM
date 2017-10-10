using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the class maps to a database table or view.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the catalog.
        /// </summary>
        public string Catalog { get; set; }

        /// <summary>
        /// Gets or sets the name of the schema.
        /// </summary>
        public string Schema { get; set; }

        /// <summary>
        /// Gets or sets the name of the table.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableAttribute"/> class.
        /// </summary>
        public TableAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableAttribute"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        public TableAttribute(string name)
        {
            this.Name = name;

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TableAttribute"/> class.
        /// </summary>
        /// <param name="catalog">The database catalog.</param>
        /// <param name="schema">The catalog schema.</param>
        /// <param name="name">The schema table.</param>
        public TableAttribute(string catalog, string schema, string name)
        {
            this.Catalog = catalog;
            this.Schema = schema;
            this.Name = name;
        }
    }
}