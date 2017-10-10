using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Paradigm.ORM.DbFirst.Schema;

namespace Paradigm.ORM.DbFirst.Configuration
{
    [DataContract]
    public class DbFirstConfiguration
    {
        #region Properties

        [DataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public string DatabaseType { get; set; }

        [DataMember]
        public string OutputFileName { get; set; }

        [DataMember]
        public string DatabaseName { get; set; }

        [DataMember]
        public List<StoredProcedureConfiguration> StoredProcedures { get; set; }

        [DataMember]
        public List<TableConfiguration> Tables { get; set; }

        [DataMember]
        public List<TableConfiguration> Views { get; set; }


        #endregion

        #region Constructor

        public DbFirstConfiguration()
        {
            this.Tables = new List<TableConfiguration>();
            this.Views = new List<TableConfiguration>();
            this.StoredProcedures = new List<StoredProcedureConfiguration>();
        }

        #endregion

        #region Public Method

        public TableConfiguration GetTableConfiguration(Table table)
        {
            return this.Tables.FirstOrDefault(x => x.Name == table.Name);
        }

        public TableConfiguration GetTableConfiguration(View table)
        {
            return this.Views.FirstOrDefault(x => x.Name == table.Name);
        }

        public TableConfiguration GetTableConfiguration(Column column)
        {
            return this.Tables.FirstOrDefault(x => x.Name == column.TableName) ??
                   this.Views.FirstOrDefault(x => x.Name == column.TableName);
        }

        public TableConfiguration GetTableConfiguration(string toTableName)
        {
            return this.Tables.FirstOrDefault(x => x.Name == toTableName) ??
                   this.Views.FirstOrDefault(x => x.Name == toTableName);
        }

        public TableConfiguration GetTableConfiguration(Constraint constraint)
        {
            return this.Tables.FirstOrDefault(x => x.Name == constraint.FromTableName) ??
                   this.Views.FirstOrDefault(x => x.Name == constraint.FromTableName);
        }

        public StoredProcedureConfiguration GetStoredProcedureConfiguration(StoredProcedure storedProcedure)
        {
            return this.StoredProcedures.FirstOrDefault(x => x.Name == storedProcedure.Name);
        }

        public StoredProcedureConfiguration GetStoredProcedureConfiguration(Parameter parameter)
        {
            return this.StoredProcedures.FirstOrDefault(x => x.Name == parameter.Name);
        }

        #endregion
    }
}