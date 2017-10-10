using System.Runtime.Serialization;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
	[DataContract]
	[TableType(typeof(IAddressTable))]
	public partial class Address : IAddress, IAddressTable
    {
		#region Public Properties

		[DataMember]
		public int Id { get; set; }

		[DataMember]
		public string Address1 { get; set; }

		[DataMember]
		public string Address2 { get; set; }

		[DataMember]
		public int AddressTypeId { get; set; }

		[DataMember]
		public string City { get; set; }

		[DataMember]
		public string State { get; set; }

		[DataMember]
		public string Country { get; set; }

		[DataMember]
		public string Email { get; set; }

		[DataMember]
		public string PhoneNumber { get; set; }

		[DataMember]
		public string FaxNumber { get; set; }

		[DataMember]
		public string Attention { get; set; }

		[DataMember]
		public string Contact { get; set; }

		[DataMember]
		public string Notes { get; set; }

		#endregion
    }
}
