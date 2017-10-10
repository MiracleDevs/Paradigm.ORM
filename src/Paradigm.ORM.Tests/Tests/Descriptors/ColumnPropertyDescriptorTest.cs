using System;
using FluentAssertions;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Tests.Mocks.Tables;
using NUnit.Framework;

namespace Paradigm.ORM.Tests.Tests.Descriptors
{
    [TestFixture]
    public class ColumnPropertyDescriptorTest
    {
        #region Properties

        private TableTypeDescriptor ClientTableTypeDescription { get; }

        #endregion

        #region Constructor

        public ColumnPropertyDescriptorTest()
        {
            // We instantiate a TableTypeDescriptor object because ColumnPropertyDescriptor
            // is declared as internal, therefore we test it through TableTypeDescriptor
            ClientTableTypeDescription = new TableTypeDescriptor(typeof(Client));
        }

        #endregion

        #region Columns Tests

        [Test]
        public void ShouldMapColumnWithMaxSize()
        {
            var nameColumn = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "Name");

            nameColumn.MaxSize.Should().Be(200);
        }

        [Test]
        public void ShouldMapColumnWithPrecision()
        {
            var addressColumn = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "AddressId");

            addressColumn.Precision.Should().Be(10);
        }

        [Test]
        public void ShouldMapColumnWithScale()
        {
            var hourlyRate = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "HourlyRate");

            hourlyRate.Scale.Should().Be(9);
        }

        [Test]
        public void ShouldDifferNotNullableTypeAndNullableType()
        {
            var regdateColumn = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "RegistrationDate");
            
            regdateColumn.PropertyType.Should().Be(typeof(DateTime?));
            regdateColumn.NotNullablePropertyType.Should().Be(typeof(DateTime));
        }

        [Test]
        public void ShouldNotDifferNotNullableTypeAndNullableType()
        {
            var nameColumn = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "Name");

            nameColumn.PropertyType.Should().Be(typeof(string));
            nameColumn.NotNullablePropertyType.Should().Be(typeof(string));
        }

        [Test]
        public void ShouldGetCorrectSqlType()
        {
            var nameColumn = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "Name");
            nameColumn.DataType.Should().Be("varchar");

            var addressColumn = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "AddressId");
            addressColumn.DataType.Should().Be("int");

            var registrationDateColumn = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "RegistrationDate");
            registrationDateColumn.DataType.Should().Be("datetime");

            var hourlyRateColumn = ClientTableTypeDescription.AllProperties.Find(x => x.ColumnName == "HourlyRate");
            hourlyRateColumn.DataType.Should().Be("decimal");
        }

        #endregion
    }
}