using System;
using Paradigm.ORM.Data.Attributes;


namespace Paradigm.ORM.Tests.Mocks.Tables
{
	
	public interface ICustomMappingTable
    {
        #region Properties
	
		[Column("IntField", "int")]
		[Identity]
		[NotNullable]
		[Range("-2147483648", "2147483647")]
		[Numeric(10, 0)]
		[PrimaryKey]
		int IntField { get; set; }
	
		[Column("NVarcharField", "nvarchar")]
		[NotNullable]
		[Size(200)]
		string NVarcharField { get; set; }
	
		[Column("DatetimeField", "datetime")]
		[Range("1753-01-01T00:00:00.0000000", "9999-12-31T23:59:59.0000000")]
		DateTime? DatetimeField { get; set; }
	

		[Column("DecimalField", "decimal")]
		[NotNullable]
		[Numeric(20, 9)]
		decimal DecimalField { get; set; }

        byte DummyField { get; set; }

        #endregion
    }
}