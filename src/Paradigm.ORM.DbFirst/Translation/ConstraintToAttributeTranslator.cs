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
            switch (input.Type)
            {
                case ConstraintType.PrimaryKey:
                    return GetPrimaryKeyAtribute();

                case ConstraintType.ForeignKey:
                    return GetForeignKeyAttribute(input);

                case ConstraintType.UniqueKey:
                    return GetUniqueKeyAttribute(input);

                default:
                    return null;
            }
        }

        #endregion

        #region Private Methods

        private static Attribute GetPrimaryKeyAtribute()
        {
            return new Attribute { Name = nameof(PrimaryKeyAttribute) };
        }

        private static Attribute GetForeignKeyAttribute(Constraint input)
        {
            return new Attribute
            {
                Name = nameof(ForeignKeyAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter { Name = nameof(ForeignKeyAttribute.Name), Value = input.Name},
                    new AttributeParameter { Name = nameof(ForeignKeyAttribute.FromTable), Value = input.FromTableName },
                    new AttributeParameter { Name = nameof(ForeignKeyAttribute.FromColumn), Value = input.FromColumnName },
                    new AttributeParameter { Name = nameof(ForeignKeyAttribute.ToTable), Value = input.ToTableName },
                    new AttributeParameter { Name = nameof(ForeignKeyAttribute.ToColumn), Value = input.ToColumnName }
                }
            };
        }

        private static Attribute GetUniqueKeyAttribute(Constraint input)
        {
            return new Attribute
            {
                Name = nameof(UniqueKeyAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter { Name = nameof(UniqueKeyAttribute.Name), Value = input.Name },
                    new AttributeParameter { Name = nameof(UniqueKeyAttribute.Column), Value = input.FromColumnName }
                }
            };
        }

        #endregion
    }
}