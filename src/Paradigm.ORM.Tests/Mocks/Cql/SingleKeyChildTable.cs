using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Cql
{
    [Table("singlekeychildtable", Catalog ="test")]
    public class SingleKeyChildTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        public int Id { get; set; }

        [Column(Type = "int")]
        public int ParentId { get; set; }

        [Column(Type = "text")]
        public string Name { get; set; }

        [Column(Type = "boolean")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "datetime")]
        public DateTimeOffset? CreatedDate { get; set; }
    }
}