using System.Runtime.Serialization;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.DbFirst.Configuration
{
    [DataContract]
    public class ConstraintConfiguration
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ConstraintType Type { get; set; }

        [DataMember]
        public string FromColumnName { get; set; }

        [DataMember]
        public string ToTableName { get; set; }

        [DataMember]
        public string ToColumnName { get; set; }

        #endregion
    }
}