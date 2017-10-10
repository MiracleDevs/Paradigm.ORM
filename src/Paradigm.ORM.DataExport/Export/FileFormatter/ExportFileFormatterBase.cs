using System;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.DataExport.Export.FileFormatter
{
    public abstract class ExportFileFormatterBase : IExportFileFormatter
    {
        #region Properties

        protected Configuration.Configuration Configuration { get; }

        #endregion

        #region Constructor

        protected ExportFileFormatterBase(Configuration.Configuration configuration)
        {
            this.Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion

        #region Public Methods

        public abstract string GetFomattedString(TableData tableData, IValueProvider valueProvider);

        #endregion
    }
}