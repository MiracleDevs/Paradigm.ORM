using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.PostgreSql
{
    [Table]
    public class SimpleTable : ISimpleTable
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

    }
}