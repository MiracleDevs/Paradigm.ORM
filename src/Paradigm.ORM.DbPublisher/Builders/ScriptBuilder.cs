using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Paradigm.ORM.DbPublisher.Builders
{
    public class ScriptBuilder : IScriptBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the scripts.
        /// </summary>
        /// <value>
        /// The scripts.
        /// </value>
        public Dictionary<string, Script> Scripts { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptBuilder"/> class.
        /// </summary>
        public ScriptBuilder()
        {
            this.Scripts = new Dictionary<string, Script>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Builds the specified file names.
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        /// <exception cref="System.Exception">File not found.</exception>
        public void Build(IEnumerable<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                if (!File.Exists(fileName))
                    throw new Exception("File not found.");

                this.Scripts.Add(fileName, this.OpenScript(fileName));
            }
        }

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        public void SaveScript(string fileName, bool verbose)
        {
            var path = Path.GetDirectoryName(fileName);

            if (path == null)
                throw new Exception("Unable to get the path to save the script.");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(fileName, string.Join(Environment.NewLine, this.Scripts.Values.Select(x => ProcessScript(x, true))));
        }

        /// <summary>
        /// Gets the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Script with path '{fileName}' was not loaded.</exception>
        public Script GetScript(string fileName)
        {
            if (!this.Scripts.ContainsKey(fileName))
                throw new Exception($"Script with path '{fileName}' was not loaded.");

            var script = this.Scripts[fileName];
            return new Script(script.Name, ProcessScript(script, false), script.IgnoreErrors);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Opens the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private Script OpenScript(string fileName)
        {
            const string ignoreString = "#ignore-errors";
            var content = File.ReadAllText(fileName);
            var ignoreErrors = false;
            int startIndex;

            while ((startIndex = content.IndexOf(ignoreString, StringComparison.Ordinal)) >= 0)
            {
                ignoreErrors = true;
                content = content.Remove(startIndex, ignoreString.Length);
            }

            return new Script(fileName, content, ignoreErrors);
        }

        /// <summary>
        /// Processes the script.
        /// </summary>
        /// <param name="script">The script.</param>
        /// <param name="isForCommandLine">if set to <c>true</c> [is for command line].</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Can not continue because a {endif} is missing at line {lineNumber} in file {name}.
        /// or
        /// Can not continue because a {endif} is missing at line {lineNumber} in file {name}.
        /// </exception>
        private string ProcessScript(Script script, bool isForCommandLine)
        {
            const string ifcmd = "#ifcmd";
            const string ifexe = "#ifexe";
            const string endif = "#endif";
            int startIndex;
            var content = script.Content;
            var name = script.Name;

            if (content == null)
                return null;

            while ((startIndex = content.IndexOf(ifcmd, StringComparison.Ordinal)) >= 0)
            {
                var endIndex = content.IndexOf(endif, startIndex, StringComparison.Ordinal);

                if (endIndex < 0)
                {
                    var lineNumber = content.Take(startIndex).Count(c => c == '\n') + 1;
                    throw new Exception($"Can not continue because a {endif} is missing at line {lineNumber} in file {name}.");
                }

                if (isForCommandLine)
                {
                    var start = startIndex + ifcmd.Length + 1;
                    var length = endIndex - start - 1;

                    var ifContent = content.Substring(start, length);
                    content = content.Remove(startIndex, endIndex - startIndex + endif.Length);
                    content = content.Insert(startIndex, ifContent);
                }
                else
                {
                    content = content.Remove(startIndex, endIndex - startIndex + endif.Length);
                }
            }

            while ((startIndex = content.IndexOf(ifexe, StringComparison.Ordinal)) >= 0)
            {
                var endIndex = content.IndexOf(endif, startIndex, StringComparison.Ordinal);

                if (endIndex < 0)
                {
                    var lineNumber = content.Take(startIndex).Count(c => c == '\n') + 1;
                    throw new Exception($"Can not continue because a {endif} is missing at line {lineNumber} in file {name}.");
                }

                if (!isForCommandLine)
                {
                    var start = startIndex + ifexe.Length + 1;
                    var length = endIndex - start - 1;

                    var ifContent = content.Substring(start, length);
                    content = content.Remove(startIndex, endIndex - startIndex + endif.Length);
                    content = content.Insert(startIndex, ifContent);
                }
                else
                {
                    content = content.Remove(startIndex, endIndex - startIndex + endif.Length);
                }
            }

            return content;
        }

        #endregion
    }
}