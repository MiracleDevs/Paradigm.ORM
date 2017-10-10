using System;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates what type of the stored procedure the class is.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    /// <seealso cref="StoredProcedureType" />
    [AttributeUsage(AttributeTargets.Class)]
    public class StoredProcedureTypeAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the type of the procedure.
        /// </summary>
        public string ProcedureType { get; set; }

        /// <summary>
        /// Gets or sets the type of the procedure.
        /// </summary>
        public StoredProcedureType Type => (StoredProcedureType)Enum.Parse(typeof(StoredProcedureType), this.ProcedureType);

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureTypeAttribute"/> class.
        /// </summary>
        public StoredProcedureTypeAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StoredProcedureTypeAttribute"/> class.
        /// </summary>
        /// <param name="procedureType">Type of the procedure.</param>
        public StoredProcedureTypeAttribute(string procedureType)
        {
            this.ProcedureType = procedureType;
        }
    }
}