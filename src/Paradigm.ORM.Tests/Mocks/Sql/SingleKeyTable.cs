using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Sql
{
    [Table(Catalog = "Test", Schema = "dbo")]
    public class SingleKeyTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "nvarchar")]
        public string Name { get; set; }

        [Column(Type = "bit")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "date")]
        public DateTime CreatedDate { get; set; }

        [Column(Type = "bit")]
        public bool  BoolProperty { get; set; }

        [Column(Type = "tinyint")]
        public byte TinyintProperty { get; set; }

        [Column(Type = "smallint")]
        public short SmallintProperty { get; set; }

        [Column(Type = "bigint")]
        public long BigintProperty { get; set; }

        [Column(Type = "real")]
        public float RealProperty { get; set; }

        [Column(Type = "float")]
        public double FloatProperty { get; set; }

        [Column(Type = "money")]
        public decimal MoneyProperty { get; set; }

        [Column(Type = "smallmoney")]
        public decimal SmallmoneyProperty { get; set; }

        [Column(Type = "numeric")]
        public decimal NumericProperty { get; set; }

        [Column(Type = "decimal")]
        public decimal DecimalProperty { get; set; }

        [Column(Type = "date")]
        public DateTime DateProperty { get; set; }

        [Column(Type = "datetime")]
        public DateTime DateTimeProperty { get; set; }

        [Column(Type = "datetime2")]
        public DateTime DateTime2Property { get; set; }

        [Column(Type = "smalldatetime")]
        public DateTime SmallDateTimeProperty { get; set; }

        [Column(Type = "datetimeoffset")]
        public DateTimeOffset DateTimeOffsetProperty { get; set; }

        [Column(Type = "char")]
        public string CharProperty { get; set; }

        [Column(Type = "text")]
        public string TextProperty { get; set; }
    
        [Column(Type = "varchar")]
        public string VarcharProperty { get; set; }
    
        [Column(Type = "nchar")]
        public string NCharProperty { get; set; }
    
        [Column(Type = "ntext")]
        public string NTextProperty { get; set; }
    
        [Column(Type = "nvarchar")]
        public string NVarcharProperty { get; set; }
    
        [Column(Type = "xml")]
        public string XmlProperty { get; set; }

        [Column(Type = "binary")]
        public byte[] BinaryProperty { get; set; }

        [Column(Type = "varbinary")]
        public byte[] VarBinaryProperty { get; set; }

        [Column(Type = "image")]
        public byte[] ImageProperty { get; set; }
    }
}