using System.Collections.Generic;

namespace Paradigm.ORM.DbPublisher.Builders
{
    public interface ICommandScriptBuilder
    {
        /// <summary>
        /// Gets the scripts.
        /// </summary>
        /// <value>
        /// The scripts.
        /// </value>
        Dictionary<string, IEnumerable<CommandScript>> CommandScripts { get; }

        /// <summary>
        /// Builds the specified file names.
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        void Build(IEnumerable<string> fileNames);

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        void SaveCommandScript(string fileName, bool verbose);

        /// <summary>
        /// Gets the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        IEnumerable<CommandScript> GetCommandScript(string fileName);
    }
}