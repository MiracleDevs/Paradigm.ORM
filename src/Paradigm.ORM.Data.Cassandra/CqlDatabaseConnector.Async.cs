using System.Threading.Tasks;
using Paradigm.ORM.Data.Exceptions;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.Cassandra
{
    public partial class CqlDatabaseConnector
    {
        #region Public Methods

        /// <summary>
        /// Opens the connection to a database.
        /// </summary>
        /// <returns></returns>
        public async Task OpenAsync()
        {
            this.ThrowIfNull();
            await this.ThrowIfFailsAsync<OrmCanNotOpenConnectionException>(async () => await this.Connection.OpenAsync(), OrmCanNotOpenConnectionException.DefaultMessage);
        }

        /// <summary>
        /// Closes a previously opened connection to a database.
        /// </summary>
        /// <returns></returns>
        public Task CloseAsync()
        {
            this.ThrowIfNull();
            this.ThrowIfFails<OrmCanNotCloseConnectionException>(() => this.Connection?.Close(), OrmCanNotCloseConnectionException.DefaultMessage);
            return Task.FromResult(default(object));
        }

        #endregion
    }
}