using System.Collections.Generic;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.DbFirst.Configuration;
using Paradigm.ORM.DbFirst.Schema;
using Attribute = Paradigm.CodeGen.Input.Json.Models.Attribute;

namespace Paradigm.ORM.DbFirst.Translation
{
    public class ConstraintToAttributeTranslator : TranslatorBase<Constraint, Attribute>
    {
        #region Constructor

        public ConstraintToAttributeTranslator(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override Attribute Translate(Constraint input)
        {
            return input.Type switch
            {
                ConstraintType.PrimaryKey => GetPrimaryKeyAttribute(),
                ConstraintType.ForeignKey => GetForeignKeyAttribute(input),
                ConstraintType.UniqueKey => GetUniqueKeyAttribute(input),
                _ => null
            };
        }

        #endregion

        #region Private Methods

        private static Attribute GetPrimaryKeyAttribute()
        {
            return new Attribute { Name = nameof(PrimaryKeyAttribute) };
        }

        private static Attribute GetForeignKeyAttribute(IConstraint input)
        {
            return new Attribute
            {
                Name = nameof(ForeignKeyAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new() { Name = nameof(ForeignKeyAttribute.Name), Value = input.Name},
                    new() { Name = nameof(ForeignKeyAttribute.FromTable), Value = input.FromTableName },
                    new() { Name = nameof(ForeignKeyAttribute.FromColumn), Value = input.FromColumnName },
                    new() { Name = nameof(ForeignKeyAttribute.ToTable), Value = input.ToTableName },
                    new() { Name = nameof(ForeignKeyAttribute.ToColumn), Value = input.ToColumnName }
                }
            };
        }

        private static Attribute GetUniqueKeyAttribute(IConstraint input)
        {
            return new Attribute
            {
                Name = nameof(UniqueKeyAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new() { Name = nameof(UniqueKeyAttribute.Name), Value = input.Name },
                    new() { Name = nameof(UniqueKeyAttribute.Column), Value = input.FromColumnName }
                }
            };
        }

        #endregion
    }
}