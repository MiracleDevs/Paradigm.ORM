using System.Runtime.Serialization;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
    [DataContract]
    public partial class NoTableClass
    {
        #region Public Properties

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        #endregion
    }
}
