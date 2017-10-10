using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.DbFirst.Schema;

namespace Paradigm.ORM.DbFirst.Configuration
{
    [DataContract]
    public class StoredProcedureConfiguration
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NewName { get; set; }

        [DataMember]
        public StoredProcedureType Type { get; set; }

        [DataMember]
        public List<RenameConfiguration> ParametersToRename { get; set; }

        [DataMember]
        public List<string> ResultTypes { get; set; }

        #endregion

        #region Constructor

        public StoredProcedureConfiguration()
        {
            this.ParametersToRename = new List<RenameConfiguration>();
            this.ResultTypes = new List<string>();
        }

        #endregion

        #region Public Methods

        public RenameConfiguration GetParameterRenameConfiguration(Parameter parameter)
        {
            return this.ParametersToRename.FirstOrDefault(x => x.Name == parameter.Name);
        }

        #endregion
    }
}