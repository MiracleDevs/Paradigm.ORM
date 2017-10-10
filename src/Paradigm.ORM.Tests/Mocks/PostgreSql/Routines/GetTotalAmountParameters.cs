using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.PostgreSql.Routines
{
    [Routine("GetTotalAmount")]
    public class GetTotalAmountParameters
    {
        [Parameter("Active", "bool", true)]
        public bool Active { get; set; }
    }
}
