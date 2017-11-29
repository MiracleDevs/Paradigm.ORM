using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Paradigm.ORM.Data.Database.Schema.Structure;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides a cache for different ORM descriptors.
    /// </summary>
    public sealed class DescriptorCache
    {
        #region Singleton

        /// <summary>
        /// The internal instance.
        /// </summary>
        private static readonly Lazy<DescriptorCache> InternalInstance = new Lazy<DescriptorCache>(() => new DescriptorCache(), true);

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The singleton instance.
        /// </value>
        public static DescriptorCache Instance => InternalInstance.Value;

        #endregion

        #region Properties

        /// <summary>
        /// Gets the table descriptors.
        /// </summary>
        /// <value>
        /// The table descriptors.
        /// </value>
        private ConcurrentDictionary<ITable, ITableDescriptor> TableDescriptors { get; }

        /// <summary>
        /// Gets the table type descriptors.
        /// </summary>
        /// <value>
        /// The table type descriptors.
        /// </value>
        private ConcurrentDictionary<Type, ITableTypeDescriptor> TableTypeDescriptors { get; }

        /// <summary>
        /// Gets the custom type descriptors.
        /// </summary>
        /// <value>
        /// The custom type descriptors.
        /// </value>
        private ConcurrentDictionary<Type, ICustomTypeDescriptor> CustomTypeDescriptors { get; }

        /// <summary>
        /// Gets the routine type descriptors.
        /// </summary>
        /// <value>
        /// The routine type descriptors.
        /// </value>
        private ConcurrentDictionary<Type, IRoutineTypeDescriptor> RoutineTypeDescriptors { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Prevents a default instance of the <see cref="DescriptorCache"/> class from being created.
        /// </summary>
        private DescriptorCache()
        {
            this.TableDescriptors = new ConcurrentDictionary<ITable, ITableDescriptor>();
            this.TableTypeDescriptors = new ConcurrentDictionary<Type, ITableTypeDescriptor>();
            this.CustomTypeDescriptors = new ConcurrentDictionary<Type, ICustomTypeDescriptor>();
            this.RoutineTypeDescriptors = new ConcurrentDictionary<Type, IRoutineTypeDescriptor>();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets a table descriptor instance.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columns">The columns.</param>
        /// <param name="constraints">The constraints.</param>
        /// <returns></returns>
        public ITableDescriptor GetTableDescriptor(ITable table, List<IColumn> columns, List<IConstraint> constraints)
        {
            return this.TableDescriptors.ContainsKey(table)
                ? this.TableDescriptors[table]
                : this.TableDescriptors.GetOrAdd(table, new TableDescriptor(table, columns, constraints));
        }

        /// <summary>
        /// Gets a table type descriptor instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ITableTypeDescriptor GetTableTypeDescriptor(Type type)
        {
            return this.TableTypeDescriptors.ContainsKey(type)
                ? this.TableTypeDescriptors[type]
                : this.TableTypeDescriptors.GetOrAdd(type, new TableTypeDescriptor(type));
        }

        /// <summary>
        /// Gets a custom type descriptor instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ICustomTypeDescriptor GetCustomTypeDescriptor(Type type)
        {
            return this.CustomTypeDescriptors.ContainsKey(type)
                ? this.CustomTypeDescriptors[type]
                : this.CustomTypeDescriptors.GetOrAdd(type, new CustomTypeDescriptor(type));
        }

        /// <summary>
        /// Gets a routine type descriptor instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IRoutineTypeDescriptor GetRoutineTypeDescriptor(Type type)
        {
            return this.RoutineTypeDescriptors.ContainsKey(type)
                ? this.RoutineTypeDescriptors[type]
                : this.RoutineTypeDescriptors.GetOrAdd(type, new RoutineTypeDescriptor(type));
        }

        /// <summary>
        /// Removes the table descriptor instance.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public bool RemoveTableDescriptor(ITable table)
        {
            return this.TableDescriptors.TryRemove(table, out ITableDescriptor _);
        }

        /// <summary>
        /// Removes the table type descriptor instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool RemoveTableTypeDescriptor(Type type)
        {
            return this.TableTypeDescriptors.TryRemove(type, out ITableTypeDescriptor _);
        }

        /// <summary>
        /// Removes the custom type descriptor instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool RemoveCustomTypeDescriptor(Type type)
        {
            return this.CustomTypeDescriptors.TryRemove(type, out ICustomTypeDescriptor _);
        }

        /// <summary>
        /// Removes the routine type descriptor instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public bool RemoveRoutineTypeDescriptor(Type type)
        {
            return this.RoutineTypeDescriptors.TryRemove(type, out IRoutineTypeDescriptor _);
        }

        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void Clear()
        {
            this.TableDescriptors.Clear();
            this.TableTypeDescriptors.Clear();
            this.CustomTypeDescriptors.Clear();
            this.RoutineTypeDescriptors.Clear();
        }

        #endregion
    }
}