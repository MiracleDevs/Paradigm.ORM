using System;

namespace Paradigm.ORM.Tests.Mocks
{
    public interface ISimpleTable
    {
        int Id { get; set; }
        
        string Name { get; set; }

        bool IsActive { get; set; }

        decimal Amount { get; set; }
    }
}
