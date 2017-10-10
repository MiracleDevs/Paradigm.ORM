using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Paradigm.Core.Mapping.Interfaces;
using Paradigm.ORM.Data.Database.Schema;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Schema
{
    [DataContract]
    public class StoredProcedure : IStoredProcedure
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CatalogName { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public StoredProcedureType Type { get; set; }

        [DataMember]
        public List<Parameter> Parameters { get; private set; }

        [IgnoreDataMember]
        public Database Database { get; private set; }

        #endregion

        #region Constructor

        public StoredProcedure()
        {
            this.Parameters = new List<Parameter>();
        }

        #endregion

        #region Public Methods

        public override string ToString() => $"Stored Procedure [{this.SchemaName}].[{this.Name}]";

        public async Task ExtractSchemaAsync(Database database, DbFirstConfiguration configuration, ISchemaProvider provider, IMapper mapper)
        {
            this.Database = database;
            this.Parameters = mapper.Map<List<Parameter>>(await provider.GetParametersAsync(this.CatalogName, this.Name));
            var storedProcedure = configuration.GetStoredProcedureConfiguration(this);
            this.Type = storedProcedure.Type;
        }

        public void ProcessResults(Database database)
        {
            foreach (var parameter in this.Parameters)
                parameter.ProcessResults(database, this);
        }

        #endregion
    }
}