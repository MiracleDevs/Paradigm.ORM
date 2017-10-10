using System;
using System.Collections.Generic;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides an interface to describe the mapping relationship between a routine and a .NET type.
    /// </summary>
    public interface IRoutineTypeDescriptor
    {
        /// <summary>
        /// Gets the mapping type.
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Gets the name of the type.
        /// </summary>
        string TypeName { get; }

        /// <summary>
        /// Gets the name of the database catalog.
        /// </summary>
        string CatalogName { get; }

        /// <summary>
        /// Gets the name of the database schema.
        /// </summary>
        string SchemaName { get; }

        /// <summary>
        /// Gets the name of the routine.
        /// </summary>
        string RoutineName { get; }

        /// <summary>
        /// Gets a list of column parameter descriptors for all the parameters.
        /// </summary>
        List<IParameterPropertyDescriptor> Parameters { get; }
    }
}