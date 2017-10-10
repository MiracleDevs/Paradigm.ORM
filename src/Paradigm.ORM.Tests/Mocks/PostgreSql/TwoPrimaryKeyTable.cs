using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.PostgreSql
{
    [Table]
    public class TwoPrimaryKeyTable : ITwoPrimaryKeyTable
    {
        [Column(Type = "integer")]
        [PrimaryKey]
        [Identity]
        public int Id { get; set; }

        [Column(Type = "integer")]
        [PrimaryKey]
        [Identity]
        public int Id2 { get; set; }

        [Column(Type = "character varying")]
        public string Name { get; set; }
    }
}