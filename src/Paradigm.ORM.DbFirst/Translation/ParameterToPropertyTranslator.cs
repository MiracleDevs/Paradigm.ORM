using System.Collections.Generic;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.Core.Extensions;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;
using Parameter = Paradigm.ORM.DbFirst.Schema.Parameter;

namespace Paradigm.ORM.DbFirst.Translation
{
    public class ParameterToPropertyTranslator : TranslatorBase<Parameter, Property>
    {
        #region Constructor

        public ParameterToPropertyTranslator(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override Property Translate(Parameter input)
        {
            var nameTranslation = this.Configuration.GetStoredProcedureConfiguration(input)?.GetParameterRenameConfiguration(input);
            var name = nameTranslation?.NewName ?? input.Name;
            var type = this.Connector.GetDbStringTypeConverter().GetType(input);
            NativeTypeList.Instance.RegisterType(type);

            return new Property
            {
                Name = name,
                TypeName = type.GetReadableFullName(),
                Attributes = new List<Attribute>
                {
                    new()
                    {
                        Name = nameof(ParameterAttribute),
                        Parameters = new List<AttributeParameter>
                        {
                            new()
                            {
                                Name = nameof(ParameterAttribute.Name),
                                Value = input.Name
                            },
                            new()
                            {
                                Name = nameof(ParameterAttribute.Type),
                                Value = input.DataType
                            },
                            new()
                            {
                                Name = nameof(ParameterAttribute.IsInput),
                                Value = input.IsInput.ToString().ToLower(),
                                IsNumeric = true
                            }
                        }
                    }
                }
            };
        }

        #endregion
    }
}