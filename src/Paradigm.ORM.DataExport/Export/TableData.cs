using System;
using System.Linq;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.DataExport.Export
{
    public class TableData : IDisposable
    {
        #region Properties

        private string DatabaseName { get; }

        private string TableName { get; }

        private IDatabaseConnector Connector { get; }

        private ISelectCommandBuilder SelectCommandBuilder { get; set; }

        private IDatabaseReader Reader { get; set; }

        public TableDescriptor TableDescriptor { get; set; }

        #endregion

        #region Constructor

        public TableData(string databaseName, string tableName, IDatabaseConnector connector)
        {
            this.DatabaseName = databaseName ?? throw new ArgumentNullException(nameof(databaseName));
            this.TableName = tableName ?? throw new ArgumentNullException(nameof(tableName));
            this.Connector = connector ?? throw new ArgumentNullException(nameof(connector));

            this.Extract();
        }

        #endregion

        #region Public Methods

        public void Dispose()
        {
            this.SelectCommandBuilder?.Dispose();
            this.Reader?.Dispose();
        }

        public IValueProvider Read()
        {
            this.Reader?.Dispose();
            this.Reader = this.SelectCommandBuilder.GetCommand().ExecuteReader();
            return new DatabaseReaderValueProvider(this.Connector, this.Reader);
        }

        #endregion

        #region Private Methods

        private void Extract()
        {
            if (!this.Connector.IsOpen())
                this.Connector.Open();

            var schemaProvider = this.Connector.GetSchemaProvider();
            var commandBuilderFactory = this.Connector.GetCommandBuilderFactory();

            var tableSchema = schemaProvider.GetTables(this.DatabaseName, this.TableName).First();
            var columnsSchema = schemaProvider.GetColumns(this.DatabaseName, this.TableName);
            var constraintsSchema = schemaProvider.GetConstraints(this.DatabaseName, this.TableName);

            this.TableDescriptor = new CustomTableDescriptor(tableSchema, columnsSchema, constraintsSchema);
            this.SelectCommandBuilder = commandBuilderFactory.CreateSelectCommandBuilder(this.TableDescriptor);
        }

        #endregion
    }
}