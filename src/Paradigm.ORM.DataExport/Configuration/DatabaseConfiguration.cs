using System.Runtime.Serialization;

namespace Paradigm.ORM.DataExport.Configuration
{
    [DataContract]
    public class DatabaseConfiguration
    {
        [DataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public Database DatabaseType { get; set; }

        [DataMember]
        public string DatabaseName { get; set; }
    }
}