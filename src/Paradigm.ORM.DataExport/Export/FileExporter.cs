using System.IO;
using Paradigm.ORM.Data.ValueProviders;
using Paradigm.ORM.DataExport.Configuration;
using Paradigm.ORM.DataExport.Export.FileFormatter;
using Paradigm.ORM.DataExport.Logging;

namespace Paradigm.ORM.DataExport.Export
{
    public class FileExporter : Exporter
    {
        #region Properties

        private IExportFileFormatter ExportFileFormatter { get; set; }

        #endregion

        #region Constructor

        public FileExporter(ILoggingService loggingService, Configuration.Configuration configuration, bool verbose) : base(loggingService, configuration, verbose)
        {
        }

        #endregion

        #region Protected Methods

        protected override void Initialize()
        {
            base.Initialize();
            this.ExportFileFormatter = ExportFileFormatterFactory.Create(this.Configuration);

            if (this.Configuration.DestinationFile.FileMode == ExportFileMode.Append)
            {
                File.WriteAllText(this.Configuration.DestinationFile.FileName, string.Empty);
            }
        }

        protected override void ProcessData(TableData tableData, IValueProvider valueProvider)
        {
            var fileName = this.Configuration.DestinationFile.FileMode == ExportFileMode.Create
                ? string.Format(this.Configuration.DestinationFile.FileName, tableData.TableDescriptor.TableName)
                : this.Configuration.DestinationFile.FileName;

            var path = Path.GetDirectoryName(fileName);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                if (this.Verbose)
                    this.LoggingService.WriteLine($"The directory [{path}] didn't exist and was created.");
            }

            if (this.Verbose)
                this.LoggingService.WriteLine($"Writing export to file [{fileName}]");


            if (this.Configuration.DestinationFile.FileMode == ExportFileMode.Create)
                File.WriteAllText(fileName, this.ExportFileFormatter.GetFomattedString(tableData, valueProvider));
            else
                File.AppendAllText(fileName, this.ExportFileFormatter.GetFomattedString(tableData, valueProvider));
        }

        #endregion
    }
}