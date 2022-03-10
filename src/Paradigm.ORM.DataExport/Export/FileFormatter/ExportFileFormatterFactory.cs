using Paradigm.ORM.DataExport.Configuration;

namespace Paradigm.ORM.DataExport.Export.FileFormatter
{
    public static class ExportFileFormatterFactory
    {
        public static IExportFileFormatter Create(Configuration.Configuration configuration)
        {
            return configuration.DestinationFile.FileType switch
            {
                ExportFileType.Csv => new CsvExportFileFormatter(configuration),
                ExportFileType.Json => new JsonExportFileFormatter(configuration),
                _ => new DatabaseExportFileFormatter(configuration)
            };
        }
    }
}