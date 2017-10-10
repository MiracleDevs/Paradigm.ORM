using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Database;

namespace Paradigm.ORM.Data.Mappers
{
    public partial class DatabaseReaderMapper
    {
        #region Public Methods

        /// <summary>
        /// Gets a list of objects mapped from a database reader.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A list of mapped objects.</returns>
        public async Task<List<object>> MapAsync(IDatabaseReader reader)
        {
            var list = new List<object>();

            while (await reader.ReadAsync())
            {
                list.Add(await this.MapRowAsync(reader));
            }

            return list;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// Maps one single row from the <see cref="IDatabaseReader"/>.
        /// </summary>
        /// <param name="reader">A database reader cursor.</param>
        /// <returns>A new instance of the known object type already mapped.</returns>
        /// <remarks>This method do not advance the reading cursor.</remarks>
        protected virtual Task<object> MapRowAsync(IDatabaseReader reader)
        {
            // TODO: maybe we could add an option to allow map by index instead of name. Should be even faster.            
            var entity = Activator.CreateInstance(this.Descriptor.Type);

            foreach (var property in this.Descriptor.AllProperties)
            {
                var value = reader.GetValue(property.ColumnName);
                property.PropertyInfo.SetValue(entity, NativeTypeConverter.ConvertTo(value, property.NotNullablePropertyType));
            }

            return Task.FromResult(entity);
        }

        #endregion
    }
}