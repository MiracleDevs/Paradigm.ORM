using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql.Routines
{
    [Routine("update_routine")]
    public class UpdateRoutineParameters
    {
        [Parameter("Id", "int", true)]
        public int Id { get; set; }
    }
}
