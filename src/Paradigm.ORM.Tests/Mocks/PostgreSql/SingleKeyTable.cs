using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.PostgreSql
{
    [Table]
    public class SingleKeyTable
    {
        [Column(Type = "integer")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "character varying")]
        public string Name { get; set; }

        [Column(Type = "boolean")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "date")]
        public DateTime CreatedDate { get; set; }

        [Column(Type = "bigint")]
        public long BigintProperty { get; set; }

        [Column(Type = "int8")]
        public long Int8Property { get; set; }

        [Column(Type = "bigserial")]
        public long BigserialProperty { get; set; }

        [Column(Type = "serial8")]
        public long Serial8Property { get; set; }

        [Column(Type = "boolean")]
        public bool BooleanProperty { get; set; }

        [Column(Type = "bool")]
        public bool BoolProperty { get; set; }

        [Column(Type = "bytea")]
        public byte[] ByteaProperty { get; set; }

        [Column(Type = "int")]
        public int IntProperty { get; set; }

        [Column(Type = "integer")]
        public int IntegerProperty { get; set; }

        [Column(Type = "smallint")]
        public int SmallintProperty { get; set; }

        [Column(Type = "serial4")]
        public long Serial4Property { get; set; }

        [Column(Type = "float")]
        public float FloatProperty { get; set; }

        [Column(Type = "double")]
        public double DoubleProperty { get; set; }

        [Column(Type = "double precision")]
        public double DoubleprecisionProperty { get; set; }

        [Column(Type = "money")]
        public decimal MoneyProperty { get; set; }

        [Column(Type = "decimal")]
        public decimal DecimalProperty { get; set; }

        [Column(Type = "numeric")]
        public decimal NumericProperty { get; set; }

        [Column(Type = "real")]
        public float RealProperty { get; set; }

        [Column(Type = "date")]
        public DateTime DateProperty { get; set; }

        [Column(Type = "timestamp")]
        public DateTime TimestampProperty { get; set; }

        [Column(Type = "timestamp without time zone")]
        public DateTime TimestampNoTimeZoneProperty { get; set; }

        [Column(Type = "timestamp with time zone")]
        public DateTime TimestampTimeZoneProperty { get; set; }

        [Column(Type = "time")]
        public TimeSpan TimeProperty { get; set; }

        [Column(Type = "time without time zone")]
        public TimeSpan TimeNoTimeZoneProperty { get; set; }

        [Column(Type = "interval")]
        public TimeSpan IntervalProperty { get; set; }

        [Column(Type = "char")]
        public string CharProperty { get; set; }

        [Column(Type = "character")]
        public string CharacterProperty { get; set; }

        [Column(Type = "varchar")]
        public string VarcharProperty { get; set; }

        [Column(Type = "character varying")]
        public string CharacterVaryingProperty { get; set; }

        [Column(Type = "text")]
        public string TextProperty { get; set; }
    }
}