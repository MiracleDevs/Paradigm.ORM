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
    public class View: IView
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CatalogName { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public List<Column> Columns { get; protected set; }

        [DataMember]
        public List<Constraint> Constraints { get; protected set; }

        [IgnoreDataMember]
        public Database Database { get; protected set; }

        #endregion

        #region Constructor

        public View()
        {
            this.Columns = new List<Column>();
            this.Constraints = new List<Constraint>();
        }

        #endregion

        #region Public Methods

        public override string ToString() => $"View [{this.SchemaName}].[{this.Name}]";

        public virtual async Task ExtractSchemaAsync(Database database, DbFirstConfiguration configuration, ISchemaProvider provider, IMapper mapper)
        {
            this.Database = database;
            this.Columns = mapper.Map<List<Column>>(await provider.GetColumnsAsync(this.CatalogName, this.Name));

            var viewConfiguration = configuration.GetTableConfiguration(this);
            this.ProcessConstraintConfigurations(viewConfiguration);
            this.ProcessColumnConfigurations(viewConfiguration);
        }

        public virtual void ProcessResults(Database database)
        {
            foreach (var column in this.Columns)
                column.ProcessResults(database, this);

            foreach (var constraint in this.Constraints)
                constraint.ProcessResults(database, this);
        }

        #endregion

        #region Protected Methods

        protected void ProcessConstraintConfigurations(TableConfiguration configuration)
        {
            if (configuration == null)
                return;

            this.Constraints.AddRange(configuration.ConstraintsToAdd.Select(x => new Constraint
            {
                SchemaName = this.SchemaName,
                FromTableName = this.Name,
                FromColumnName = x.FromColumnName,
                ToTableName = x.ToTableName,
                ToColumnName = x.ToColumnName,
                Name = x.Name,
                Type = x.Type
            }));

            this.Constraints.RemoveAll(x => configuration.ConstraintsToRemove.Contains(x.Name));
        }

        protected void ProcessColumnConfigurations(TableConfiguration configuration)
        {
            if (configuration == null)
                return;

            this.Columns.AddRange(configuration.ColumnsToAdd.Select(x => new Column
            {
                SchemaName = this.SchemaName,
                TableName = this.Name,
                Name = x.Name,
                DataType = x.DataType,
                MaxSize = x.MaxSize,
                Precision = x.Precision,
                Scale = x.Scale,
                DefaultValue = x.DefaultValue,
                IsNullable = x.IsNullable,
                IsIdentity = x.IsIdentity,
            }));

            this.Columns.RemoveAll(x => configuration.ColumnsToRemove.Contains(x.Name));
        }

        #endregion
    }
}