namespace Paradigm.ORM.Tests.Fixtures
{
    public class ConnectionStrings
    {
        public const string Cql = "Contact Points=127.0.0.1;Port=9042;Default Keyspace=test;Username=root";

        public const string MySql = "Server=127.0.0.1;Database=test;User=root;Password=Paradigm_Test_1234;Connection Timeout=3600;Allow User Variables=True;POOLING=true";

        public const string PSql = "Server=127.0.0.1;User Id=postgres;Password=Paradigm_Test_1234;Timeout=3;Database=test";

        public const string MsSql = "Server=127.0.0.1;User=sa;Password=Paradigm_Test_1234;Connection Timeout=3600";
    }
}