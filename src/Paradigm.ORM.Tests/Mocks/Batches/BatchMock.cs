
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Batches
{
    [Table("batch")]
    public class BatchMock
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("name")]
        public string Name { get; set; }

        [Column("mobile")]
        public string Mobile { get; set; }

        [Column("mobile_brand")]
        public string MobileBrand { get; set; }

        [Column("mobile_number")]
        public string MobileNumber {get; set;}
    }
}