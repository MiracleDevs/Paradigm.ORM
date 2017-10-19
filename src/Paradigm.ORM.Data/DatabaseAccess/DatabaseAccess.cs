using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.ORM.Data.Batching;
using Paradigm.ORM.Data.CommandBuilders;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Descriptors;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.Data.Mappers;
using Paradigm.ORM.Data.Mappers.Generic;
using Paradigm.ORM.Data.StoredProcedures;
using Paradigm.ORM.Data.ValueProviders;

namespace Paradigm.ORM.Data.DatabaseAccess
{
    /// <summary>
    /// Provides the means to access a database table or view.
    /// This class contains all the standard CRUD methods to work with
    /// a table or view.
    /// </summary>
    /// <seealso cref="IDatabaseAccess"/>
    public partial class DatabaseAccess : IDatabaseAccess
    {
        #region Properties

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        protected IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Gets the database connector.
        /// </summary>
        protected IDatabaseConnector Connector { get; private set; }

        /// <summary>
        /// Gets the batch manager.
        /// </summary>
        protected IBatchManager BatchManager { get; private set; }

        /// <summary>
        /// Gets the table type descriptor.
        /// </summary>
        protected ITableTypeDescriptor Descriptor { get; private set; }

        /// <summary>
        /// Gets the command builder manager.
        /// </summary>
        protected ICommandBuilderManager CommandBuilderManager { get; private set; }

        /// <summary>
        /// Gets or sets the mapper.
        /// </summary>
        protected IDatabaseReaderMapper Mapper { get; set; }

        /// <summary>
        /// Gets the list of navigation database access.
        /// </summary>
        protected List<INavigationDatabaseAccess> NavigationDatabaseAccesses { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess"/> class.
        /// </summary>
        /// <param name="connector">A reference to the scoped database connector.</param>
        /// <param name="type">A type containing mapping information, or referencing typing information.</param>
        public DatabaseAccess(IDatabaseConnector connector, Type type) : this(null, connector, new TableTypeDescriptor(type))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess"/> class.
        /// </summary>
        /// <param name="connector">A reference to the scoped database connector.</param>
        /// <param name="descriptor">The table type descriptor.</param>
        public DatabaseAccess(IDatabaseConnector connector, ITableTypeDescriptor descriptor) : this(null, connector, descriptor)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess"/> class.
        /// </summary>
        /// <param name="serviceProvider">A reference to the scoped service provider.</param>
        /// <param name="type">A type containing mapping information, or referencing typing information.</param>
        public DatabaseAccess(IServiceProvider serviceProvider, Type type) : this(serviceProvider, serviceProvider.GetService<IDatabaseConnector>(), new TableTypeDescriptor(type))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess"/> class.
        /// </summary>
        /// <param name="serviceProvider">A reference to the scoped service provider.</param>
        /// <param name="connector">A reference to the scoped database connector.</param>
        /// <param name="type">A type containing mapping information, or referencing typing information.</param>
        public DatabaseAccess(IServiceProvider serviceProvider, IDatabaseConnector connector, Type type) : this(serviceProvider, connector, new TableTypeDescriptor(type))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseAccess"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="connector">The database connector.</param>
        /// <param name="descriptor">The table type descriptor.</param>
        public DatabaseAccess(IServiceProvider serviceProvider, IDatabaseConnector connector, ITableTypeDescriptor descriptor)
        {
            this.ServiceProvider = serviceProvider;

            // Sets the database connector.
            this.Connector = connector;

            // Sets the command batch manager.
            this.BatchManager = new BatchManager(connector);

            // Sets the Table Type Descriptor with reflected info about the table behind the entity.
            this.Descriptor = descriptor;

            // List of related data access entities.
            this.NavigationDatabaseAccesses = new List<INavigationDatabaseAccess>();

            this.Initialize();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            this.BatchManager?.Dispose();
            this.NavigationDatabaseAccesses?.ForEach(x => x?.Dispose());
            this.CommandBuilderManager?.Dispose();

            this.ServiceProvider = null;
            this.Connector = null;
            this.BatchManager = null;
            this.Mapper = null;
            this.CommandBuilderManager = null;
            this.Descriptor = null;
            this.NavigationDatabaseAccesses?.Clear();
        }

        /// <summary>
        /// Selects one element from the database.
        /// </summary>
        /// <param name="ids">Array of id values.</param>
        /// <returns>
        /// Element with the same id values as the values provider; otherwise null.
        /// </returns>
        public virtual object SelectOne(params object[] ids)
        {
            // 1. get current entity.
            var entities = this.Connector.ExecuteReader(this.CommandBuilderManager.SelectOneCommandBuilder.GetCommand(ids), reader => this.Mapper.Map(reader));

            // 2. get related entities.
            foreach (var x in this.NavigationDatabaseAccesses)
                x.Select(entities);

            return entities.FirstOrDefault();

        }

        /// <summary>
        /// Selects a list of all the elements in a table or view.
        /// </summary>
        /// <returns>
        /// A list of objects that belong to the table or view.
        /// </returns>
        /// <remarks>
        /// To select filtering results, use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.IDatabaseAccess.Select(System.String)" />.
        /// </remarks>
        public virtual List<object> Select()
        {
            return this.Select(string.Empty);
        }

        /// <summary>
        /// Selects a list of elements in a table or view.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>
        /// A list of objects that belong to the table or view.
        /// </returns>
        public virtual List<object> Select(string whereClause, params object[] parameters)
        {
            // 1. get the current entities.
            var entities = this.Connector.ExecuteReader(this.CommandBuilderManager.SelectCommandBuilder.GetCommand(whereClause, parameters), reader => this.Mapper.Map(reader));

            // 2. get related entities.
            foreach (var x in this.NavigationDatabaseAccesses)
                x.Select(entities);

            return entities;
        }

        /// <summary>
        /// Inserts an object into the table or view.
        /// </summary>
        /// <param name="entity">Object to insert.</param>
        /// <exception cref="System.ArgumentNullException">entity can not be null.</exception>
        /// <remarks>
        /// If there are more than one element to insert, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.IDatabaseAccess.Insert(System.Collections.Generic.IEnumerable{System.Object})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Insert(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} can not be null.");

            this.Insert(new List<object> { entity });
        }

