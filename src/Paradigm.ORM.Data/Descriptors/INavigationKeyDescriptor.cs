namespace Paradigm.ORM.Data.Descriptors
{
    /// <summary>
    /// Provides an interface to define a navigation key descriptor.
    /// </summary>
    public interface INavigationKeyDescriptor
    {
        /// <summary>
        /// Gets the table type descriptor for the source type.
        /// </summary>
        ITableTypeDescriptor FromTypeDescriptor { get; }

        /// <summary>
        /// Gets the column property descriptor for the source property.
        /// </summary>
        IColumnPropertyDescriptor FromPropertyDescriptor { get; }

        /// <summary>
        /// Gets the table type descriptor for the referenced type.
        /// </summary>
        ITableTypeDescriptor ToTypeDescriptor { get; }

        /// <summary>
        /// Gets the column property descriptor for the referrenced property.
        /// </summary>
        IColumnPropertyDescriptor ToPropertyDescriptor { get; }
    }
}