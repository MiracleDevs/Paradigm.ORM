using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
    [Table]
    public class PurchaseOrder: AuditBase
    {
        [Column]
        public string Number { get; set; }

        [Column]
        public string VendorSalesOrderNumber { get; set; }

        [Column]
        public string VendorName { get; set; }
    }
}