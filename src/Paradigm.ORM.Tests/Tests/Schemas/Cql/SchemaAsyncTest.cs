﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Paradigm.ORM.Data.Database.Schema;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.Tests.Fixtures.Cql;

namespace Paradigm.ORM.Tests.Tests.Schemas.Cql
{
    public class SchemaAsyncTest
    {
        private CqlSchemaFixture Fixture { get; }

        public SchemaAsyncTest()
        {
            this.Fixture = new CqlSchemaFixture();
        }

        [OneTimeSetUp]
        public void Setup()
        {
            this.Fixture.CreateTables();
        }

        [Test]
        public void ShouldCreateTheSchemaProvider()
        {
            ISchemaProvider schemaProvider = null;
            this.Fixture.Connector.Invoking(x => schemaProvider = x.GetSchemaProvider()).Should().NotThrow();
            schemaProvider.Should().NotBeNull();
        }

        [Test]
        public async Task ShouldRetrieveAllTables()
        {
            var schemaProvider = this.Fixture.Connector.GetSchemaProvider();
            List<ITable> tables = null;

            var getDatabaseNameFunction = async () => tables = await schemaProvider.GetTablesAsync(this.Fixture.GetDatabaseName());
            await getDatabaseNameFunction.Should().NotThrowAsync();
            tables.Should().NotBeNull();
            tables.Count.Should().Be(3);

            tables[0].Name.Should().Be("table1");
            tables[0].CatalogName.Should().Be(this.Fixture.GetDatabaseName());
            tables[0].SchemaName.Should().BeNull();

            tables[1].Name.Should().Be("table2");
            tables[1].CatalogName.Should().Be(this.Fixture.GetDatabaseName());
            tables[1].SchemaName.Should().BeNull();

            tables[2].Name.Should().Be("table3");
            tables[2].CatalogName.Should().Be(this.Fixture.GetDatabaseName());
            tables[2].SchemaName.Should().BeNull();

        }

