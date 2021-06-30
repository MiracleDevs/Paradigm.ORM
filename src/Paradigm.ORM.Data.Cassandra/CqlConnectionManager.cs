using System;
using System.Collections.Concurrent;
using Cassandra.Data;

namespace Paradigm.ORM.Data.Cassandra
{
    public class CqlConnectionManager
    {
        #region Singleton

        /// <summary>
        /// The internal instance
        /// </summary>
        private static readonly Lazy<CqlConnectionManager> InternalInstance = new Lazy<CqlConnectionManager>(() => new CqlConnectionManager(), true);

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>
        /// The instance.
        /// </value>
        public static CqlConnectionManager Instance => InternalInstance.Value;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the clusters.
        /// </summary>
        /// <value>
        /// The clusters.
        /// </value>
        private ConcurrentDictionary<string, CqlConnection> Connections { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CqlConnection"/> class.
        /// </summary>
        private CqlConnectionManager()
        {
            this.Connections = new ConcurrentDictionary<string, CqlConnection>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void ClearConnections()
        {
            foreach (var connection in this.Connections.Values)
            {
                connection?.Dispose();
            }

            this.Connections.Clear();
        }

        /// <summary>
        /// Gets the cluster.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        public CqlConnection GetConnection(string connectionString)
        {
            if (this.Connections.ContainsKey(connectionString))
                return this.Connections[connectionString];

            var connection = new CqlConnection(connectionString);
            this.Connections[connectionString] = connection;

            return connection;
        }

        #endregion
    }
}