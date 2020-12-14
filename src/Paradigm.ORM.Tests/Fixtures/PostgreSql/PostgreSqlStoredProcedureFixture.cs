using System;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Tests.Mocks.PostgreSql;
using System.Collections.Generic;
using Paradigm.ORM.Data.PostgreSql;

namespace Paradigm.ORM.Tests.Fixtures.PostgreSql
{
    public class PostgreSqlStoredProcedureFixture : StoredProcedureFixtureBase
    {
        private string ConnectionString => ConnectionStrings.PSql;

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

        public override object CreateActiveChildEntity()
        {
            return new SingleKeyChildTable
            {
                Name = "Test Child " + Guid.NewGuid(),
                IsActive = true,
                Amount = new decimal(30.34),
                CreatedDate = DateTime.Today,
            };
        }

        public override object CreateNewActiveEntity()
        {
            return new SingleKeyParentTable
            {
                Name = $"Test Parent 1 {Guid.NewGuid()}",
                IsActive = true,
                Amount = (decimal)30.34,
                CreatedDate = new DateTime(2017, 4, 12),
                Childs = new List<SingleKeyChildTable>
                {
                    CreateActiveChildEntity() as SingleKeyChildTable,
                    CreateActiveChildEntity() as SingleKeyChildTable,
                }
            };
        }

        public override object CreateNewInactiveEntity()
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
                        IsActive = false,
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

        public override void CreateStoredProcedures()
        {
            this.Connector.ExecuteNonQuery(@"
                 CREATE OR REPLACE FUNCTION ""SearchParentTable""(""ParentName"" VARCHAR(200), ""Active"" BOOLEAN)
                 RETURNS TABLE (Id int, Name varchar(200), IsActive bool, Amount decimal(20,9), CreatedDate date) AS $$
                 BEGIN
                     RETURN QUERY
                     SELECT *
                     FROM ""SingleKeyParentTable"" as skpt
                     WHERE skpt.""Name"" ilike '%' || ""ParentName"" || '%' AND skpt.""IsActive"" = ""Active"";
                 END;
                 $$ LANGUAGE plpgsql;");

            this.Connector.ExecuteNonQuery(@"
                CREATE OR REPLACE FUNCTION ""UpdateRoutine""(""tId"" INT)
                RETURNS void AS $$
                BEGIN
	                UPDATE ""SingleKeyParentTable"" as skpt
                    SET ""Name"" = 'Test Parent ChangedNameTest'
                    WHERE skpt.""Id"" = ""tId"";
                END;
                $$ LANGUAGE plpgsql;");

            this.Connector.ExecuteNonQuery(@"
                CREATE OR REPLACE FUNCTION ""SearchParentsAndChilds""(""ParentName"" TEXT, ""Active"" BOOLEAN)
                RETURNS SETOF refcursor AS
                $$
                DECLARE
	                ref1 refcursor;
	                ref2 refcursor;
                BEGIN
	                OPEN ref1 FOR
		                SELECT * FROM ""SingleKeyParentTable"" as skpt
                        WHERE skpt.""Name"" like ('%' || ""ParentName"" || '%')
                              AND skpt.""IsActive"" = ""Active"";
                    RETURN NEXT ref1;
                    OPEN ref2 FOR
                            SELECT* FROM ""SingleKeyChildTable"" as skct
                            WHERE skct.""Name"" like ('%' || ""ParentName"" || '%')
                                  AND skct.""IsActive"" = ""Active"";
                    RETURN NEXT ref2;
                END;
                    $$ LANGUAGE plpgsql;");

            this.Connector.ExecuteNonQuery(@"
               CREATE OR REPLACE FUNCTION ""GetTotalAmount""(""Active"" bool)
               RETURNS decimal(20, 9) AS $$
               DECLARE
                   amount decimal(20, 9);
               BEGIN
               SELECT SUM(skpt.""Amount"") INTO amount
                   FROM ""SingleKeyParentTable"" as skpt
                   WHERE skpt.""IsActive"" = ""Active"";
               RETURN amount;
               END;
               $$ LANGUAGE plpgsql;");
        }

        public override void DeleteStoredProcedures()
        {
            this.Connector.ExecuteNonQuery("DROP FUNCTION IF EXISTS \"SearchParentTable\"(varchar, boolean);");
            this.Connector.ExecuteNonQuery("DROP FUNCTION IF EXISTS \"GetTotalAmount\"(boolean);");
            this.Connector.ExecuteNonQuery("DROP FUNCTION IF EXISTS \"SearchParentsAndChilds\"(varchar, boolean);");
            this.Connector.ExecuteNonQuery("DROP FUNCTION IF EXISTS \"UpdateRoutine\"(int);");
        }
    }
}
