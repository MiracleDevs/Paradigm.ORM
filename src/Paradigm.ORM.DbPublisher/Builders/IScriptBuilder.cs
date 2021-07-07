using System.Collections.Generic;

namespace Paradigm.ORM.DbPublisher.Builders
{
    public interface IScriptBuilder
    {
        /// <summary>
        /// Gets the scripts.
        /// </summary>
        /// <value>
        /// The scripts.
        /// </value>
        Dictionary<string, Script> Scripts { get; }

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
        void SaveScript(string fileName, bool verbose);

        /// <summary>
        /// Gets the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        Script GetScript(string fileName);
    }
}