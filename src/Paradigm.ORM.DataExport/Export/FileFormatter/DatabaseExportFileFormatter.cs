using System;
using System.Text;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.ValueProviders;
using Paradigm.ORM.DataExport.Configuration;

namespace Paradigm.ORM.DataExport.Export.FileFormatter
{
    public class DatabaseExportFileFormatter : ExportFileFormatterBase
    {
        #region Properties

        private IDatabaseConnector Connector { get; }

        private ICommandFormatProvider FormatProvider { get; }

        #endregion

        #region Constructor

        public DatabaseExportFileFormatter(Configuration.Configuration configuration) : base(configuration)
        {
            this.Connector = ConnectorFactory.Create(this.GetDatabaseType());
            this.Connector.Initialize();
            this.FormatProvider = this.Connector.GetCommandFormatProvider();
        }

        #endregion

        #region Public Methods

        public override string GetFomattedString(TableData tableData, IValueProvider valueProvider)
        {
            var builder = new StringBuilder();

            using (var insertCommandBuilder = new InsertCommandBuilder(this.Connector, tableData.TableDescriptor))
            {
                while (valueProvider.MoveNext())
                {
                    var command = insertCommandBuilder.GetCommand(valueProvider);
                    var commandText = command.CommandText;

                    foreach (var parameter in command.Parameters)
                    {
                        commandText = commandText.Replace(parameter.ParameterName, this.FormatProvider.GetColumnValue(parameter.Value, parameter.Value.GetType()));
                    }

                    builder.Append(commandText);
                    builder.AppendLine(this.FormatProvider.GetQuerySeparator());
                }
            }

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        private DatabaseType GetDatabaseType()
        {
            switch (this.Configuration.DestinationFile.FileType)
            {
                case ExportFileType.SqlServer:
                    return DatabaseType.SqlServer;

                case ExportFileType.MySql:
                    return DatabaseType.MySql;

                case ExportFileType.PostgreSql:
                    return DatabaseType.PostgreSql;

                case ExportFileType.Cassandra:
                    return DatabaseType.Cassandra;

                default:
                    throw new ArgumentOutOfRangeException(nameof(this.Configuration.DestinationFile.FileType), this.Configuration.DestinationFile.FileType, null);
            }
        }

        #endregion
    }
}