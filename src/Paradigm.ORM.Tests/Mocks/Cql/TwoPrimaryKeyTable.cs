using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Cql
{
    [Table]
    public class TwoPrimaryKeyTable : ITwoPrimaryKeyTable
    {
        [Column(Type = "integer")]
        [PrimaryKey]
        public int Id { get; set; }

        [Column(Type = "integer")]
        [PrimaryKey]
        public int Id2 { get; set; }

        [Column(Type = "text")]
        public string Name { get; set; }
    }
}