namespace Paradigm.ORM.Data.Database.Schema.Structure
{
    /// <summary>
    /// Specifies the constraint types the ORM retrieves.
    /// </summary>
    public enum ConstraintType
    {
        /// <summary>
        /// Primary key constraint.
        /// </summary>
        PrimaryKey = 1,

        /// <summary>
        /// Foreign key constraint.
        /// </summary>
        ForeignKey = 2,

        /// <summary>
        /// Unique key constraint.
        /// </summary>
        UniqueKey = 3
    }
}