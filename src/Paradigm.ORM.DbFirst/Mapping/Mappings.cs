using Paradigm.Core.Mapping;
using Paradigm.ORM.DbFirst.Schema;
using Paradigm.ORM.Data.Database.Schema.Structure;
using StoredProcedure = Paradigm.ORM.DbFirst.Schema.StoredProcedure;

namespace Paradigm.ORM.DbFirst.Mapping
{
    public class Mappings
    {
        public static void Initialize()
        {
            Mapper.Initialize(MapperLibrary.AutoMapper);

            Mapper.Container
                .Register<IColumn, Column>()
                .Ignore(x => x.Database)
                .Ignore(x => x.TableView)
                .Ignore(x => x.OwnConstraints)
                .Ignore(x => x.ReferredConstraints);

            Mapper.Container.Register<IConstraint, Constraint>()
                .Ignore(x => x.Database)
                .Ignore(x => x.FromTableView)
                .Ignore(x => x.FromColumn)
                .Ignore(x => x.ToTableView)
                .Ignore(x => x.ToColumn);

            Mapper.Container.Register<IParameter, Parameter>()
                .Ignore(x => x.Database)
                .Ignore(x => x.StoredProcedure);

            Mapper.Container.Register<IView, View>()
                .Ignore(x => x.Database)
                .Ignore(x => x.Columns)
                .Ignore(x => x.Constraints);

            Mapper.Container.Register<ITable, Table>()
                .Ignore(x => x.Database)
                .Ignore(x => x.Columns)
                .Ignore(x => x.Constraints)
                .Ignore(x => x.PrimaryKeys);

            Mapper.Container.Register<IStoredProcedure, StoredProcedure>()
                .Ignore(x => x.Database)
                .Ignore(x => x.Parameters)
                .Ignore(x => x.Type);

            Mapper.Compile();
        }
    }
}