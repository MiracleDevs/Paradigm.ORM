using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql.Routines
{
    [Routine("get_8_results")]
    public class GetEightResultsParameters
    {
        [Parameter("Active", "tinyint", true)]
        public bool Active { get; set; }
    }
}