        /// <summary>
        /// Inserts a list of objects into the table or view.
        /// </summary>
        /// <param name="entities">List of entities to insert.</param>
        /// <exception cref="System.ArgumentNullException">entities can not be null.</exception>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Insert(IEnumerable<object> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} can not be null.");

            var entityList = entities as List<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            // 1. Save 1-1 relationships first, as we'll need the ids of the related entities
            //    for the main entities to be stored later.
            foreach (var navigationDatabaseAccess in this.NavigationDatabaseAccesses)
            {
                if (!navigationDatabaseAccess.NavigationPropertyDescriptor.IsAggregateRoot)
                {
                    navigationDatabaseAccess.SaveBefore(entityList);
                }
            }

            // 2. Save the main entities, and batch the queries.
            this.BatchManager.Reset();

            var valueProvider = new ClassValueProvider(entityList);

            while(valueProvider.MoveNext())
            {
                this.BatchManager.Add(new CommandBatchStep(this.CommandBuilderManager.InsertCommandBuilder.GetCommand(valueProvider)));

                // if the entity has an auto incremental property,
                // queue a command to retrieve the id from the last insertion.
                if (this.Descriptor.IdentityProperty != null)
                {
                    var entity = valueProvider.CurrentEntity;
                    this.BatchManager.Add(new CommandBatchStep(this.CommandBuilderManager.LastInsertIdCommandBuilder.GetCommand(), reader => this.SetEntityId(entity, reader)));
                }
            }

            this.BatchManager.Execute();

            // 3. Save the 1-Many relationship at last, as they'll need the
            //    main entity id before being stored.
            foreach (var navigationDatabaseAccess in this.NavigationDatabaseAccesses)
            {
                if (navigationDatabaseAccess.NavigationPropertyDescriptor.IsAggregateRoot)
                {
                    navigationDatabaseAccess.SaveAfter(entityList);
                }
            }
        }

        /// <summary>
        /// Updates an object already stored in the table or view.
        /// </summary>
        /// <param name="entity">Object to insert.</param>
        /// <exception cref="System.ArgumentNullException">entity can not be null.</exception>
        /// <remarks>
        /// If there are more than one element to update, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.IDatabaseAccess.Update(System.Collections.Generic.IEnumerable{System.Object})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Update(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} can not be null.");

