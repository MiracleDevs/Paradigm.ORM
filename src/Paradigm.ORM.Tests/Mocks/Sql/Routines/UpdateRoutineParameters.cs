using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Sql.Routines
{
    [Routine("UpdateRoutine")]
    public class UpdateRoutineParameters
    {
        [Parameter("tId", "int", true)]
        public int Id { get; set; }
    }
}
