using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Paradigm.ORM.DbPublisher.Configuration
{
    [DataContract]
    public class PublishConfiguration
    {
        #region Properties

        [DataMember]
        public string ConnectionString { get; set; }

        [DataMember]
        public string DatabaseType { get; set; }

        [DataMember]
        public bool ExecuteScript { get; set; }

        [DataMember]
        public bool GenerateScript { get; set; }

        [DataMember]
        public string OutputFileName { get; set; }

        [DataMember]
        public List<string> Files { get; set; }

        [DataMember]
        public List<string> Paths { get; set; }

        [DataMember]
        public bool TopDirectoryOnly { get; set; }

        #endregion

        #region Constructor

        public PublishConfiguration()
        {
            this.Files = new List<string>();
            this.Paths = new List<string>();
        }

        #endregion
    }
}