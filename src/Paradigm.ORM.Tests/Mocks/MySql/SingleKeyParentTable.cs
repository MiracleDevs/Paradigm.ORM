using System;
using System.Collections.Generic;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql
{
    [Table("singlekeyparenttable", Catalog = "test")]
    public class SingleKeyParentTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "nvarchar")]
        public string Name { get; set; }

        [Column(Type = "tinyint")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "datetime")]
        public DateTime CreatedDate { get; set; }

        [Navigation(typeof(SingleKeyChildTable), nameof(Id), nameof(SingleKeyChildTable.ParentId))]
        public List<SingleKeyChildTable> Childs { get; set; }
    }
}