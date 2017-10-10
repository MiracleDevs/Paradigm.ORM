using System.Collections.Generic;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Mappers
{
    /// <summary>
    /// Provides an interface for database reaader mappers.
    /// </summary>
    /// <remarks>
    /// When executing database reading commands, the command will return a <see cref="IDatabaseReader" /> object,
    /// which will be used to extract the result sets and registers from the database.
    /// The <see cref="IDatabaseReaderMapper"/> provides a way to automatically map one of the result sets to a known
    /// object type.
    /// This mapper works by creating a <see cref="Descriptors.TableTypeDescriptor"/>, and extracting the mapping information
    /// from the class attributes. 
    /// </remarks>
    /// <seealso cref="Descriptors.TableTypeDescriptor"/>
    /// <seealso cref="Attributes.TableTypeAttribute" />
    /// <seealso cref="Attributes.TableAttribute"/>
    public partial interface IDatabaseReaderMapper
    {
        /// <summary>
        /// Gets a list of objects mapped from a database reader.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A list of mapped objects.</returns>
        List<object> Map(IDatabaseReader reader);
    }
}