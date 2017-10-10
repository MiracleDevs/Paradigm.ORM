using System.Linq;

namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides the information to describe a navigation key.
    /// </summary>
    /// <seealso cref="Paradigm.ORM.Data.Descriptors.INavigationKeyDescriptor" />
    internal class NavigationKeyDescriptor : INavigationKeyDescriptor
    {
        #region Properties

        /// <summary>
        /// Gets the table type descriptor for the source type.
        /// </summary>
        public ITableTypeDescriptor FromTypeDescriptor { get; }

        /// <summary>
        /// Gets the table type descriptor for the referenced type.
        /// </summary>
        public ITableTypeDescriptor ToTypeDescriptor { get; }

        /// <summary>
        /// Gets the column property descriptor for the source property.
        /// </summary>
        public IColumnPropertyDescriptor FromPropertyDescriptor { get; }

        /// <summary>
        /// Gets the column property descriptor for the referrenced property.
        /// </summary>
        public IColumnPropertyDescriptor ToPropertyDescriptor { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationKeyDescriptor"/> class.
        /// </summary>
        /// <param name="fromTypeDescriptor">From type descriptor.</param>
        /// <param name="toTypeDescriptor">To type descriptor.</param>
        /// <param name="fromProperty">From property.</param>
        /// <param name="toProperty">To property.</param>
        internal NavigationKeyDescriptor(ITableTypeDescriptor fromTypeDescriptor, ITableTypeDescriptor toTypeDescriptor, string fromProperty, string toProperty)
        {
            this.FromTypeDescriptor = fromTypeDescriptor;
            this.ToTypeDescriptor = toTypeDescriptor;

            this.FromPropertyDescriptor = this.FromTypeDescriptor.AllProperties.First(x => x.PropertyName == fromProperty);
            this.ToPropertyDescriptor = this.ToTypeDescriptor.AllProperties.First(x => x.PropertyName == toProperty);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString() => $"Navigation Key Descriptor [{this.FromPropertyDescriptor.ColumnName} => {this.ToPropertyDescriptor.ColumnName}]";

        #endregion
    }
}