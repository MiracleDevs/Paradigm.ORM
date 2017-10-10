using System.Runtime.Serialization;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.DbFirst.Schema
{
    [DataContract]
    public class Parameter : IParameter
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CatalogName { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public string StoredProcedureName { get; set; }

        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public long MaxSize { get; set; }

        [DataMember]
        public byte Precision { get; set; }

        [DataMember]
        public byte Scale { get; set; }

        [DataMember]
        public bool IsInput { get; set; }

        [IgnoreDataMember]
        public Database Database { get; private set; }

        [IgnoreDataMember]
        public StoredProcedure StoredProcedure { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString() => $"Column [{this.StoredProcedureName}].[{this.Name}]";

        public void ProcessResults(Database database, StoredProcedure storedProcedure)
        {
            this.Database = database;
            this.StoredProcedure = storedProcedure;
        }

        #endregion
    }
}