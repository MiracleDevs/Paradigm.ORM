using System;

namespace Paradigm.ORM.Data.CommandBuilders
{
    /// <summary>
    /// Provides an interface for command builder manager objects.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface ICommandBuilderManager: IDisposable
    {
        /// <summary>
        /// Gets the select one command builder.
        /// </summary>
        /// <value>
        /// The select one command builder.
        /// </value>
        ISelectOneCommandBuilder SelectOneCommandBuilder { get; }

        /// <summary>
        /// Gets the select command builder.
        /// </summary>
        /// <value>
        /// The select command builder.
        /// </value>
        ISelectCommandBuilder SelectCommandBuilder { get; }

        /// <summary>
        /// Gets the insert command builder.
        /// </summary>
        /// <value>
        /// The insert command builder.
        /// </value>
        IInsertCommandBuilder InsertCommandBuilder { get; }

        /// <summary>
        /// Gets the update command builder.
        /// </summary>
        /// <value>
        /// The update command builder.
        /// </value>
        IUpdateCommandBuilder UpdateCommandBuilder { get; }

        /// <summary>
        /// Gets the delete command builder.
        /// </summary>
        /// <value>
        /// The delete command builder.
        /// </value>
        IDeleteCommandBuilder DeleteCommandBuilder { get; }

        /// <summary>
        /// Gets the last insert identifier command builder.
        /// </summary>
        /// <value>
        /// The last insert identifier command builder.
        /// </value>
        ILastInsertIdCommandBuilder LastInsertIdCommandBuilder { get; }
    }
}