using System.Collections.Generic;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;
using Paradigm.ORM.DbFirst.Schema;

namespace Paradigm.ORM.DbFirst.Translation
{
    public class DatabaseToObjectContainerTranslator: TranslatorBase<Database, List<ObjectContainer>>
    {
        #region Constructor

        public DatabaseToObjectContainerTranslator(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override List<ObjectContainer> Translate(Database input)
        {
            var tableTranslator = new TableToObjectContainerTranslator(this.Connector, this.Configuration);
            var viewTranslator = new ViewToObjectContainerTranslator(this.Connector, this.Configuration);
            var storedProcedureTranslator = new StoredProcedureToObjectContainerTranslator(this.Connector, this.Configuration);
            var typeTranslator = new TypeToObjectContainerTranslator(this.Connector, this.Configuration);
            var arrayTranslator = new ObjectToArrayTranslator(this.Connector, this.Configuration);
            
            // on each run, we clear the native type list.
            // this list contains all the db to .net types required
            // to create the models (int, float, decimal, DateTime, etc).
            NativeTypeList.Instance.Clear();

            var tableObjects = input.Tables.Select(tableTranslator.Translate).ToList();
            var viewObjects = input.Views.Select(viewTranslator.Translate).ToList();
            var storedProcedureObjects = input.StoredProcedures.Select(storedProcedureTranslator.Translate).ToList();
            var nativeObjects = NativeTypeList.Instance.Types.Select(typeTranslator.Translate).ToList();
            var arrayObjects = tableObjects.Union(viewObjects).Select(arrayTranslator.Translate).ToList();

            return tableObjects
                .Union(viewObjects)
                .Union(storedProcedureObjects)
                .Union(nativeObjects)
                .Union(arrayObjects)
                .ToList();
        }

        #endregion
    }
}