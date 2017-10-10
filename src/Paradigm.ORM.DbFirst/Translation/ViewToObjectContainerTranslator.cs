using System.Collections.Generic;
using System.Linq;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.DbFirst.Configuration;
using Paradigm.ORM.DbFirst.Schema;

namespace Paradigm.ORM.DbFirst.Translation
{
    public class ViewToObjectContainerTranslator : TableViewToObjectContainerTranslatorBase<View>
    {
        #region Constructor

        public ViewToObjectContainerTranslator(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override ObjectContainer Translate(View input)
        {
            var columnTranslator = new ColumnToPropertyTranslator(this.Connector, this.Configuration);
            var name = this.GetTableName(input);
            var configuration = this.Configuration.GetTableConfiguration(input);

            var objectContainer = new ObjectContainer
            {
                Class = new Class
                {
                    Name = name,
                    FullName = $"{input.SchemaName}.{name}",
                    Namespace = input.SchemaName,
                    Properties = input.Columns.Select(columnTranslator.Translate)
                        .Union(CreateOwnConstraintNavigationProperties(input))
                        .Union(CreateReferredConstraintProperties(input))
                        .ToList(),

                    Attributes = new List<Attribute>
                    {
                        new Attribute
                        {
                            Name = nameof(TableAttribute),
                            Parameters = new List<AttributeParameter>
                            {
                                new AttributeParameter
                                {
                                    Name = nameof(TableAttribute.Catalog),
                                    Value = input.CatalogName
                                },
                                new AttributeParameter
                                {
                                    Name = nameof(TableAttribute.Schema),
                                    Value =input.SchemaName
                                },
                                new AttributeParameter
                                {
                                    Name = nameof(TableAttribute.Name),
                                    Value = input.Name
                                }
                            }
                        }
                    }
                }
            };

            objectContainer.Class.Properties.RemoveAll(x => configuration.ColumnsToRemove.Contains(x.Name));
            return objectContainer;
        }

        #endregion
    }
}