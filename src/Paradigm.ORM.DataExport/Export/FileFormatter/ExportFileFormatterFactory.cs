using Paradigm.ORM.DataExport.Configuration;

namespace Paradigm.ORM.DataExport.Export.FileFormatter
{
    public static class ExportFileFormatterFactory
    {
        public static IExportFileFormatter Create(Configuration.Configuration configuration)
        {
            if (configuration.DestinationFile.FileType == ExportFileType.Csv)
                return new CsvExportFileFormatter(configuration);

            if (configuration.DestinationFile.FileType == ExportFileType.Json)
                return new JsonExportFileFormatter(configuration);

            return new DatabaseExportFileFormatter(configuration);
        }
    }
}