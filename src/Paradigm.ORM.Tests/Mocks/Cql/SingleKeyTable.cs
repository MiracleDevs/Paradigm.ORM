using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Cql
{
    [Table(Catalog = "Test", Schema = "dbo")]
    public class SingleKeyTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        public int Id { get; set; }

        [Column(Type = "boolean")]
        public bool BooleanProperty { get; set; }

        [Column(Type = "tinyint")]
        public byte TinyintProperty { get; set; }

        [Column(Type = "smallint")]
        public short SmallintProperty { get; set; }

        [Column(Type = "int")]
        public int IntProperty { get; set; }

        [Column(Type = "bigint")]
        public long BigintProperty { get; set; }

        [Column(Type = "float")]
        public double FloatProperty { get; set; }

        [Column(Type = "double")]
        public double DoubleProperty { get; set; }

        [Column(Type = "decimal")]
        public decimal DecimalProperty { get; set; }

        [Column(Type = "date")]
        public DateTime DateProperty { get; set; }

        [Column(Type = "time")]
        public TimeSpan TimeProperty { get; set; }

        [Column(Type = "timestamp")]
        public DateTime TimestampProperty { get; set; }

        [Column(Type = "text")]
        public string TextProperty { get; set; }
      
        [Column(Type = "blob")]
        public byte[] BinaryProperty { get; set; }
    }
}