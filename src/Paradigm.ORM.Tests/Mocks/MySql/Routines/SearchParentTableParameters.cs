﻿using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.MySql.Routines
{
    [Routine("search_parent_table")]
    public class SearchParentTableParameters
    {
        [Parameter("ParentName", "varchar", true), Size(200)]
        public string ParentName { get; set; }

        [Parameter("Active", "tinyint", true)]
        public bool? Active { get; set; }
        
    }
}
