using System;
using Paradigm.ORM.DataExport.Configuration;
using Paradigm.ORM.DataExport.Logging;

namespace Paradigm.ORM.DataExport.Export
{
    public static class ExporterFactory
    {
        public static Exporter Create(ILoggingService loggingService, Configuration.Configuration configuration, bool verbose)
        {
            switch (configuration.ExportType)
            {
                case ExportType.File:
                    return new FileExporter(loggingService, configuration, verbose);

                case ExportType.Database:
                    return new DatabaseExporter(loggingService, configuration, verbose);

                default:
                    throw new ArgumentOutOfRangeException(nameof(configuration.ExportType), configuration.ExportType, null);
            }
        }
    }
}