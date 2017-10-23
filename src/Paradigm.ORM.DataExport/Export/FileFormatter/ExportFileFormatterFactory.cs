using Paradigm.ORM.DataExport.Configuration;

namespace Paradigm.ORM.DataExport.Export.FileFormatter
{
    public static class ExportFileFormatterFactory
    {
        public static IExportFileFormatter Create(Configuration.Configuration configuration)
        {
            switch (configuration.DestinationFile.FileType)
            {
                case ExportFileType.Csv:
                    return new CsvExportFileFormatter(configuration);

                case ExportFileType.Json:
                    return new JsonExportFileFormatter(configuration);
            }

            return new DatabaseExportFileFormatter(configuration);
        }
    }
}