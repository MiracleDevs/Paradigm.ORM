using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;

namespace Paradigm.ORM.DbFirst.Translation
{
    public class ObjectToArrayTranslator: TranslatorBase<ObjectContainer, ObjectContainer>
    {
        #region Constructor

        public ObjectToArrayTranslator(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override ObjectContainer Translate(ObjectContainer input)
        {
            return new ObjectContainer
            {
                Class = new Class
                {
                    Name = $"{input.Class.Name}[]",
                    FullName = $"{input.Class.FullName}[]",
                    Namespace = input.Class.Namespace,
                    IsAbstract = false,
                    IsArray = true,
                    InnerObjectName = input.Class.FullName
                }
            };
        }

        #endregion
    }
}