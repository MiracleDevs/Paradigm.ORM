using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Paradigm.Core.Mapping;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Schema
{
    [DataContract]
    public class Database
    {
        #region Properties

        [DataMember]
        public List<Table> Tables { get; set; }

        [DataMember]
        public List<View> Views { get; set; }

        [DataMember]
        public List<StoredProcedure> StoredProcedures { get; set; }

        [IgnoreDataMember]
        private IDatabaseConnector Connector { get; }

        #endregion

        #region Constructor

        public Database(IDatabaseConnector connector)
        {
            this.Tables = new List<Table>();
            this.Views = new List<View>();
            this.Connector = connector;
        }

        #endregion

        #region Public Methods

        public async Task ExtractSchemaAsync(DbFirstConfiguration configuration)
        {
            var provider = this.Connector.GetSchemaProvider();
            var mapper = Mapper.Container;

            var tables = configuration.Tables ?? new List<TableConfiguration>();
            var views = configuration.Views ?? new List<TableConfiguration>();
            var storedProcedures = configuration.StoredProcedures ?? new List<StoredProcedureConfiguration>();

            this.Tables = tables.Any() 
                ? mapper.Map<List<Table>>(await provider.GetTablesAsync(configuration.DatabaseName, tables.Select(x => x.Name).ToArray())) 
                : new List<Table>();

            this.Views = views.Any() 
                ? mapper.Map<List<View>>(await provider.GetViewsAsync(configuration.DatabaseName, views.Select(x => x.Name).ToArray())) 
                : new List<View>();

            this.StoredProcedures = storedProcedures.Any() 
                ? mapper.Map<List<StoredProcedure>>(await provider.GetStoredProceduresAsync(configuration.DatabaseName, storedProcedures.Select(x => x.Name).ToArray())) 
                : new List<StoredProcedure>();

            foreach (var table in tables)
            {
                if (this.Tables.Any(x => x.Name == table.Name))
                    continue;

                this.Tables.Add(new Table { Name = table.Name, SchemaName = configuration.DatabaseName });
            }

            foreach (var view in views)
            {
                if (this.Views.Any(x => x.Name == view.Name))
                    continue;

                this.Views.Add(new View { Name = view.Name, SchemaName = configuration.DatabaseName });
            }

            foreach (var storedProcedure in storedProcedures)
            {
                if (this.StoredProcedures.Any(x => x.Name == storedProcedure.Name))
                    continue;

                this.StoredProcedures.Add(new StoredProcedure { Name = storedProcedure.Name, Type = storedProcedure.Type, SchemaName = configuration.DatabaseName });
            }

            foreach (var table in this.Tables)
                await table.ExtractSchemaAsync(this, configuration, provider, mapper);

            foreach (var view in this.Views)
                await view.ExtractSchemaAsync(this, configuration, provider, mapper);

            foreach (var storedProcedure in this.StoredProcedures)
                await storedProcedure.ExtractSchemaAsync(this, configuration, provider, mapper);

        }

        public void ProcessResults()
        {
            foreach (var table in this.Tables)
                table.ProcessResults(this);

            foreach (var view in this.Views)
                view.ProcessResults(this);

            foreach (var storedProcedure in this.StoredProcedures)
                storedProcedure.ProcessResults(this);
        }

        #endregion
    }
}