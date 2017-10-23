using System;
using System.Text;
using Paradigm.Core.Extensions;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.DataExport.Export.FileFormatter
{
    public class JsonExportFileFormatter : ExportFileFormatterBase
    {
        #region Constructor

        public JsonExportFileFormatter(Configuration.Configuration configuration) : base(configuration)
        {
        }

        #endregion

        #region Public Methods

        public override string GetFomattedString(TableData tableData, IValueProvider valueProvider)
        {
            var builder = new StringBuilder();
            var first = true;

            builder.Append("[");
            while (valueProvider.MoveNext())
            {
                if (!first)
                    builder.AppendLine(",");

                var firstColumn = true;
                builder.AppendLine("{");

                foreach(var column in tableData.TableDescriptor.AllColumns)
                {
                    if (!firstColumn)
                        builder.AppendLine(",");

                    builder.AppendFormat("\t\"{0}\": {1}", column.ColumnName, this.FormatValue(valueProvider.GetValue(column)));

                    firstColumn = false;
                }

                builder.Append("\n}");
                first = false;
            }

            builder.Append("]");
            return builder.ToString();
        }

        #endregion

        #region Private Methods

        public string FormatValue(object value)
        {
            if (value == null || value == DBNull.Value)
                return "null";

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