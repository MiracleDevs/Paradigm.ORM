using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
	[Table("address")]
	public interface IAddressTable
    {
        #region Properties
	
		[Column("Id", "int")]
		[Identity]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[PrimaryKey]
		int Id { get; }
	
		[Column("Address1", "varchar")]
		[NotNullable]
		[Size(200)]
		string Address1 { get; }
	
		[Column("Address2", "varchar")]
		[Size(200)]
		string Address2 { get; }
	
		[Column("AddressTypeId", "int")]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[ForeignKey("FK_Address_AddressTypeId", "address", "AddressTypeId", "addresstype", "Id")]
		int AddressTypeId { get; }
	
		[Column("City", "varchar")]
		[NotNullable]
		[Size(200)]
		string City { get; }
	
		[Column("State", "varchar")]
		[Size(200)]
		string State { get; }
	
		[Column("Country", "varchar")]
		[Size(200)]
		string Country { get; }
	
		[Column("Email", "varchar")]
		[Size(200)]
		string Email { get; }
	
		[Column("PhoneNumber", "varchar")]
		[Size(200)]
		string PhoneNumber { get; }
	
		[Column("FaxNumber", "varchar")]
		[Size(200)]
		string FaxNumber { get; }
	
		[Column("Attention", "text")]
		[Size(65535)]
		string Attention { get; }
	
		[Column("Contact", "text")]
		[Size(65535)]
		string Contact { get; }
	
		[Column("Notes", "text")]
		[Size(65535)]
		string Notes { get; }

		#endregion
    }
}