using System.Collections.Generic;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Mappers.Generic
{
    /// <summary>
    /// Provides an interface for database reaader mappers.
    /// </summary>
    /// <typeparam name="TEntity">A type containing mapping information.</typeparam>
    /// <remarks>
    /// When executing database reading commands, the command will return a <see cref="IDatabaseReader" /> object,
    /// which will be used to extract the result sets and registers from the database.
    /// The <see cref="IDatabaseReaderMapper"/> provides a way to automatically map one of the result sets to a known
    /// object type.
    /// This mapper works by creating a <see cref="Descriptors.TableTypeDescriptor"/> from the TEntity type, 
    /// and extracting the mapping information from the class attributes. 
    /// </remarks>
    /// <seealso cref="IDatabaseReaderMapper"/>
    /// <seealso cref="Descriptors.TableTypeDescriptor"/>
    /// <seealso cref="Attributes.TableTypeAttribute" />
    /// <seealso cref="Attributes.TableAttribute"/>
    public partial interface IDatabaseReaderMapper<TEntity> : IDatabaseReaderMapper
    {
        /// <summary>
        /// Gets a list of <see cref="TEntity"/> from a <see cref="IDatabaseReader"/>.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A list of <see cref="TEntity"/>.</returns>
        new List<TEntity> Map(IDatabaseReader reader);
    }
}