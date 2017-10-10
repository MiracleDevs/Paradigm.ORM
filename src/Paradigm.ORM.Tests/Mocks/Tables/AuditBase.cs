using System;
using Paradigm.ORM.Data.Attributes;

namespace Paradigm.ORM.Tests.Mocks.Tables
{
    public abstract class AuditBase
    {
        [Column]
        public int CreationUserId { get; set; }
        
        [Column]
        public DateTime CreationDate { get; set; }

        [Column]
        public int ModificationUserId { get; set; }

        [Column]
        public DateTime ModificationDate { get; set; }
    }
}