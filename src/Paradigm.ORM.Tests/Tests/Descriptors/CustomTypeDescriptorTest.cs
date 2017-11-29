using FluentAssertions;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Tests.Mocks.Tables;
using NUnit.Framework;

namespace Paradigm.ORM.Tests.Tests.Descriptors
{
    [TestFixture]
    public class CustomTypeDescriptorTest
    {
        #region Properties

        private ICustomTypeDescriptor CustomTypeDescription { get; }

        #endregion

        #region Constructor

        public CustomTypeDescriptorTest()
        {
            // We set up this as a class property because we use it in almost every test (excepting one)
            CustomTypeDescription = DescriptorCache.Instance.GetCustomTypeDescriptor(typeof(ICustomMappingTable));
        }

        #endregion

        #region General Tests

        [Test]
        public void TypeNameShouldBeOk()
        {
            CustomTypeDescription.TypeName.Should().Be(nameof(ICustomMappingTable));
        }

        [Test]
        public void TypeShouldBeOk()
        {
            CustomTypeDescription.Type.Should().Be(typeof(ICustomMappingTable));
        }

        [Test]
        public void MappingsCountShouldBeOk()
        {
            CustomTypeDescription.AllProperties.Count.Should().Be(4);
        }

        #endregion
    }
}