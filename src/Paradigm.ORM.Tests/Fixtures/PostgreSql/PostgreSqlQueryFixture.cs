using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.PostgreSql;
using Paradigm.ORM.Data.PostgreSql;
using System.Collections.Generic;

namespace Paradigm.ORM.Tests.Fixtures.PostgreSql
{
    public class PostgreSqlQueryFixture: QueryFixtureBase
    {
        private string ConnectionString => "Server=192.168.2.90;User Id=test;Password=test1234;Timeout=3;Database=test";

        public override string SelectClause => "SELECT * FROM \"SingleKeyParentTable\"";

        protected override IDatabaseConnector CreateConnector()
        {
            return new PostgreSqlDatabaseConnector(this.ConnectionString);
        }

        public override void CreateDatabase()
        {
        }

        public override void DropDatabase()
        {
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS \"SingleKeyChildTable\";");
            this.Connector.ExecuteNonQuery("DROP TABLE IF EXISTS \"SingleKeyParentTable\";");
        }

        public override void CreateParentTable()
        {
            this.Connector.ExecuteNonQuery(
                "CREATE TABLE IF NOT EXISTS \"SingleKeyParentTable\"(" +
                    "\"Id\"            SERIAL," +
                    "\"Name\"          VARCHAR(200)     NOT NULL," +
                    "\"IsActive\"      BOOLEAN          NOT NULL," +
                    "\"Amount\"        DECIMAL(20,9)    NOT NULL," +
                    "\"CreatedDate\"   DATE             NOT NULL," +

                    "CONSTRAINT \"PK_SingleKeyParentTable\" PRIMARY KEY (\"Id\")," +
                    "CONSTRAINT \"UX_SingleKeyParentTable_Name\" UNIQUE (\"Name\")" +
                ");");
        }

        public override void CreateChildTable()
        {
            this.Connector.ExecuteNonQuery(
                "CREATE TABLE IF NOT EXISTS \"SingleKeyChildTable\"(" +
                    "\"Id\"            SERIAL," +
                    "\"ParentId\"      INT             NOT NULL," +
                    "\"Name\"          VARCHAR(200)    NOT NULL," +
                    "\"IsActive\"      BOOLEAN         NOT NULL," +
                    "\"Amount\"        DECIMAL(20,9)   NOT NULL," +
                    "\"CreatedDate\"   DATE            NOT NULL," +

                    "CONSTRAINT \"PK_SingleKeyChildTable\" PRIMARY KEY (\"Id\")," +
                    "CONSTRAINT \"UX_SingleKeyChildTable_Name\" UNIQUE (\"Name\")," +
                    "CONSTRAINT \"FK_SingleKeyChildTable_Parent\" FOREIGN KEY (\"ParentId\") REFERENCES \"SingleKeyParentTable\" (\"Id\")" +
                ");");
        }

        public override object CreateNewEntity()
        {
            return new SingleKeyParentTable
            {
                Name = $"Test Parent 1 {Guid.NewGuid()}",
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = new DateTime(2017, 4, 12),
                Childs = new List<SingleKeyChildTable>
                {
                    new SingleKeyChildTable
                    {
                        Name = "Test Child " + Guid.NewGuid(),
                        IsActive = true,
                        Amount = new decimal(30.34),
                        CreatedDate = DateTime.Today,
                    }
                }
            };
        }

        public override object CreateNewEntity2()
        {
            return new SingleKeyParentTable
            {
                Name = $"Test Parent 2 {Guid.NewGuid()}",
                IsActive = false,
                Amount = 215.50m,
                CreatedDate = new DateTime(2017, 6, 21),
                Childs = new List<SingleKeyChildTable>
                {
                    new SingleKeyChildTable
                    {
                        Name = $"Test Child 1 {Guid.NewGuid()}",
                        IsActive = false,
                        Amount = 100.25m,
                        CreatedDate = new DateTime(2017, 6, 22),
                    },
                    new SingleKeyChildTable
                    {
                        Name = $"Test Child 2 {Guid.NewGuid()}",
                        IsActive = true,
                        Amount = 115.25m,
                        CreatedDate = new DateTime(2017, 6, 23),
                    }
                }
            };
        }

        public override ITableTypeDescriptor GetDescriptor()
        {
            return DescriptorCache.Instance.GetTableTypeDescriptor(typeof(AllColumnsClass));
        }
    }
}
