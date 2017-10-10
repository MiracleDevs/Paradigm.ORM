using System.Runtime.Serialization;

namespace Paradigm.ORM.DbFirst.Configuration
{
    public class ColumnConfiguration
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public int MaxSize { get; set; }

        [DataMember]
        public byte Precision { get; set; }

        [DataMember]
        public byte Scale { get; set; }

        [DataMember]
        public string DefaultValue { get; set; }

        [DataMember]
        public bool IsNullable { get; set; }

        [DataMember]
        public bool IsIdentity { get; set; }

        #endregion
    }
}