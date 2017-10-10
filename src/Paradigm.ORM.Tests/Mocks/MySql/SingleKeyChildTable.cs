using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql
{
    [Table("singlekeychildtable", Catalog ="test")]
    public class SingleKeyChildTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "int")]
        public int ParentId { get; set; }

        [Column(Type = "nvarchar")]
        public string Name { get; set; }

        [Column(Type = "tinyint")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "datetime")]
        public DateTime CreatedDate { get; set; }
    }
}