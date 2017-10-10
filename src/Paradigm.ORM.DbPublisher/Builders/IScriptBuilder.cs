using System.Collections.Generic;

namespace Paradigm.ORM.DbPublisher.Builders
{
    public interface IScriptBuilder
    {
        Dictionary<string, string> Scripts { get; }

        void Build(IEnumerable<string> fileNames);

        void SaveScript(string fileName);

        string GetScript(string fileName);
    }
}