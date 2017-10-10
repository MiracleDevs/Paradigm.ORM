namespace Paradigm.ORM.Tests.Mocks
{
    public interface ITwoPrimaryKeyTable
    {
        int Id { get; set; }

        int Id2 { get; set; }

        string Name { get; set; }
        
    }
}
