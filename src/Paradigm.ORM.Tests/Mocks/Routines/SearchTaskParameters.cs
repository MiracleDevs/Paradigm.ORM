using System.Runtime.Serialization;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Routines
{
	[DataContract]
    [Routine("SearchTask")]
	public class SearchTaskParameters
    {
        #region Properties
	
		[DataMember]
		[Parameter("Name", "text", true)]
		public string Name { get; set; }
	
		[DataMember]
		[Parameter("ClientId", "int", true)]
		public int? ClientId { get; set; }
	
		[DataMember]
		[Parameter("ProjectId", "int", true)]
		public int? ProjectId { get; set; }
	
		[DataMember]
		[Parameter("TypeId", "int", true)]
		public int? TypeId { get; set; }
	
		[DataMember]
		[Parameter("ComplexityId", "int", true)]
		public int? ComplexityId { get; set; }
	
		[DataMember]
		[Parameter("IsBillable", "tinyint", true)]
		public bool? IsBillable { get; set; }
	
		[DataMember]
		[Parameter("Active", "tinyint", true)]
		public bool? Active { get; set; }
	
		[DataMember]
		[Parameter("MaxTasks", "int", true)]
		public int? MaxTasks { get; set; }

        [DataMember]
        [Parameter("NumericParam", "int", false)]
        [Numeric(10, 20)]
        public int? NumericParam { get; set; }

        [DataMember]
        [Parameter("StringParam", "text", false)]
        [Size(200)]
        public int? StringParam { get; set; }

        [DataMember]
        [Parameter("NoInputParam", "int", false)]
        public int? NoInputParam { get; set; }

        public int? NoParameter { get; set; }
        #endregion
    }
}