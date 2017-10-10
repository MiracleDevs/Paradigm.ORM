using System;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface for command builder objects.
    /// The ORM uses the command builders to create and cache
    /// regular CRUD commands.
    /// </summary>
    public interface ICommandBuilder: IDisposable
    {
    }
}