        [Test]
        public async Task ShouldRetrieveColumns()
        {
            var schemaProvider = this.Fixture.Connector.GetSchemaProvider();
            List<IColumn> columns = null;

            var getColumnsFunction = async () => columns = await schemaProvider.GetColumnsAsync(this.Fixture.GetDatabaseName(), "table1");
            await getColumnsFunction.Should().NotThrowAsync();

            columns.Should().NotBeNull();
            columns.Count.Should().Be(19);

            columns[0].Name.Should().Be("column01");
            columns[0].DataType.Should().Be("uuid");
            columns[0].TableName.Should().Be("table1");
            columns[0].CatalogName.Should().Be("test");
            columns[0].SchemaName.Should().Be(null);
            columns[0].DefaultValue.Should().Be(null);
            columns[0].IsIdentity.Should().Be(false);
            columns[0].IsNullable.Should().Be(false);
            columns[0].MaxSize.Should().Be(0);
            columns[0].Precision.Should().Be(0);
            columns[0].Scale.Should().Be(0);

            columns[1].Name.Should().Be("column02");
            columns[1].DataType.Should().Be("ascii");

            columns[2].Name.Should().Be("column03");
            columns[2].DataType.Should().Be("bigint");

            columns[3].Name.Should().Be("column04");
            columns[3].DataType.Should().Be("blob");

            columns[4].Name.Should().Be("column05");
            columns[4].DataType.Should().Be("boolean");

            columns[5].Name.Should().Be("column06");
            columns[5].DataType.Should().Be("date");

            columns[6].Name.Should().Be("column07");
            columns[6].DataType.Should().Be("decimal");

            columns[7].Name.Should().Be("column08");
            columns[7].DataType.Should().Be("double");

            columns[8].Name.Should().Be("column09");
            columns[8].DataType.Should().Be("float");

            columns[9].Name.Should().Be("column10");
            columns[9].DataType.Should().Be("inet");

            columns[10].Name.Should().Be("column11");
            columns[10].DataType.Should().Be("int");

            columns[11].Name.Should().Be("column12");
            columns[11].DataType.Should().Be("smallint");

            columns[12].Name.Should().Be("column13");
            columns[12].DataType.Should().Be("text");

            columns[13].Name.Should().Be("column14");
            columns[13].DataType.Should().Be("time");

            columns[14].Name.Should().Be("column15");
            columns[14].DataType.Should().Be("timestamp");

            columns[15].Name.Should().Be("column16");
            columns[15].DataType.Should().Be("timeuuid");

            columns[16].Name.Should().Be("column17");
            columns[16].DataType.Should().Be("tinyint");

            columns[17].Name.Should().Be("column18");
            columns[17].DataType.Should().Be("text"); // varchar is a synomym of text.

            columns[18].Name.Should().Be("column19");
            columns[18].DataType.Should().Be("varint");

            getColumnsFunction = async () => columns = await schemaProvider.GetColumnsAsync(this.Fixture.GetDatabaseName(), "table3");
            await getColumnsFunction.Should().NotThrowAsync();

            columns.Should().NotBeNull();
            columns.Count.Should().Be(19);

            columns[0].Name.Should().Be("column01");
            columns[0].DataType.Should().Be("uuid");
            columns[0].TableName.Should().Be("table3");
            columns[0].CatalogName.Should().Be("test");
            columns[0].SchemaName.Should().Be(null);
            columns[0].DefaultValue.Should().Be(null);
            columns[0].IsIdentity.Should().Be(false);
            columns[0].IsNullable.Should().Be(false);
            columns[0].MaxSize.Should().Be(0);
            columns[0].Precision.Should().Be(0);
            columns[0].Scale.Should().Be(0);

            columns[1].Name.Should().Be("column02");
            columns[1].DataType.Should().Be("ascii");

            columns[2].Name.Should().Be("column03");
            columns[2].DataType.Should().Be("bigint");

            columns[3].Name.Should().Be("column04");
            columns[3].DataType.Should().Be("blob");

            columns[4].Name.Should().Be("column05");
            columns[4].DataType.Should().Be("boolean");

            columns[5].Name.Should().Be("column06");
            columns[5].DataType.Should().Be("date");

            columns[6].Name.Should().Be("column07");
            columns[6].DataType.Should().Be("decimal");

            columns[7].Name.Should().Be("column08");
            columns[7].DataType.Should().Be("double");

            columns[8].Name.Should().Be("column09");
            columns[8].DataType.Should().Be("float");

            columns[9].Name.Should().Be("column10");
            columns[9].DataType.Should().Be("inet");

            columns[10].Name.Should().Be("column11");
            columns[10].DataType.Should().Be("int");

            columns[11].Name.Should().Be("column12");
            columns[11].DataType.Should().Be("smallint");

            columns[12].Name.Should().Be("column13");
            columns[12].DataType.Should().Be("text");

            columns[13].Name.Should().Be("column14");
            columns[13].DataType.Should().Be("time");

            columns[14].Name.Should().Be("column15");
            columns[14].DataType.Should().Be("timestamp");

            columns[15].Name.Should().Be("column16");
            columns[15].DataType.Should().Be("timeuuid");

            columns[16].Name.Should().Be("column17");
            columns[16].DataType.Should().Be("tinyint");

            columns[17].Name.Should().Be("column18");
            columns[17].DataType.Should().Be("text"); // varchar is a synomym of text.

            columns[18].Name.Should().Be("column19");
            columns[18].DataType.Should().Be("varint");

        }

