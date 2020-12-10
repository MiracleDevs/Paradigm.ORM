using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql
{
    [Table("simple_table", Catalog = "test")]
    public class SimpleTable : ISimpleTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "varchar")]
        public string Name { get; set; }

        [Column(Type = "tinyint")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "date")]
        public DateTime CreatedDate { get; set; }

    }
}