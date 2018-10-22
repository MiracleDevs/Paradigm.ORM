using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Cql
{
    [Table(Name = "TwoPrimaryKeyTable", Catalog = "test")]
    public class TwoPrimaryKeyTable : ITwoPrimaryKeyTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        public int Id1 { get; set; }

        [Column(Type = "int")]
        [PrimaryKey]
        public int Id2 { get; set; }

        [Column(Type = "text")]
        public string Name { get; set; }

        [Column(Type = "timestamp")]
        [PrimaryKey]
        public DateTimeOffset Date { get; set; }
    }
}