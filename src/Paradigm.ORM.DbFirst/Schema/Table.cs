using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Paradigm.Core.Mapping.Interfaces;
using Paradigm.ORM.Data.Database.Schema;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Schema
{
    [DataContract]
    public class Table : View, ITable
    {
        #region Properties

        [DataMember]
        public List<Column> PrimaryKeys { get; private set; }

        #endregion

        #region Constructor
        
        public Table()
        {
            this.PrimaryKeys = new List<Column>();
        }

        #endregion

        #region Public Methods

        public override string ToString() => $"Table [{this.SchemaName}].[{this.Name}]";

        public override async Task ExtractSchemaAsync(Database database, DbFirstConfiguration configuration, ISchemaProvider provider, IMapper mapper)
        {
            this.Database = database;

            this.Columns = mapper.Map<List<Column>>(await provider.GetColumnsAsync(this.CatalogName, this.Name));
            this.Constraints = mapper.Map<List<Constraint>>(await provider.GetConstraintsAsync(this.CatalogName, this.Name));

            var tableConfiguration = configuration.GetTableConfiguration(this);
            this.ProcessConstraintConfigurations(tableConfiguration);
            this.ProcessColumnConfigurations(tableConfiguration);
        }

        public override void ProcessResults(Database database)
        {
            foreach (var column in this.Columns)
                column.ProcessResults(database, this);

            foreach (var constraint in this.Constraints)
                constraint.ProcessResults(database, this);

            this.PrimaryKeys = this.Constraints
                .Where(x => x.Type == ConstraintType.PrimaryKey)
                .Select(x => this.Columns.FirstOrDefault(c => c.Name == x.FromColumnName))
                .ToList();
        }

        #endregion
    }
}