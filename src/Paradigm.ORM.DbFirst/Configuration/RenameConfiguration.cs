using System.Runtime.Serialization;

namespace Paradigm.ORM.DbFirst.Configuration
{
    [DataContract]
    public class RenameConfiguration
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NewName { get; set; }

        #endregion
    }
}