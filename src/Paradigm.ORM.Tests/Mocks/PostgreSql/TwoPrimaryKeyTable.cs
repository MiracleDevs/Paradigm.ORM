using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.PostgreSql
{
    [Table]
    public class TwoPrimaryKeyTable : ITwoPrimaryKeyTable
    {
        [Column(Type = "integer")]
        [PrimaryKey]
        public int Id1 { get; set; }

        [Column(Type = "integer")]
        [PrimaryKey]
        public int Id2 { get; set; }

        [Column(Type = "character varying")]
        public string Name { get; set; }
    }
}