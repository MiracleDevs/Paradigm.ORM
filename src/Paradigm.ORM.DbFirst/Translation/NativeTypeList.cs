using System;
using System.Collections.Generic;
using System.Linq;

namespace Paradigm.ORM.DbFirst.Translation
{
    public sealed class NativeTypeList
    {
        #region Properties

        private static readonly Lazy<NativeTypeList> InternalInstance = new(() => new NativeTypeList(), true);

        public static NativeTypeList Instance => InternalInstance.Value;

        private List<Type> InnerList { get; }

        public IReadOnlyCollection<Type> Types => this.InnerList;

        #endregion

        #region Constructor

        private NativeTypeList()
        {
            this.InnerList = new List<Type>();
        }

        #endregion

        #region Public Methods

        public void RegisterType(Type type)
        {
            if (this.InnerList.Any(x => x == type))
                return;

            foreach (var argument in type.GenericTypeArguments)
            {
                this.RegisterType(argument);
            }

            this.InnerList.Add(type);
        }

        public void Clear()
        {
            this.InnerList.Clear();
        }

        #endregion
    }
}