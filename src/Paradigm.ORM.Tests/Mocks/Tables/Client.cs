using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
	[DataContract]
	[TableType(typeof(IClientTable))]
	public partial class Client : IClient, IClientTable
    {
		#region Public Properties

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string ContactName { get; set; }

		[DataMember]
		public string ContactEmail { get; set; }

		[DataMember]
		public string ContactPhone { get; set; }

		[DataMember]
		public int? AddressId { get; set; }

		[DataMember]
		public DateTime? RegistrationDate { get; set; }

		[DataMember]
		public int? LogoFileId { get; set; }

		[DataMember]
		public string Notes { get; set; }

		[DataMember]
		public decimal HourlyRate { get; set; }

		[DataMember]
		public int CreationUserId { get; set; }

		[DataMember]
		public DateTime CreationDate { get; set; }

		[DataMember]
		public int ModificationUserId { get; set; }

		[DataMember]
		public DateTime ModificationDate { get; set; }

		[DataMember]
		[Navigation(typeof(Address), "AddressId", "Id")]
		public IAddress Address { get; set; }

		[DataMember]
		[Navigation(typeof(Project), "Id", "ClientId")]
		public IReadOnlyCollection<IProject> Projects { get; set; }

		#endregion
    }
}
