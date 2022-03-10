using System;
using Paradigm.ORM.DataExport.Configuration;
using Paradigm.ORM.DataExport.Logging;

namespace Paradigm.ORM.DataExport.Export
{
    public static class ExporterFactory
    {
        public static Exporter Create(ILoggingService loggingService, Configuration.Configuration configuration, bool verbose)
        {
            return configuration.ExportType switch
            {
                ExportType.File => new FileExporter(loggingService, configuration, verbose),
                ExportType.Database => new DatabaseExporter(loggingService, configuration, verbose),
                _ => throw new ArgumentOutOfRangeException(nameof(configuration.ExportType), configuration.ExportType,
                    null)
            };
        }
    }
}