using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Paradigm.ORM.DbPublisher.Builders
{
    public class ScriptBuilder : IScriptBuilder
    {
        #region Properties

        public Dictionary<string, string> Scripts { get; }

        #endregion

        #region Constructor

        public ScriptBuilder()
        {
            this.Scripts = new Dictionary<string, string>();
        }

        #endregion

        #region Public Methods

        public void Build(IEnumerable<string> fileNames)
        {
            foreach (var fileName in fileNames)
            {
                if (!File.Exists(fileName))
                    throw new Exception("File not found.");

                this.Scripts.Add(fileName, File.ReadAllText(fileName));
            }
        }

        public void SaveScript(string fileName)
        {
            File.WriteAllText(fileName, string.Join(Environment.NewLine, this.Scripts.Keys.Select(x => ProcessScript(x, true))));
        }

        public string GetScript(string fileName)
        {
            return ProcessScript(fileName, false);
        }

        #endregion

        #region Private Methods

        private string ProcessScript(string fileName, bool isForCommandLine)
        {
            if (!this.Scripts.ContainsKey(fileName))
                return string.Empty;

            const string ifcmd = "#ifcmd";
            const string ifexe = "#ifexe";
            const string endif = "#endif";
            var file = Path.GetFileName(fileName);
            int startIndex;

            var content = this.Scripts[fileName];

            if (content == null)
                return null;

            while ((startIndex = content.IndexOf(ifcmd, StringComparison.Ordinal)) >= 0)
            {
                var endIndex = content.IndexOf(endif, startIndex, StringComparison.Ordinal);

                if (endIndex < 0)
                {
                    var lineNumber = content.Take(startIndex).Count(c => c == '\n') + 1;
                    throw new Exception($"Can not continue because a {endif} is missing at line {lineNumber} in file {file}.");
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
                    throw new Exception($"Can not continue because a {endif} is missing at line {lineNumber} in file {file}.");
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