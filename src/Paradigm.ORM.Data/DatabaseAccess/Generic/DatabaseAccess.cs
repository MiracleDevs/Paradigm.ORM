using System;
using System.Collections.Generic;
using System.Linq;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.DatabaseAccess.Generic
{
    /// <summary>
    /// Provides the means to access a database table or view.
    /// This class contains all the standard CRUD methods to work with
    /// a table or view.
    /// </summary>
    /// <typeparam name="TEntity">A type containing or referencing the mapping information.</typeparam>
    /// <seealso cref="DatabaseAccess"/>
    /// <seealso cref="IDatabaseAccess{TEntity}"/>
    public partial class DatabaseAccess<TEntity> : DatabaseAccess, IDatabaseAccess<TEntity>
        where TEntity: class, new()
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess{TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public DatabaseAccess(IServiceProvider serviceProvider) : base(serviceProvider, typeof(TEntity))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess{TEntity}"/> class.
        /// </summary>
        /// <param name="connector">The database connector.</param>
        public DatabaseAccess(IDatabaseConnector connector) : base(null, connector, typeof(TEntity))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess{TEntity}"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        public DatabaseAccess(IServiceProvider serviceProvider, IDatabaseConnector connector) : base(serviceProvider, connector, typeof(TEntity))
        {
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Selects one element from the database.
        /// </summary>
        /// <param name="ids">Array of id values.</param>
        /// <returns>
        ///   <see cref="TEntity" /> with the same id values as the values provider; otherwise null.
        /// </returns>
        public new TEntity SelectOne(params object[] ids)
        {
            return base.SelectOne(ids) as TEntity;
        }

        /// <summary>
        /// Selects a list of all the elements in a table or view.
        /// </summary>
        /// <returns>
        /// A list of <see cref="TEntity" />.
        /// </returns>
        /// <remarks>
        /// To select filtering results, use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1.Select(System.String)" />.
        /// </remarks>
        public new List<TEntity> Select()
        {
            return base.Select().Cast<TEntity>().ToList();
        }

        /// <summary>
        /// Selects a list of elements in a table or view.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>
        /// A list of <see cref="TEntity" />.
        /// </returns>
        public new List<TEntity> Select(string whereClause, params object[] parameters)
        {
            return base.Select(whereClause, parameters).Cast<TEntity>().ToList();
        }

        /// <summary>
        /// Inserts a <see cref="TEntity" /> into the table or view.
        /// </summary>
        /// <param name="entity"><see cref="TEntity" /> to insert.</param>
        /// <remarks>
        /// If there are more than one element to insert, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1.Insert(System.Collections.Generic.IEnumerable{`0})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Insert(TEntity entity)
        {
            base.Insert(entity);
        }

        /// <summary>
        /// Inserts a list of <see cref="TEntity" /> into the table or view.
        /// </summary>
        /// <param name="entities">List of <see cref="TEntity" />.</param>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Insert(IEnumerable<TEntity> entities)
        {
            base.Insert(entities.Cast<object>().ToList());
        }

        /// <summary>
        /// Updates a <see cref="TEntity" /> already stored in the table or view.
        /// </summary>
        /// <param name="entity"><see cref="TEntity" /> to insert.</param>
        /// <remarks>
        /// If there are more than one element to update, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1.Update(System.Collections.Generic.IEnumerable{`0})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Update(TEntity entity)
        {
            base.Update(entity);
        }

        /// <summary>
        /// Updates a list of <see cref="TEntity" /> stored in the table or view.
        /// </summary>
        /// <param name="entities">List of <see cref="TEntity" /> to update.</param>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Update(IEnumerable<TEntity> entities)
        {
            base.Update(entities.Cast<object>().ToList());
        }

        /// <summary>
        /// Deletes a <see cref="TEntity" /> from the table or view.
        /// </summary>
        /// <param name="entity"><see cref="TEntity" /> to delete.</param>
        /// <remarks>
        /// If there are more than one element to delete, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.Generic.IDatabaseAccess`1.Delete(System.Collections.Generic.IEnumerable{`0})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Delete(TEntity entity)
        {
            base.Delete(entity);
        }

        /// <summary>
        /// Deletes a list of <see cref="TEntity" /> from the table or view.
        /// </summary>
        /// <param name="entities">List of <see cref="TEntity" /> to delete.</param>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Delete(IEnumerable<TEntity> entities)
        {
            base.Delete(entities.Cast<object>().ToList());
        }

        #endregion
    }
}