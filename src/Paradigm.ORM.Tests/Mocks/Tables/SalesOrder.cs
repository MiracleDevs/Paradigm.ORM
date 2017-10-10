using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
    [Table]
    public class SalesOrder: AuditBase
    {
        [Column]
        public string Number { get; set; }

        [Column]
        public string CustomerPurchaseOrderNumber { get; set; }

        [Column]
        public string CustomerName { get; set; }
    }
}