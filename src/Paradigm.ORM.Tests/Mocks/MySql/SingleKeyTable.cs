using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql
{
    [Table("singlekeytable", Catalog = "test")]
    public class SingleKeyTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "nvarchar")]
        public string Name { get; set; }

        [Column(Type = "boolean")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Column(Type = "tinyint")]
        public bool TinyIntProperty { get; set; }

        [Column(Type = "boolean")]
        public bool BoolProperty { get; set; }

        [Column(Type = "smallint")]
        public short SmallintProperty { get; set; }

        [Column(Type = "mediumint")]
        public int MediumIntProp { get; set; }

        [Column(Type = "int")]
        public int IntProp { get; set; }

        [Column(Type = "bigint")]
        public long BigIntProperty { get; set; }

        [Column(Type = "float")]
        public float FloatProperty { get; set; }

        [Column(Type = "double")]
        public double DoubleProperty { get; set; }

        [Column(Type = "year")]
        public short YearProperty { get; set; }

        [Column(Type = "time")]
        public TimeSpan TimeProperty { get; set; }

        [Column(Type = "date")]
        public DateTime DateProperty { get; set; }

        [Column(Type = "datetime")]
        public DateTime DatetimeProperty { get; set; }

        [Column(Type = "timestamp")]
        public DateTime TimestampProperty { get; set; }
        
        [Column(Type = "char")]
        public string CharProperty { get; set; }

        [Column(Type = "varchar")]
        public string VarcharProperty { get; set; }

        [Column(Type = "tinytext")]
        public string TinytextProperty { get; set; }

        [Column(Type = "text")]
        public string TextProperty { get; set; }

        [Column(Type = "mediumtext")]
        public string MediumtextProperty { get; set; }

        [Column(Type = "longtext")]
        public string LongtextProperty { get; set; }

        [Column(Type = "binary")]
        public byte[] BinaryProperty { get; set; }

        [Column(Type = "tinyblob")]
        public byte[] TinyBlobProperty { get; set; }

        [Column(Type = "blob")]
        public byte[] BlobProperty { get; set; }

        [Column(Type = "mediumblob")]
        public byte[] MediumBlobProperty { get; set; }

        [Column(Type = "longblob")]
        public byte[] LongBlobProperty { get; set; }

        [Column(Type = "varbinary")]
        public byte[] VarBinaryProperty { get; set; }
    }
}