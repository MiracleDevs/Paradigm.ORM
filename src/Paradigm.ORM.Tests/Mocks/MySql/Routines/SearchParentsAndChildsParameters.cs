using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql.Routines
{
    [Routine("SearchParentsAndChilds")]
    public class SearchParentsAndChildsParameters
    {
        [Parameter("ParentName", "varchar", true), Size(200)]
        public string ParentName { get; set; }

        [Parameter("Active", "tinyint", true)]
        public bool Active { get; set; }
    }
}
