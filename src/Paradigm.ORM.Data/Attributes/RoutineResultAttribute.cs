using System;

namespace Paradigm.ORM.Data.Attributes
{
    /// <summary>
    /// Indicates that the class maps to a routine, and returns specific classes as a result of the execution of the routine.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RoutineResultAttribute: Attribute
    {
        /// <summary>
        /// Gets or sets the name of the result type.
        /// </summary>
        public string ResultType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineResultAttribute"/> class.
        /// </summary>
        public RoutineResultAttribute()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineResultAttribute"/> class.
        /// </summary>
        /// <param name="resultType">Type of the result.</param>
        public RoutineResultAttribute(string resultType)
        {
            this.ResultType = resultType;
        }
    }
}