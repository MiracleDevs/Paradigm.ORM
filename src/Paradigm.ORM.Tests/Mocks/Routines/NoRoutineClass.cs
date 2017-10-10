using System.Runtime.Serialization;

namespace Paradigm.ORM.Tests.Mocks.Routines
{
    [DataContract]
    public partial class NoRoutineClass
    {
        #region Public Properties

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        #endregion
    }
}
