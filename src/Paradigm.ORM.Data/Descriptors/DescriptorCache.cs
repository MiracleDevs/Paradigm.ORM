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

        private static readonly Lazy<DescriptorCache> InternalInstance = new Lazy<DescriptorCache>(() => new DescriptorCache(), true);

        public static DescriptorCache Instance => InternalInstance.Value;

        #endregion

        #region Properties

        private ConcurrentDictionary<ITable, ITableDescriptor> TableDescriptors { get; }

        private ConcurrentDictionary<Type, ITableTypeDescriptor> TableTypeDescriptors { get; }

        private ConcurrentDictionary<Type, ICustomTypeDescriptor> CustomTypeDescriptors { get; }

        private ConcurrentDictionary<Type, IRoutineTypeDescriptor> RoutineTypeDescriptors { get; }

        #endregion

        #region Constructor

        private DescriptorCache()
        {
            this.TableDescriptors = new ConcurrentDictionary<ITable, ITableDescriptor>();
            this.TableTypeDescriptors = new ConcurrentDictionary<Type, ITableTypeDescriptor>();
            this.CustomTypeDescriptors = new ConcurrentDictionary<Type, ICustomTypeDescriptor>();
            this.RoutineTypeDescriptors = new ConcurrentDictionary<Type, IRoutineTypeDescriptor>();
        }

        #endregion

        #region Public Methods

        public ITableDescriptor GetTableDescriptor(ITable table, List<IColumn> columns, List<IConstraint> constraints)
        {
            return this.TableDescriptors.ContainsKey(table)
                ? this.TableDescriptors[table]
                : this.TableDescriptors.GetOrAdd(table, new TableDescriptor(table, columns, constraints));
        }

        public ITableTypeDescriptor GetTableTypeDescriptor(Type type)
        {
            return this.TableTypeDescriptors.ContainsKey(type)
                ? this.TableTypeDescriptors[type]
                : this.TableTypeDescriptors.GetOrAdd(type, new TableTypeDescriptor(type));
        }

        public ICustomTypeDescriptor GetCustomTypeDescriptor(Type type)
        {
            return this.CustomTypeDescriptors.ContainsKey(type)
                ? this.CustomTypeDescriptors[type]
                : this.CustomTypeDescriptors.GetOrAdd(type, new CustomTypeDescriptor(type));
        }

        public IRoutineTypeDescriptor GetRoutineTypeDescriptor(Type type)
        {
            return this.RoutineTypeDescriptors.ContainsKey(type)
                ? this.RoutineTypeDescriptors[type]
                : this.RoutineTypeDescriptors.GetOrAdd(type, new RoutineTypeDescriptor(type));
        }

        public bool RemoveTableDescriptor(ITable table)
        {
            return this.TableDescriptors.TryRemove(table, out ITableDescriptor _);
        }

        public bool RemoveTableTypeDescriptor(Type type)
        {
            return this.TableTypeDescriptors.TryRemove(type, out ITableTypeDescriptor _);
        }

        public bool RemoveCustomTypeDescriptor(Type type)
        {
            return this.CustomTypeDescriptors.TryRemove(type, out ICustomTypeDescriptor _);
        }

        public bool RemoveRoutineTypeDescriptor(Type type)
        {
            return this.RoutineTypeDescriptors.TryRemove(type, out IRoutineTypeDescriptor _);
        }

        #endregion
    }
}