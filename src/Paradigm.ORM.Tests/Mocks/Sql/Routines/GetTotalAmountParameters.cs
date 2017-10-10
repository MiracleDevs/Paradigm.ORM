using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Sql.Routines
{
    [Routine("GetTotalAmount")]
    public class GetTotalAmountParameters
    {
        [Parameter("Active", "bit", true)]
        public bool Active { get; set; }
    }
}
