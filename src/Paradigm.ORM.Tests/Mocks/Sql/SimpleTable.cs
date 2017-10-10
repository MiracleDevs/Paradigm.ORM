using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Sql
{
    [Table(Catalog = "Test", Schema = "dbo")]
    public class SimpleTable : ISimpleTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "nvarchar")]
        public string Name { get; set; }

        [Column(Type = "bit")]
        public bool IsActive { get; set; }

        [Column(Type = "decimal")]
        public decimal Amount { get; set; }

        [Column(Type = "date")]
        public DateTime CreatedDate { get; set; }

    }
}