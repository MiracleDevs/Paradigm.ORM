using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Paradigm.CodeGen.Input.Json.Models;
using Paradigm.ORM.Data.Attributes;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Database.Schema.Structure;
using Paradigm.ORM.DbFirst.Configuration;
using Paradigm.ORM.DbFirst.Schema;
using Attribute = Paradigm.CodeGen.Input.Json.Models.Attribute;

namespace Paradigm.ORM.DbFirst.Translation
{
    public abstract class TableViewToObjectContainerTranslatorBase<TInput>: TranslatorBase<TInput, ObjectContainer>
    {
        #region Constructor

        protected TableViewToObjectContainerTranslatorBase(IDatabaseConnector connector, DbFirstConfiguration configuration) : base(connector, configuration)
        {
        }

        #endregion

        #region Public Methods

        public override ObjectContainer Translate(TInput input)
        {
            return null;
        }

        #endregion

        #region Protected Methods

        protected IEnumerable<Property> CreateReferredConstraintProperties(View view)
        {
            var viewConfiguration = view is Table table
                ? this.Configuration.GetTableConfiguration(table)
                : this.Configuration.GetTableConfiguration(view);

            var properties = new List<Property>();
            var navigationProperties = view.Database
                    .Tables
                    .SelectMany(x => x.Constraints)
                    .Union(view.Database.Views.SelectMany(x => x.Constraints))
                    .Where(x => x.ToTableName == view.Name && x.Type == ConstraintType.ForeignKey)
                    .GroupBy(x => new { x.Name, x.FromTableName, x.SchemaName });

            foreach (var navigationProperty in navigationProperties)
            {
                if (view.Database.Tables.All(x => x.Name != navigationProperty.Key.FromTableName) &&
                    view.Database.Views.All(x => x.Name != navigationProperty.Key.FromTableName))
                    continue;

                var fromTableName = navigationProperty.Key.FromTableName;
                var toTableConfiguration = this.Configuration.GetTableConfiguration(fromTableName);
                var objectName = toTableConfiguration?.NewName ?? fromTableName;
                var pluralName = objectName.Pluralize();
                var propertyName = viewConfiguration?.ColumnsToRename?.FirstOrDefault(x => x.Name == pluralName)?.NewName ?? pluralName;

                properties.Add(new Property
                {
                    Name = propertyName,
                    TypeName = $"{navigationProperty.Key.SchemaName}.{objectName}[]",
                    Attributes = this.GetAttributes(navigationProperty, true)
                });
            }

            return properties;
        }

        protected IEnumerable<Property> CreateOwnConstraintNavigationProperties(View view)
        {
            var viewConfiguration = view is Table table
                ? this.Configuration.GetTableConfiguration(table)
                : this.Configuration.GetTableConfiguration(view);

            var properties = new List<Property>();
            var navigationProperties = view.Constraints.Where(x => x.Type == ConstraintType.ForeignKey).GroupBy(x => new { x.Name, x.ToTableName, x.SchemaName });

            foreach (var navigationProperty in navigationProperties)
            {
                if (view.Database.Tables.All(x => x.Name != navigationProperty.Key.ToTableName) &&
                    view.Database.Views.All(x => x.Name != navigationProperty.Key.ToTableName))
                    continue;

                var toTableName = navigationProperty.Key.ToTableName;
                var toTableConfiguration = this.Configuration.GetTableConfiguration(toTableName);
                var objectName = toTableConfiguration?.NewName ?? toTableName;
                var propertyName = viewConfiguration?.ColumnsToRename?.FirstOrDefault(x => x.Name == objectName)?.NewName ?? objectName;

                properties.Add(new Property
                {
                    Name = propertyName,
                    TypeName = $"{navigationProperty.Key.SchemaName}.{objectName}",
                    Attributes = this.GetAttributes(navigationProperty, false)
                });
            }

            return properties;
        }

        protected List<Attribute> GetAttributes(IEnumerable<Constraint> constraints, bool invert)
        {
            return constraints.Select(x => new Attribute
            {
                Name = nameof(NavigationAttribute),
                Parameters = new List<AttributeParameter>
                {
                    new()
                    {
                        Name = nameof(NavigationAttribute.ReferencedType),
                        Value = this.GetTableName(invert ? x.FromTableView : x.ToTableView)
                    },
                    new()
                    {
                        Name = nameof(NavigationAttribute.SourceProperty),
                        Value = this.GetColumnName(invert ? x.ToColumn : x.FromColumn)
                    },
                    new()
                    {
                        Name = nameof(NavigationAttribute.ReferencedProperty),
                        Value = this.GetColumnName(invert ? x.FromColumn : x.ToColumn)
                    }
                }

            }).ToList();
        }

        protected string GetTableName(View view)
        {
            var viewConfiguration = view is Table table
                ? this.Configuration.GetTableConfiguration(table)
                : this.Configuration.GetTableConfiguration(view);

            return viewConfiguration?.NewName ?? view.Name;
        }

        protected string GetColumnName(Column column)
        {
            var viewConfiguration = column.TableView is Table table
                ? this.Configuration.GetTableConfiguration(table)
                : this.Configuration.GetTableConfiguration(column.TableView);

            var nameTranslation = viewConfiguration.GetColumnRenameConfiguration(column);
            var name = nameTranslation?.NewName ?? column.Name;
            return name;
        }

        #endregion
    }
}