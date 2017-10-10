using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.DbFirst.Schema
{
    [DataContract]
    public class Column: IColumn
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string CatalogName { get; set; }

        [DataMember]
        public string SchemaName { get; set; }

        [DataMember]
        public string TableName { get; set; }

        [DataMember]
        public string DataType { get; set; }

        [DataMember]
        public long MaxSize { get; set; }

        [DataMember]
        public byte Precision { get; set; }

        [DataMember]
        public byte Scale { get; set; }

        [DataMember]
        public string DefaultValue { get; set; }

        [DataMember]
        public bool IsNullable { get; set; }

        [DataMember]
        public bool IsIdentity { get; set; }

        [IgnoreDataMember]
        public Database Database { get; private set; }

        [IgnoreDataMember]
        public View TableView { get; private set; }

        [IgnoreDataMember]
        public List<Constraint> OwnConstraints { get; private set; }

        [IgnoreDataMember]
        public List<Constraint> ReferredConstraints { get; private set; }

        #endregion

        #region Constructor

        public Column()
        {
            this.OwnConstraints = new List<Constraint>();
            this.ReferredConstraints = new List<Constraint>();
        }

        #endregion

        #region Public Methods

        public override string ToString() => $"Column [{this.TableName}].[{this.Name}]";

        public void ProcessResults(Database database, View table)
        {
            this.Database = database;
            this.TableView = table;
    
            this.OwnConstraints = table.Constraints
                .Where(x => x.FromTableName == this.TableName &&
                            x.FromColumnName == this.Name).ToList();

            this.ReferredConstraints = database.Tables
                .SelectMany(x => x.Constraints)
                .Where(x => x.ToTableName == this.TableName &&
                            x.ToColumnName == this.Name).ToList();
        }

        #endregion
    }
}