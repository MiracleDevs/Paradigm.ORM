using System;
using System.Linq;
using System.Text;
using Paradigm.Core.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.DataExport.Export.FileFormatter
{
    public class CsvExportFileFormatter : ExportFileFormatterBase
    {
        #region Constructor

        public CsvExportFileFormatter(Configuration.Configuration configuration) : base(configuration)
        {
        }

        #endregion

        #region Public Methods

        public override string GetFomattedString(TableData tableData, IValueProvider valueProvider)
        {
            var builder = new StringBuilder();

            builder.AppendLine(string.Join(",", tableData.TableDescriptor.AllColumns.Select(x => x.ColumnName)));

            while (valueProvider.MoveNext())
            { 
                builder.AppendLine(string.Join(",", tableData.TableDescriptor.AllColumns.Select(x => this.FormatValue(valueProvider.GetValue(x)))));
            }

            return builder.ToString();
        }

        #endregion

        #region Private Methods

        public string FormatValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return string.Empty;

            if (value.GetType().IsNumeric())
                return value.ToString();

            switch (value)
            {
                case DateTime time:
                    return $"\"{time:yyyy-MM-dd hh:mm:ss}\"";
                case bool b:
                    return b ? "1" : "0";
            }

            var strValue = value.ToString() ?? string.Empty;

            strValue = strValue.Replace("\r\n", "\\n").Replace("\n", "\\n");

            return $"\"{strValue}\"";
        }

        #endregion
    }
}