            this.Update(new List<object> { entity });
        }

        /// <summary>
        /// Updates a list of objects stored in the table or view.
        /// </summary>
        /// <param name="entities">List of entities to update.</param>
        /// <exception cref="System.ArgumentNullException">entities can not be null.</exception>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Update(IEnumerable<object> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities), $"{nameof(entities)} can not be null.");

            var entityList = entities as List<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            // 1. Save 1-1 relationships first, as we'll need the ids of the related entities
            //    for the main entities to be stored later.
            foreach (var navigationDatabaseAccess in this.NavigationDatabaseAccesses)
            {
                if (!navigationDatabaseAccess.NavigationPropertyDescriptor.IsAggregateRoot)
                {
                    navigationDatabaseAccess.SaveBefore(entityList);
                }
            }

            // 2. Save the main entities, and batch the queries.
            this.BatchManager.Reset();

            var valueProvider = new ClassValueProvider(entityList);

            while (valueProvider.MoveNext())
            {
                this.BatchManager.Add(new CommandBatchStep(this.CommandBuilderManager.UpdateCommandBuilder.GetCommand(valueProvider)));
            }

            this.BatchManager.Execute();

            // 3. Save the 1-Many relationship at last, as they'll need the
            //    main entity id before being stored.
            foreach (var navigationDatabaseAccess in this.NavigationDatabaseAccesses)
            {
                if (navigationDatabaseAccess.NavigationPropertyDescriptor.IsAggregateRoot)
                {
                    navigationDatabaseAccess.SaveAfter(entityList);
                }
            }
        }

        /// <summary>
        /// Deletes an object from the table or view.
        /// </summary>
        /// <param name="entity">Object to delete.</param>
        /// <exception cref="System.ArgumentNullException">entity can not be null.</exception>
        /// <remarks>
        /// If there are more than one element to delete, please use the overloaded method <see cref="M:Paradigm.ORM.Data.DatabaseAccess.IDatabaseAccess.Delete(System.Collections.Generic.IEnumerable{System.Object})" />
        /// because is prepared to batch the operation, and preventing unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Delete(object entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), $"{nameof(entity)} can not be null.");

            this.Delete(new List<object> { entity });
        }

        /// <summary>
        /// Deletes a list of objects from the table or view.
        /// </summary>
        /// <param name="entities">List of entities to delete.</param>
        /// <exception cref="System.ArgumentException">entities can not be null.</exception>
        /// <remarks>
        /// This method utilizes batching to prevent unnecessary roundtrips to the database.
        /// </remarks>
        public virtual void Delete(IEnumerable<object> entities)
        {
            if (entities == null)
                throw new ArgumentException(nameof(entities), $"{nameof(entities)} can not be null.");

            var entityList = entities as List<object> ?? entities.ToList();

            if (!entityList.Any())
                return;

            foreach (var x in this.NavigationDatabaseAccesses)
                x.DeleteBefore(entityList);

            var valueProvider = new ClassValueProvider(entityList);
            this.Connector.ExecuteNonQuery(this.CommandBuilderManager.DeleteCommandBuilder.GetCommand(valueProvider));

            foreach (var x in this.NavigationDatabaseAccesses)
                x.DeleteAfter(entityList);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Executes logic previous to initialize the instance.
        /// </summary>
        protected virtual void BeforeInitialize()
        {
        }

        /// <summary>
        /// Executes logic after the instance has been initialized.
        /// </summary>
        protected virtual void AfterInitialize()
        {
        }

        #region Protected Methods

        /// <summary>
        /// Gets the stored procedure from the service provider.
        /// </summary>
        /// <typeparam name="TProcedure">The type of the procedure.</typeparam>
        /// <remarks>The stored procedure must be registered or this method will fail.</remarks>
        /// <returns>A instance of the stored procedure.</returns>
        protected TProcedure GetStoredProcedure<TProcedure>() where TProcedure : class, IRoutine
        {
            // this method is not being used here, but will be used
            // by inherited classes to instance stored procedures if needed.
            return this.ServiceProvider?.GetService<TProcedure>();
        }

        #endregion

        #endregion

        #region Private Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.BeforeInitialize();
            this.InitializeComponents();
            this.InitializeNavigationDatabaseAccesses();
            this.AfterInitialize();
        }

        /// <summary>
        /// Initializes the components.
        /// </summary>
        private void InitializeComponents()
        {
            // Table to entity mapper which knows how to map from a DataReader to an entity.
            this.Mapper = this.ServiceProvider.GetServiceIfAvailable(typeof(IDatabaseReaderMapper<>).MakeGenericType(this.Descriptor.Type), () => new DatabaseReaderMapper(this.Connector, this.Descriptor)) as IDatabaseReaderMapper;

            // Sets the command builder manager.
            this.CommandBuilderManager = new CommandBuilderManager(this.ServiceProvider, this.Connector, this.Descriptor);
        }

        /// <summary>
        /// Initializes the navigation database accesses.
        /// </summary>
        private void InitializeNavigationDatabaseAccesses()
        {
            foreach (var navigationProperty in this.Descriptor.NavigationProperties)
            {
                var navigationDatabaseAccess = new NavigationDatabaseAccess(this.ServiceProvider, this.Connector, navigationProperty);
                navigationDatabaseAccess.Initialize();
                this.NavigationDatabaseAccesses.Add(navigationDatabaseAccess);
            }
        }

        /// <summary>
        /// Sets the entity identifier.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <param name="reader">The reader.</param>
        /// <exception cref="System.Exception">Couldn't retrieve entity auto increment property.</exception>
        private void SetEntityId(object entity, IDatabaseReader reader)
        {
            if (!reader.Read())
                throw new Exception("Couldn't retrieve entity auto increment property.");

            this.Descriptor.IdentityProperty.PropertyInfo.SetValue(entity, Convert.ToInt32(reader.GetValue(0)));
        }

        #endregion
    }
}