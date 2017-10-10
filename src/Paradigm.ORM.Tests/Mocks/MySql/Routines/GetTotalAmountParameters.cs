using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql.Routines
{
    [Routine("GetTotalAmount")]
    public class GetTotalAmountParameters
    {
        [Parameter("Active", "tinyint", true)]
        public bool Active { get; set; }
    }
}
