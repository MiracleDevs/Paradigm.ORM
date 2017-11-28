using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.ValueProviders;
using Paradigm.ORM.DataExport.Logging;

namespace Paradigm.ORM.DataExport.Export
{
    public class DatabaseExporter : Exporter
    {
        #region Properties

        private IDatabaseConnector DestinationDatabase { get; set; }

        private IDatabaseTransaction Transaction { get; set; }

        #endregion

        #region Constructor

        public DatabaseExporter(ILoggingService loggingService, Configuration.Configuration configuration, bool verbose) : base(loggingService, configuration, verbose)
        {
        }

        #endregion

        #region Public Methods

        public override void Dispose()
        {
            base.Dispose();
            this.Transaction?.Dispose();
            this.DestinationDatabase?.Dispose();

        }

        #endregion

        #region Protected Methods

        protected override void Initialize()
        {
            base.Initialize();

            if (this.Verbose)
                this.LoggingService.WriteLine("\nOpening Destination Database Connection...");

            this.DestinationDatabase = ConnectorFactory.Create(this.Configuration.DestinationDatabase.DatabaseType);
            this.DestinationDatabase.Initialize(this.Configuration.DestinationDatabase.ConnectionString);

            if (!this.DestinationDatabase.IsOpen())
                this.DestinationDatabase.Open();

            this.Transaction = this.DestinationDatabase.CreateTransaction();

            if (this.Verbose)
                this.LoggingService.WriteLine("Destination Database Connection openend.");
        }

        protected override void FinishExport()
        {
            base.FinishExport();
            this.Transaction.Commit();
        }

        protected override void ProcessData(TableData tableData, IValueProvider valueProvider)
        {
            var commandBuilder = new InsertCommandBuilder(this.DestinationDatabase, tableData.TableDescriptor);

            if (this.Verbose)
            {
                this.LoggingService.WriteLine("Command Created...");
            }

            while (valueProvider.MoveNext())
            {
                using (var command = commandBuilder.GetCommand(valueProvider))
                {
                    command.ExecuteNonQuery();
                }
            }

            if (this.Verbose)
            {
                this.LoggingService.WriteLine("Inserts Finished.");
            }

        }

        #endregion
    }
}