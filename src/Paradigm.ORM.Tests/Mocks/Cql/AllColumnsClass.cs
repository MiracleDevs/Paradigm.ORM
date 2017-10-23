using System;
using System.Net;
using System.Numerics;
using Cassandra;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Cql
{
    [Table("allcolumns", Catalog = "test")]
    public class AllColumnsClass
    {
        [PrimaryKey]
        [Column("column01", "uuid")]
        public Guid Id { get; set; }

        [Column("column02", "ascii")]
        public string Ascii { get; set; }

        [Column("column03", "bigint")]
        public long BigInt { get; set; }

        [Column("column04", "blob")]
        public byte[] Blob { get; set; }

        [Column("column05", "boolean")]
        public bool Boolean { get; set; }

        [Column("column06", "date")]
        public DateTime Date { get; set; }

        [Column("column07", "decimal")]
        public decimal Decimal { get; set; }

        [Column("column08", "double")]
        public double Double { get; set; }

        [Column("column09", "float")]
        public float Float { get; set; }

        [Column("column10", "inet")]
        public IPAddress Inet { get; set; }

        [Column("column11", "int")]
        public int Int { get; set; }

        [Column("column12", "smallint")]
        public short SmallInt { get; set; }

        [Column("column13", "text")]
        public string Text { get; set; }

        [Column("column14", "time")]
        public TimeSpan Time { get; set; }

        [Column("column15", "timestamp")]
        public DateTime Timestamp { get; set; }

        [Column("column16", "timeuuid")]
        public Guid TimeUuid { get; set; }

        [Column("column17", "tinyint")]
        public sbyte TinyInt { get; set; }

        [Column("column18", "varchar")]
        public string VarChar { get; set; }

        [Column("column19", "varint")]
        public BigInteger VarInt { get; set; }
    }
}