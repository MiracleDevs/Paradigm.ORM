using Newtonsoft.Json;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;
using Paradigm.ORM.DbFirst.Schema;
using Paradigm.ORM.DbFirst.Translation;
using System.IO;
using System.Threading.Tasks;

namespace Paradigm.ORM.DbFirst.Export
{
    internal abstract class DbFirstExporterBase : IDbFirstExporter
    {
        public async Task ExportAsync(string configurationFileName, DbFirstConfiguration configuration)
        {
            using var connector = this.CreateConnector(configuration);
            await connector.OpenAsync();

            var database = new Database(connector);
            await database.ExtractSchemaAsync(configuration);
            database.ProcessResults();

            var objects = new DatabaseToObjectContainerTranslator(connector, configuration).Translate(database);
            var json = JsonConvert.SerializeObject(objects, Formatting.Indented);
            var outputPath = Path.IsPathRooted(configuration.OutputFileName) ? configuration.OutputFileName : Path.GetFullPath($"{Path.GetDirectoryName(configurationFileName)}/{configuration.OutputFileName}");
            var outputParentPath = Directory.GetParent(outputPath);

            if (outputParentPath != null && !outputParentPath.Exists)
            {
                outputParentPath.Create();
            }

            await File.WriteAllTextAsync(outputPath, json);
        }

        protected abstract IDatabaseConnector CreateConnector(DbFirstConfiguration configuration);
    }
}