using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.DataExport.Export.FileFormatter
{
    public interface IExportFileFormatter
    {
        string GetFomattedString(TableData tableData, IValueProvider valueProvider);
    }
}