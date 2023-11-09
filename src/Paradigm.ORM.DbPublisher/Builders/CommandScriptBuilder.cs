using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Paradigm.ORM.DbPublisher.Builders
{
    public class CommandScriptBuilder : ICommandScriptBuilder
    {
        #region Properties

        /// <summary>
        /// Gets the scripts.
        /// </summary>
        /// <value>
        /// The scripts.
        /// </value>
        public Dictionary<string, IEnumerable<CommandScript>> CommandScripts { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandScriptBuilder"/> class.
        /// </summary>
        public CommandScriptBuilder()
        {
            this.CommandScripts = new Dictionary<string, IEnumerable<CommandScript>>();
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

                var commandScripts = this.OpenCommandScripts(fileName);
                this.CommandScripts.Add(fileName, commandScripts);
            }
        }

        /// <summary>
        /// Saves the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        public void SaveCommandScript(string fileName, bool verbose)
        {
            var path = Path.GetDirectoryName(fileName) ?? throw new Exception("Unable to get the path to save the script.");

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            File.WriteAllText(fileName, string.Join(Environment.NewLine, this.CommandScripts.SelectMany(x => x.Value).Select(x => ProcessCommandScript(x, true))));
        }

        /// <summary>
        /// Gets the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Script with path '{fileName}' was not loaded.</exception>
        public IEnumerable<CommandScript> GetCommandScript(string fileName)
        {
            if (!this.CommandScripts.ContainsKey(fileName))
                throw new Exception($"Script with path '{fileName}' was not loaded.");

            var scripts = this.CommandScripts[fileName];
            return scripts.Select(x => new CommandScript(x.Name, ProcessCommandScript(x, false), x.IgnoreErrors));
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Opens the script.
        /// </summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        private IEnumerable<CommandScript> OpenCommandScripts(string fileName)
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

            var scriptsContent = content.Split("#go");
            return scriptsContent.Select(x => new CommandScript(fileName, x, ignoreErrors));
        }

        /// <summary>
        /// Processes the script.
        /// </summary>
        /// <param name="commandScript">The script.</param>
        /// <param name="isForCommandLine">if set to <c>true</c> [is for command line].</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">
        /// Can not continue because a {endif} is missing at line {lineNumber} in file {name}.
        /// or
        /// Can not continue because a {endif} is missing at line {lineNumber} in file {name}.
        /// </exception>
        private string ProcessCommandScript(CommandScript commandScript, bool isForCommandLine)
        {
            const string ifcmd = "#ifcmd";
            const string ifexe = "#ifexe";
            const string endif = "#endif";
            int startIndex;
            var content = commandScript.Content;
            var name = commandScript.Name;

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