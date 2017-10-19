using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Cql
{
    [Table("singlekeyparenttable", Catalog = "test")]
    public class SingleKeyParentTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        public int Id { get; set; }

        [Column(Type = "text")]
        public string Name { get; set; }

        [Column(Type = "boolean")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "date")]
        public DateTimeOffset? CreatedDate { get; set; }

        [Navigation(typeof(SingleKeyChildTable), nameof(Id), nameof(SingleKeyChildTable.ParentId))]
        public List<SingleKeyChildTable> Childs { get; set; }
    }
}