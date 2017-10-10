using System;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
	[DataContract]
	[TableType(typeof(IProjectTable))]
	public partial class Project : IProject, IProjectTable
    {
		#region Public Properties

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public DateTime RegistrationDate { get; set; }

		[DataMember]
		public int ClientId { get; set; }

		[DataMember]
		public bool Active { get; set; }

		[DataMember]
		public string Notes { get; set; }

		[DataMember]
		public int CreationUserId { get; set; }

		[DataMember]
		public DateTime CreationDate { get; set; }

		[DataMember]
		public int ModificationUserId { get; set; }

		[DataMember]
		public DateTime ModificationDate { get; set; }

		#endregion
    }
}
