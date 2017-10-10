using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
	[Table("client")]
	public interface IClientTable
    {
        #region Properties
	
		[Column("Id", "int")]
		[Identity]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[PrimaryKey]
		int Id { get; }
	
		[Column("Name", "varchar")]
		[NotNullable]
		[Size(200)]
		string Name { get; }
	
		[Column("ContactName", "varchar")]
		[Size(200)]
		string ContactName { get; }
	
		[Column("ContactEmail", "varchar")]
		[Size(200)]
		string ContactEmail { get; }
	
		[Column("ContactPhone", "varchar")]
		[Size(200)]
		string ContactPhone { get; }
	
		[Column("AddressId", "int")]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[ForeignKey("FK_Client_Address", "client", "AddressId", "address", "Id")]
		int? AddressId { get; }
	
		[Column("RegistrationDate", "datetime")]
		[Range("1000-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
		DateTime? RegistrationDate { get; }
	
		[Column("LogoFileId", "int")]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[ForeignKey("FK_Client_File", "client", "LogoFileId", "file", "Id")]
		int? LogoFileId { get; }
	
		[Column("Notes", "text")]
		[Size(65535)]
		string Notes { get; }
	
		[Column("HourlyRate", "decimal")]
		[NotNullable]
		[Numeric(20, 9)]
		decimal HourlyRate { get; }
	
		[Column("CreationUserId", "int")]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[ForeignKey("FK_Client_CreationUser", "client", "CreationUserId", "user", "Id")]
		int CreationUserId { get; }
	
		[Column("CreationDate", "datetime")]
		[NotNullable]
		[Range("1000-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
		DateTime CreationDate { get; }
	
		[Column("ModificationUserId", "int")]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[ForeignKey("FK_Client_MOdificationUser", "client", "ModificationUserId", "user", "Id")]
		int ModificationUserId { get; }
	
		[Column("ModificationDate", "datetime")]
		[NotNullable]
		[Range("1000-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
		DateTime ModificationDate { get; }

		#endregion
    }
}