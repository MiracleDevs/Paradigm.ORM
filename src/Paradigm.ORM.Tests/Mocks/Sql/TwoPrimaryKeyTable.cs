using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Sql
{
    [Table(Catalog = "Test", Schema = "dbo")]
    public class TwoPrimaryKeyTable : ITwoPrimaryKeyTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "int")]
        [PrimaryKey]
        [Identity]
        public int Id2 { get; set; }

        [Column(Type = "nvarchar")]
        public string Name { get; set; }
    }
}