using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.ORM.DataExport.Configuration
{
    [DataContract]
    public class Configuration
    {
        [DataMember]
        public DatabaseConfiguration SourceDatabase { get; set; }

        [DataMember]
        public List<string> TableNames { get; set; }

        [DataMember]
        public ExportType ExportType { get; set; }

        [DataMember]
        public DatabaseConfiguration DestinationDatabase { get; set; }

        [DataMember]
        public ExportFileConfiguration DestinationFile { get; set; }
    }
}