using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Cql
{
    [Table]
    public class TwoPrimaryKeyTable : ITwoPrimaryKeyTable
    {
        [Column(Type = "int")]
        [PrimaryKey]
        public int Id { get; set; }

        [Column(Type = "int")]
        [PrimaryKey]
        public int Id2 { get; set; }

        [Column(Type = "text")]
        public string Name { get; set; }
    }
}