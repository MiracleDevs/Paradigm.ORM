using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.PostgreSql.Routines
{
    [Routine("SearchParentsAndChilds")]
    public class SearchParentsAndChildsParameters
    {
        [Parameter("ParentName", "text", true)]
        public string ParentName { get; set; }

        [Parameter("Active", "bool", true)]
        public bool Active { get; set; }
    }
}
