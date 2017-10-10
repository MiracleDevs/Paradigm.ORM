using System;
using System.Collections.Generic;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;
using Attribute = Paradigm.CodeGen.Input.Json.Models.Attribute;
using StoredProcedure = Paradigm.ORM.DbFirst.Schema.StoredProcedure;

namespace Paradigm.ORM.DbFirst.Translation
{
    public class StoredProcedureToObjectContainerTranslator : TranslatorBase<StoredProcedure, ObjectContainer>
    {
        #region Constructor

        public StoredProcedureToObjectContainerTranslator(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override ObjectContainer Translate(StoredProcedure input)
        {
            var spConfig = this.Configuration.StoredProcedures.FirstOrDefault(x => x.Name == input.Name);

            if (spConfig == null)
                throw new Exception($"The configuration for stored procedure '{input.Name}' couldn't be retrieved.");

            var parameterTranslator = new ParameterToPropertyTranslator(this.Connector, this.Configuration);
            var nameTranslation = this.Configuration.GetStoredProcedureConfiguration(input);
            var name = nameTranslation?.NewName ?? input.Name;
            var attributes = new List<Attribute>
            {
                GetRoutineAttribute(input),
                GetStoredProcedureTypeAttribute(input)
            };

            attributes.AddRange(spConfig.ResultTypes.Select(GetRoutineResultAttribute));

            return new ObjectContainer
            {
                Class = new Class
                {
                    Name = name,
                    FullName = $"{input.SchemaName}.{name}",
                    Namespace = input.SchemaName,
                    Properties = input.Parameters.Select(parameterTranslator.Translate).ToList(),
                    Attributes = attributes
                }
            };
        }

        #endregion

        #region Private Methods

        private static Attribute GetRoutineAttribute(StoredProcedure input)
        {
            return new Attribute
            {
                Name = nameof(RoutineAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter
                    {
                        Name = nameof(RoutineAttribute.Name),
                        Value = input.Name
                    }
                }
            };
        }

        private static Attribute GetStoredProcedureTypeAttribute(StoredProcedure input)
        {
            return new Attribute
            {
                Name = nameof(StoredProcedureTypeAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter
                    {
                        Name = nameof(StoredProcedureTypeAttribute.ProcedureType),
                        Value = input.Type.ToString()
                    }
                }
            };
        }

        private static Attribute GetRoutineResultAttribute(string resultType)
        {
            return new Attribute
            {
                Name = nameof(RoutineResultAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new AttributeParameter
                    {
                        Name = nameof(RoutineResultAttribute.ResultType),
                        Value = resultType
                    }
                }
            };
        }

        #endregion
    }
}