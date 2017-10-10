using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Sql.Routines
{
    [Routine("SearchParentsAndChilds")]
    public class SearchParentsAndChildsParameters
    {
        [Parameter("ParentName", "nvarchar", true), Size(200)]
        public string ParentName { get; set; }

        [Parameter("Active", "bit", true)]
        public bool Active { get; set; }
    }
}