        [Test]
        public async Task ShouldRetrieveConstraints()
        {
            var schemaProvider = this.Fixture.Connector.GetSchemaProvider();
            List<IConstraint> constraints = null;

            var getConstraintsFunction = async () => constraints = await schemaProvider.GetConstraintsAsync(this.Fixture.GetDatabaseName(), "table1");
            await getConstraintsFunction.Should().NotThrowAsync();

            constraints.Should().NotBeNull();
            constraints.Count.Should().Be(1);
            constraints[0].Name.Should().Be("column01");
            constraints[0].CatalogName.Should().Be("test");
            constraints[0].SchemaName.Should().Be(null);
            constraints[0].FromTableName.Should().Be("table1");
            constraints[0].FromColumnName.Should().Be("column01");
            constraints[0].ToTableName.Should().Be(null);
            constraints[0].ToColumnName.Should().Be(null);
            constraints[0].Type.Should().Be(ConstraintType.PrimaryKey);

            getConstraintsFunction = async () => constraints = await schemaProvider.GetConstraintsAsync(this.Fixture.GetDatabaseName(), "table2");
            await getConstraintsFunction.Should().NotThrowAsync();

            constraints.Should().NotBeNull();
            constraints.Count.Should().Be(3);
            constraints[0].Name.Should().Be("column01");
            constraints[0].CatalogName.Should().Be("test");
            constraints[0].SchemaName.Should().Be(null);
            constraints[0].FromTableName.Should().Be("table2");
            constraints[0].FromColumnName.Should().Be("column01");
            constraints[0].ToTableName.Should().Be(null);
            constraints[0].ToColumnName.Should().Be(null);
            constraints[0].Type.Should().Be(ConstraintType.PrimaryKey);

            getConstraintsFunction = async () => constraints = await schemaProvider.GetConstraintsAsync(this.Fixture.GetDatabaseName(), "table3");
            await getConstraintsFunction.Should().NotThrowAsync();

            constraints.Should().NotBeNull();
            constraints.Count.Should().Be(4);

            constraints[0].Name.Should().Be("column01");
            constraints[0].CatalogName.Should().Be("test");
            constraints[0].SchemaName.Should().Be(null);
            constraints[0].FromTableName.Should().Be("table3");
            constraints[0].FromColumnName.Should().Be("column01");
            constraints[0].ToTableName.Should().Be(null);
            constraints[0].ToColumnName.Should().Be(null);
            constraints[0].Type.Should().Be(ConstraintType.PrimaryKey);

            constraints[1].Name.Should().Be("column02");
            constraints[1].CatalogName.Should().Be("test");
            constraints[1].SchemaName.Should().Be(null);
            constraints[1].FromTableName.Should().Be("table3");
            constraints[1].FromColumnName.Should().Be("column02");
            constraints[1].ToTableName.Should().Be(null);
            constraints[1].ToColumnName.Should().Be(null);
            constraints[1].Type.Should().Be(ConstraintType.PrimaryKey);
        }

        [Test]
        public async Task ShouldntRetrieveStoredProcedures()
        {
            var schemaProvider = this.Fixture.Connector.GetSchemaProvider();
            List<IStoredProcedure> storedProcedures = null;

            var getStoredProceduresFunction = async () => storedProcedures = await schemaProvider.GetStoredProceduresAsync(this.Fixture.GetDatabaseName());
            await getStoredProceduresFunction.Should().NotThrowAsync();

            storedProcedures.Should().NotBeNull();
            storedProcedures.Count.Should().Be(0);
        }

        [Test]
        public async Task ShouldntRetrieveParameters()
        {
            var schemaProvider = this.Fixture.Connector.GetSchemaProvider();
            List<IParameter> parameters = null;

            var getParametersFunction = async () => parameters = await schemaProvider.GetParametersAsync(this.Fixture.GetDatabaseName(), "StoredProcedure");
            await getParametersFunction.Should().NotThrowAsync();

            parameters.Should().NotBeNull();
            parameters.Count.Should().Be(0);
        }

        [OneTimeTearDown]
        public void Teardown()
        {
            this.Fixture.DropDatabase();
            this.Fixture.Dispose();
        }
    }
}