using System.Runtime.Serialization;

namespace Paradigm.ORM.DataExport.Configuration
{
    [DataContract]
    public class ExportFileConfiguration
    {
        [DataMember]
        public ExportFileType FileType { get; set; }

        [DataMember]
        public ExportFileMode FileMode { get; set; }

        [DataMember]
        public string FileName { get; set; }
    }
}