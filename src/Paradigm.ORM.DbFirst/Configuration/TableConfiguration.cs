using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Paradigm.ORM.DbFirst.Schema;

namespace Paradigm.ORM.DbFirst.Configuration
{
    [DataContract]
    public class TableConfiguration
    {
        #region Properties

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string NewName { get; set; }

        [DataMember]
        public List<RenameConfiguration> ColumnsToRename { get; set; }

        [DataMember]
        public List<RenameConfiguration> ConstraintsToRename { get; set; }

        [DataMember]
        public List<ColumnConfiguration> ColumnsToAdd { get; set; }

        [DataMember]
        public List<ConstraintConfiguration> ConstraintsToAdd { get; set; }

        [DataMember]
        public List<string> ColumnsToRemove { get; set; }

        [DataMember]
        public List<string> ConstraintsToRemove { get; set; }

        #endregion

        #region Constructor

        public TableConfiguration()
        {
            this.ColumnsToRename = new List<RenameConfiguration>();
            this.ColumnsToRemove = new List<string>();
            this.ColumnsToAdd = new List<ColumnConfiguration>();

            this.ConstraintsToRename = new List<RenameConfiguration>();
            this.ConstraintsToRemove = new List<string>();
            this.ConstraintsToAdd = new List<ConstraintConfiguration>();
        }

        #endregion

        #region Public Methods

        public RenameConfiguration GetColumnRenameConfiguration(Column column)
        {
            return this.ColumnsToRename.FirstOrDefault(x => x.Name == column.Name);
        }

        public RenameConfiguration GetConstraintRenameConfiguration(Constraint constraint)
        {
            return this.ConstraintsToRename.FirstOrDefault(x => x.Name == constraint.Name);
        }

        #endregion
    }
}