using System;
using System.Linq;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.ValueProviders;
using Paradigm.ORM.DataExport.Logging;

namespace Paradigm.ORM.DataExport.Export
{
    public abstract class Exporter : IExporter
    {
        #region Properties

        protected ILoggingService LoggingService { get; }

        protected bool Verbose { get; }

        protected Configuration.Configuration Configuration { get; }

        private IDatabaseConnector SourceDatabase { get; set; }

        #endregion

        #region Constructor

        protected Exporter(ILoggingService loggingService, Configuration.Configuration configuration, bool verbose)
        {
            this.LoggingService = loggingService ?? throw new ArgumentNullException(nameof(loggingService));
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.Verbose = verbose;
        }

        #endregion

        #region Public Methods

        public virtual void Dispose()
        {
            this.SourceDatabase?.Dispose();
        }

        public virtual void Export()
        {
            this.Initialize();

            foreach (var tableName in this.Configuration.TableNames)
            {
                if (this.Verbose)
                    this.LoggingService.Notice($"\nTable {tableName}");

                using (var tableData = new TableData(this.Configuration.SourceDatabase.DatabaseName, tableName, this.SourceDatabase))
                {
                    if (this.Verbose)
                    {
                        this.LoggingService.WriteLine("Table schema extracted.");
                        this.LoggingService.WriteLine($"Columns: [{tableData.TableDescriptor.AllColumns.Count}]");
                        this.LoggingService.WriteLine($"Primary Keys: [{string.Join(",", tableData.TableDescriptor.PrimaryKeyColumns.Select(x => x.ColumnName))}]");
                        this.LoggingService.WriteLine("Reading data...");
                    }

                    var valueProvider = tableData.Read();

                    if (this.Verbose)
                        this.LoggingService.WriteLine("Data reading successfull.");

                    this.ProcessData(tableData, valueProvider);

                    if (this.Verbose)
                        this.LoggingService.WriteLine("Data export successfull.");

                }
            }

            this.FinishExport();
        }

        #endregion

        #region Protected Methods

        protected virtual void Initialize()
        {
            if (this.Verbose)
                this.LoggingService.WriteLine("\nOpening Source Database Connection...");

            this.SourceDatabase = ConnectorFactory.Create(this.Configuration.SourceDatabase.DatabaseType);
            this.SourceDatabase.Initialize(this.Configuration.SourceDatabase.ConnectionString);

            if (this.Verbose)
                this.LoggingService.WriteLine("Source Database Connection openend.");
        }

        protected virtual void FinishExport()
        {

        }

        protected abstract void ProcessData(TableData tableData, IValueProvider valueProvider);

        #endregion
    }
}