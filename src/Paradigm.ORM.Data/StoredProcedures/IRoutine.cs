using System;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.StoredProcedures
{
    /// <summary>
    /// Provides an interface to create routine callers.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IRoutine: IDisposable
    {
        /// <summary>
        /// Gets the routine caller command.
        /// </summary>
        IDatabaseCommand Command { get; }
    }
}