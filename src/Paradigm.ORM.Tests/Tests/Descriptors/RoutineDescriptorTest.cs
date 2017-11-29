using System;
using System.Linq;
using FluentAssertions;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Tests.Mocks.Routines;
using NUnit.Framework;

namespace Paradigm.ORM.Tests.Tests.Descriptors
{
    [TestFixture]
    public class RoutineDescriptorTest
    {
        #region Properties

        private IRoutineTypeDescriptor RoutineTypeDescription { get; }

        #endregion

        #region Constructor

        public RoutineDescriptorTest()
        {
            // We set up this as a class property because we use it in almost every test (excepting one)
            RoutineTypeDescription = DescriptorCache.Instance.GetRoutineTypeDescriptor(typeof(SearchTaskParameters));
        }

        #endregion

        #region General Tests

        [Test]
        public void NoRoutineClassShouldThrowMissingRoutineException()
        {
            Action newRoutine = () => DescriptorCache.Instance.GetRoutineTypeDescriptor(typeof(NoRoutineClass));
            newRoutine.ShouldThrow<OrmMissingRoutineMappingException>();
        }

        #endregion

        #region Root Object Tests

        [Test]
        public void ShouldReturnCorrectTypes()
        {
            RoutineTypeDescription.Type.Should().Be(typeof(SearchTaskParameters));
        }

        [Test]
        public void ShouldReturnCorrectNames()
        {
            RoutineTypeDescription.RoutineName.Should().Be("SearchTask");
            RoutineTypeDescription.TypeName.Should().Be(nameof(SearchTaskParameters));
        }

        #endregion

        #region Parameters Tests

        [Test]
        public void NavigationPropertiesCountShouldBeOk()
        {
            RoutineTypeDescription.Parameters.Count.Should().Be(11);
        }

        [Test]
        public void ShouldDifferNotNullableTypeAndNullableType()
        {
            var regdateParam = RoutineTypeDescription.Parameters.Find(x => x.ParameterName == nameof(SearchTaskParameters.ClientId));

            regdateParam.Type.Should().Be(typeof(int?));
            regdateParam.NotNullableType.Should().Be(typeof(int));
        }

        [Test]
        public void ShouldNotDifferNotNullableTypeAndNullableType()
        {
            var nameParam = RoutineTypeDescription.Parameters.Find(x => x.ParameterName == nameof(SearchTaskParameters.Name));

            nameParam.Type.Should().Be(typeof(string));
            nameParam.NotNullableType.Should().Be(typeof(string));
        }

        [Test]
        public void ShouldGetScaleOk()
        {
            var numericParam = RoutineTypeDescription.Parameters.Find(x => x.ParameterName == nameof(SearchTaskParameters.NumericParam));

            numericParam.Scale.Should().Be(20);
        }

        [Test]
        public void ShouldGetPrecisionOk()
        {
            var numericParam = RoutineTypeDescription.Parameters.Find(x => x.ParameterName == nameof(SearchTaskParameters.NumericParam));

            numericParam.Precision.Should().Be(10);
        }

        [Test]
        public void ShouldGetMaxSizeOk()
        {
            var stringParam = RoutineTypeDescription.Parameters.Find(x => x.ParameterName == nameof(SearchTaskParameters.StringParam));

            stringParam.MaxSize.Should().Be(200);
        }

        [Test]
        public void ParametersOrderShouldBeTheSameAsDeclarationOrder()
        {
            RoutineTypeDescription.Parameters[0].ParameterName.Should().Be(nameof(SearchTaskParameters.Name));
            RoutineTypeDescription.Parameters[1].ParameterName.Should().Be(nameof(SearchTaskParameters.ClientId));
            RoutineTypeDescription.Parameters[2].ParameterName.Should().Be(nameof(SearchTaskParameters.ProjectId));
            RoutineTypeDescription.Parameters[3].ParameterName.Should().Be(nameof(SearchTaskParameters.TypeId));
            RoutineTypeDescription.Parameters[4].ParameterName.Should().Be(nameof(SearchTaskParameters.ComplexityId));
            RoutineTypeDescription.Parameters[5].ParameterName.Should().Be(nameof(SearchTaskParameters.IsBillable));
            RoutineTypeDescription.Parameters[6].ParameterName.Should().Be(nameof(SearchTaskParameters.Active));
            RoutineTypeDescription.Parameters[7].ParameterName.Should().Be(nameof(SearchTaskParameters.MaxTasks));
            RoutineTypeDescription.Parameters[8].ParameterName.Should().Be(nameof(SearchTaskParameters.NumericParam));
            RoutineTypeDescription.Parameters[9].ParameterName.Should().Be(nameof(SearchTaskParameters.StringParam));
           RoutineTypeDescription.Parameters[10].ParameterName.Should().Be(nameof(SearchTaskParameters.NoInputParam));
        }

        [Test]
        public void ShouldGetCorrectSqlType()
        {
            var nameParam = RoutineTypeDescription.Parameters.Find(x => x.PropertyName == nameof(SearchTaskParameters.Name));
            nameParam.DataType.Should().Be("text");

            var clientIdParam = RoutineTypeDescription.Parameters.Find(x => x.PropertyName == nameof(SearchTaskParameters.ClientId));
            clientIdParam.DataType.Should().Be("int");

            var activeParam = RoutineTypeDescription.Parameters.Find(x => x.PropertyName == nameof(SearchTaskParameters.Active));
            activeParam.DataType.Should().Be("tinyint");
        }

        [Test]
        public void InputParameterShouldHasIsInputTrue()
        {
            var nameParam = RoutineTypeDescription.Parameters.Find(x => x.PropertyName == nameof(SearchTaskParameters.Name));
            nameParam.IsInput.Should().BeTrue();
        }

        [Test]
        public void NoInputParameterShouldHasIsInputFalse()
        {
            var noInputParam = RoutineTypeDescription.Parameters.Find(x => x.PropertyName == nameof(SearchTaskParameters.NoInputParam));
            noInputParam.IsInput.Should().BeFalse();
        }

        [Test]
        public void EveryParameterShouldHaveParameterAttribute()
        {
            foreach (var parameter in RoutineTypeDescription.Parameters)
            {
                parameter.PropertyInfo
                         .CustomAttributes
                         .Where(x => x.AttributeType == typeof(ParameterAttribute))
                         .Should()
                         .HaveCount(1);
            }
        }
        
        #endregion
    }
}