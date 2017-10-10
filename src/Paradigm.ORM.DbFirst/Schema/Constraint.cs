using System.Linq;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.DbFirst.Schema
{
    [DataContract]
    public class Constraint: IConstraint
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public ConstraintType Type { get; set; }

        [DataMember]
        public string CatalogName { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public string FromTableName { get; set; }

        [DataMember]
        public string FromColumnName { get; set; }

        [DataMember]
        public string ToTableName { get; set; }

        [DataMember]
        public string ToColumnName { get; set; }

        [IgnoreDataMember]
        public View FromTableView { get; private set; }

        [IgnoreDataMember]
        public Column FromColumn { get; private set; }

        [IgnoreDataMember]
        public View ToTableView { get; private set; }

        [IgnoreDataMember]
        public Column ToColumn { get; private set; }

        [IgnoreDataMember]
        public Database Database { get; private set; }

        #endregion

        #region Public Methods

        public override string ToString() => $"Constraint [{this.FromTableName}].[{this.Name}]";

        public void ProcessResults(Database database, View table)
        {
            this.Database = database;
            this.FromTableView = table;

            if (this.FromTableName != null && this.FromColumnName != null)
                this.FromColumn = this.FromTableView.Columns.FirstOrDefault(x => x.Name == this.FromColumnName);

            if (this.ToTableName != null)
                this.ToTableView = database.Tables.Union(database.Views).FirstOrDefault(x => x.Name == this.ToTableName);

            if (this.ToTableView != null && this.ToColumnName != null)
                this.ToColumn = this.ToTableView.Columns.FirstOrDefault(x => x.Name == this.ToColumnName);
        }

        #endregion
    }
}