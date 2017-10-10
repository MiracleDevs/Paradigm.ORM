using System;
using System.Collections.Generic;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.Core.Extensions;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;
using Paradigm.ORM.DbFirst.Schema;
using Attribute = Paradigm.CodeGen.Input.Json.Models.Attribute;

namespace Paradigm.ORM.DbFirst.Translation
{
    public class ColumnToPropertyTranslator : TranslatorBase<Column, Property>
    {
        #region Constructor

        public ColumnToPropertyTranslator(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override Property Translate(Column input)
        {
            var valueProvider = this.Connector.GetDbTypeValueRangeProvider();
            var constraintToAttributeTranslator = new ConstraintToAttributeTranslator(this.Connector, this.Configuration);
            var minValue = valueProvider.GetMinValue(input.DataType);
            var maxValue = valueProvider.GetMaxValue(input.DataType);
            var attributes = new List<Attribute>();

            var nameTranslation = this.Configuration.GetTableConfiguration(input)?.GetColumnRenameConfiguration(input);
            var name = nameTranslation?.NewName ?? input.Name;

            attributes.Add(GetColumnAttribute(input));

            if (input.IsIdentity)
                attributes.Add(GetIdentityAttribute());

            if (!input.IsNullable)
                attributes.Add(GetNullableAttribute());

            if (minValue != null && maxValue != null)
                attributes.Add(GetRangeAttribute(minValue, maxValue));

            if (input.MaxSize > 0)
                attributes.Add(GetSizeAttribute(input));

            if (input.Precision > 0 || input.Scale > 0)
                attributes.Add(GetNumericAttribute(input));

            attributes = attributes.Union(input.OwnConstraints.Select(constraintToAttributeTranslator.Translate).Where(x => x != null)).ToList();

            var type = this.Connector.GetDbStringTypeConverter().GetType(input);
            NativeTypeList.Instance.RegisterType(type);

            return new Property
            {
                Name = name,
                TypeName = type.GetReadableFullName(),
                Attributes = attributes
            };
        }

        #endregion

        #region Private Methods

        private string FormatValue(object value)
        {
            if (value is DateTime dateTime)
                return dateTime.ToString("O");

            if (value is DateTimeOffset dateTimeOffset)
                return dateTimeOffset.ToString("O");

            if (value is TimeSpan timeSpan)
                return timeSpan.ToString("G");

            return value?.ToString();
        }

        private static Attribute GetColumnAttribute(Column input)
        {
            return new Attribute
            {
                Name = nameof(ColumnAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter
                    {
                        Name = nameof(ColumnAttribute.Name),
                        Value = input.Name
                    },
                    new AttributeParameter
                    {
                        Name = nameof(ColumnAttribute.Type),
                        Value = input.DataType
                    }
                }
            };
        }

        private static Attribute GetIdentityAttribute()
        {
            return new Attribute { Name = nameof(IdentityAttribute) };
        }

        private static Attribute GetNullableAttribute()
        {
            return new Attribute { Name = nameof(NotNullableAttribute) };
        }

        private Attribute GetRangeAttribute(object minValue, object maxValue)
        {
            return new Attribute
            {
                Name = nameof(RangeAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter
                    {
                        Name = nameof(RangeAttribute.MinValue),
                        Value = this.FormatValue(minValue)
                    },

                    new AttributeParameter
                    {
                        Name = nameof(RangeAttribute.MaxValue),
                        Value = this.FormatValue(maxValue)
                    }
                }
            };
        }

        private static Attribute GetSizeAttribute(Column input)
        {
            return new Attribute
            {
                Name = nameof(SizeAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter
                    {
                        Name = nameof(SizeAttribute.MaxSize),
                        Value = input.MaxSize.ToString(),
                        IsNumeric = true
                    }
                }
            };
        }

        private static Attribute GetNumericAttribute(Column input)
        {
            return new Attribute
            {
                Name = nameof(NumericAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter
                    {
                        Name = nameof(NumericAttribute.Precision),
                        Value = input.Precision.ToString(),
                        IsNumeric = true
                    },
                    new AttributeParameter
                    {
                        Name = nameof(NumericAttribute.Scale),
                        Value = input.Scale.ToString(),
                        IsNumeric = true
                    }
                }
            };
        }

        #endregion
    }
}