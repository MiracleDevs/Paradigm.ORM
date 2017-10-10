using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
	[Table("project")]
	public interface IProjectTable
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
	
		[Column("RegistrationDate", "datetime")]
		[NotNullable]
		[Range("1000-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
		DateTime RegistrationDate { get; }
	
		[Column("ClientId", "int")]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[ForeignKey("FK_Project_ClientId", "project", "ClientId", "client", "Id")]
		int ClientId { get; }
	
		[Column("Active", "tinyint")]
		[NotNullable]
		[Range("-128", "127")]
		[Numeric(3, 0)]
		bool Active { get; }
	
		[Column("Notes", "text")]
		[Size(65535)]
		string Notes { get; }
	
		[Column("CreationUserId", "int")]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[ForeignKey("FK_Project_CreationUser", "project", "CreationUserId", "user", "Id")]
		int CreationUserId { get; }
	
		[Column("CreationDate", "datetime")]
		[NotNullable]
		[Range("1000-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
		DateTime CreationDate { get; }
	
		[Column("ModificationUserId", "int")]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[ForeignKey("FK_Project_MOdificationUser", "project", "ModificationUserId", "user", "Id")]
		int ModificationUserId { get; }
	
		[Column("ModificationDate", "datetime")]
		[NotNullable]
		[Range("1000-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
		DateTime ModificationDate { get; }

		#endregion
    }
}