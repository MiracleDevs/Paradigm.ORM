using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the property is part of a database foreign key.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = true)]
    public class ForeignKeyAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the name of the column.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the source table.
        /// </summary>
        public string FromTable { get; set; }

        /// <summary>
        /// Gets or sets the name  the source column.
        /// </summary>
        public string FromColumn { get; set; }

        /// <summary>
        /// Gets or sets the name of the referenced table if any.
        /// </summary>
        public string ToTable { get; set; }

        /// <summary>
        /// Gets or sets the name of the referenced column if any.
        /// </summary>
        public string ToColumn { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyAttribute"/> class.
        /// </summary>
        public ForeignKeyAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyAttribute"/> class.
        /// </summary>
        /// <param name="name">The name of the column.</param>
        /// <param name="fromTable">The name of the source table.</param>
        /// <param name="fromColumn">The name of the source column.</param>
        /// <param name="toTable">The name of the referenced table.</param>
        /// <param name="toColumn">The name of the referenced column.</param>
        public ForeignKeyAttribute(string name, string fromTable, string fromColumn, string toTable, string toColumn)
        {
            this.Name = name;
            this.FromTable = fromTable;
            this.FromColumn = fromColumn;
            this.ToTable = toTable;
            this.ToColumn = toColumn;
        }
    }
}