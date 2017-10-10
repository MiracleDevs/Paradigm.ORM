using System;
using System.Collections.Generic;
using FluentAssertions;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Tests.Mocks.Tables;
using NUnit.Framework;

namespace Paradigm.ORM.Tests.Tests.Descriptors
{
    [TestFixture]
    public class TableTypeDescriptorTest
    {
        #region Properties

        private TableTypeDescriptor ClientTableTypeDescription { get; }

        #endregion

        #region Constructor

        public TableTypeDescriptorTest()
        {
            // We set up this as a class property because we use it in almost every test (excepting one)
            ClientTableTypeDescription = new TableTypeDescriptor(typeof(Client));
        }

        #endregion

        #region General Tests

        [Test]
        public void NoTableClassShouldThrowMissingTableException()
        {
            Action newDescriptor = () => new TableTypeDescriptor(typeof(NoTableClass));
            newDescriptor.ShouldThrow<OrmMissingTableMappingException>();
        }

        #endregion

        #region Root Object Tests

        [Test]
        public void PrimaryKeysShouldHavePrimaryKeyAttribute()
        {
            ClientTableTypeDescription.PrimaryKeyProperties.Count.Should().Be(1);
            ClientTableTypeDescription.PrimaryKeyProperties[0].PropertyName.Should().Be(nameof(Client.Id));
        }

        [Test]
        public void DescriptorTypesShouldBeOk()
        {
            ClientTableTypeDescription.Type.Should().Be(typeof(Client));
        }

        [Test]
        public void PropertiesCountShouldBeOk()
        {
            ClientTableTypeDescription.AllProperties.Count.Should().Be(14);
            ClientTableTypeDescription.SimpleProperties.Count.Should().Be(13);
        }

        [Test]
        public void DescriptorNamesShouldBeOk()
        {
            ClientTableTypeDescription.TableName.Should().Be("client");
            ClientTableTypeDescription.TypeName.Should().Be(nameof(Client));
        }

        [Test]
        public void NewEntityShouldBeNew()
        {
            var newClient = new Client();

            ClientTableTypeDescription.IsNew(newClient).Should().BeTrue();
        }

        [Test]
        public void EntityShouldNotBeNew()
        {
            var client = new Client
            {
                Id = 35
            };

            ClientTableTypeDescription.IsNew(client).Should().BeFalse();
        }

        #endregion

        #region Navigation Properties Tests

        [Test]
        public void NavigationPropertiesCountShouldBeOk()
        {
            ClientTableTypeDescription.NavigationProperties.Count.Should().Be(2);
        }

        [Test]
        public void PrimaryKeysShouldHavePrimaryKeyAttributeOnOneToOne()
        {
            var addressNavigation = ClientTableTypeDescription.NavigationProperties.Find(x => x.PropertyName == "Address");

            addressNavigation.ToDescriptor.PrimaryKeyProperties.Count.Should().Be(1);
            addressNavigation.ToDescriptor.PrimaryKeyProperties[0].PropertyName.Should().Be(nameof(Client.Id)); 
        }

        [Test]
        public void DescriptorTypesShouldBeOkOneToOne()
        {
            var addressNavigation = ClientTableTypeDescription.NavigationProperties.Find(x => x.PropertyName == "Address");

            addressNavigation.PropertyType.Should().Be(typeof(IAddress));
            addressNavigation.FromDescriptor.Type.Should().Be(typeof(Client));
            addressNavigation.ToDescriptor.Type.Should().Be(typeof(Address));
        }

        [Test]
        public void PropertiesCountShouldBeOkOneToOne()
        {
            var addressNavigation = ClientTableTypeDescription.NavigationProperties.Find(x => x.PropertyName == "Address");

            addressNavigation.ToDescriptor.AllProperties.Count.Should().Be(13);
            addressNavigation.ToDescriptor.SimpleProperties.Count.Should().Be(12);
            addressNavigation.ToDescriptor.NavigationProperties.Count.Should().Be(0);
        }

        [Test]
        public void PrimaryKeysShouldHavePrimaryKeyAttributeOnManyToOne()
        {
            var projectNavigation = ClientTableTypeDescription.NavigationProperties.Find(x => x.PropertyName == "Projects");

            projectNavigation.ToDescriptor.PrimaryKeyProperties.Count.Should().Be(1);
            projectNavigation.ToDescriptor.PrimaryKeyProperties[0].PropertyName.Should().Be(nameof(Client.Id));
        }

        [Test]
        public void DescriptorTypesShouldBeOkManyToOne()
        {
            var projectNavigation = ClientTableTypeDescription.NavigationProperties.Find(x => x.PropertyName == "Projects");

            projectNavigation.PropertyType.Should().Be(typeof(IReadOnlyCollection<IProject>));
            projectNavigation.FromDescriptor.Type.Should().Be(typeof(Client));
            projectNavigation.ToDescriptor.Type.Should().Be(typeof(Project));
        }

        [Test]
        public void PropertiesCountShouldBeOkManyToOne()
        {
            var projectNavigation = ClientTableTypeDescription.NavigationProperties.Find(x => x.PropertyName == "Projects");

            projectNavigation.ToDescriptor.AllProperties.Count.Should().Be(10);
            projectNavigation.ToDescriptor.SimpleProperties.Count.Should().Be(9);
            projectNavigation.ToDescriptor.NavigationProperties.Count.Should().Be(0);
        }

        [Test]
        public void ShouldInheritPropertyMappings()
        {
            var salesOrderDescriptor = new TableTypeDescriptor(typeof(SalesOrder));
            var purchaseOrderDescriptor = new TableTypeDescriptor(typeof(PurchaseOrder));

            salesOrderDescriptor.AllProperties.Should().HaveCount(7);
            purchaseOrderDescriptor.AllProperties.Should().HaveCount(7);

            salesOrderDescriptor.AllProperties[0].PropertyName.Should().Be(nameof(SalesOrder.Number));
            salesOrderDescriptor.AllProperties[1].PropertyName.Should().Be(nameof(SalesOrder.CustomerPurchaseOrderNumber));
            salesOrderDescriptor.AllProperties[2].PropertyName.Should().Be(nameof(SalesOrder.CustomerName));
            salesOrderDescriptor.AllProperties[3].PropertyName.Should().Be(nameof(AuditBase.CreationUserId));
            salesOrderDescriptor.AllProperties[4].PropertyName.Should().Be(nameof(AuditBase.CreationDate));
            salesOrderDescriptor.AllProperties[5].PropertyName.Should().Be(nameof(AuditBase.ModificationUserId));
            salesOrderDescriptor.AllProperties[6].PropertyName.Should().Be(nameof(AuditBase.ModificationDate));

            purchaseOrderDescriptor.AllProperties[0].PropertyName.Should().Be(nameof(PurchaseOrder.Number));
            purchaseOrderDescriptor.AllProperties[1].PropertyName.Should().Be(nameof(PurchaseOrder.VendorSalesOrderNumber));
            purchaseOrderDescriptor.AllProperties[2].PropertyName.Should().Be(nameof(PurchaseOrder.VendorName));
            purchaseOrderDescriptor.AllProperties[3].PropertyName.Should().Be(nameof(AuditBase.CreationUserId));
            purchaseOrderDescriptor.AllProperties[4].PropertyName.Should().Be(nameof(AuditBase.CreationDate));
            purchaseOrderDescriptor.AllProperties[5].PropertyName.Should().Be(nameof(AuditBase.ModificationUserId));
            purchaseOrderDescriptor.AllProperties[6].PropertyName.Should().Be(nameof(AuditBase.ModificationDate));
        }

        #endregion
    }
}