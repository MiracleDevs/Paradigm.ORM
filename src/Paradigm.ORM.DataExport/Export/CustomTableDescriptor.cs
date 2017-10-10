using System.Collections.Generic;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.Data.Descriptors;

namespace Paradigm.ORM.DataExport.Export
{
    public class CustomTableDescriptor : TableDescriptor
    {
        public override string CatalogName => null;

        public override string SchemaName => null;

        public CustomTableDescriptor(ITable table, List<IColumn> columns, List<IConstraint> constraints) : base(table, columns, constraints)
        {
        }
    }
}