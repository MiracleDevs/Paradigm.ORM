using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.Core.Extensions;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Translation
{
    public class TypeToObjectContainerTranslator: TranslatorBase<Type, ObjectContainer>
    {
        #region Constructor

        public TypeToObjectContainerTranslator(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override ObjectContainer Translate(Type input)
        {
            var typeInfo = input.GetTypeInfo();
            var output = new ObjectContainer();

            // enum is not possible in this scenario.
            var objectBase = typeInfo.IsClass ? (output.Class = new Class()) : (output.Struct = new Struct());

            if (input.GenericTypeArguments != null && input.GenericTypeArguments.Length > 0)
                objectBase.InnerObjectName = input.GenericTypeArguments[0].GetReadableFullName();

            objectBase.Name = input.GetReadableName();
            objectBase.FullName = input.GetReadableFullName();
            objectBase.Namespace = input.Namespace;
            
            if (typeInfo.IsArray && input != typeof(string))
            {
                objectBase.IsArray = true;
                objectBase.InnerObjectName = input.GetElementType().FullName;
            }
            else if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo) && input != typeof(string))
            {
                objectBase.IsArray = true;
                objectBase.InnerObjectName = (typeInfo.IsGenericType ? input.GenericTypeArguments.FirstOrDefault() : input.GetElementType())?.FullName;
            }

            return output;
        }

        #endregion
    }
}