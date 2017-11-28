using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Paradigm.ORM.Data.Converters;
using Paradigm.ORM.Data.Extensions;

namespace Paradigm.ORM.Data.Querying
{
    public partial class CustomQuery<TResultType>
    {
        #region Public Methods

        /// <summary>
        /// Executes the specified query and returns a list of <see cref="TResultType" />.
        /// </summary>
        /// <param name="whereClause">A where filter clause. Do not add the "WHERE" keyword to it. If you need to pass parameters, pass using @1, @2, @3.</param>
        /// <param name="parameters">A list of parameter values.</param>
        /// <returns>
        /// A list of <see cref="TResultType" />.
        /// </returns>
        public async Task<List<TResultType>> ExecuteAsync(string whereClause = null, params object[] parameters)
        {
            var builder = new StringBuilder(this.CommandText);

            if (!string.IsNullOrWhiteSpace(whereClause))
                builder.AppendFormat(" WHERE {0}", whereClause);

            using (var command = this.Connector.CreateCommand(builder.ToString()))
            {
                if (parameters != null)
                {
                    for (var index = 0; index < parameters.Length; index++)
                    {
                        var parameter = parameters[index];
                        var type = parameter == null ? typeof(object) : parameter.GetType();
                        var commandParameter = command.AddParameter($"@{index + 1}", DbTypeConverter.FromType(type));
                        commandParameter.Value = parameter ?? DBNull.Value;
                    }
                }

                return await this.Connector.ExecuteReaderAsync(command, async reader => await this.Mapper.MapAsync(reader));
            }
        }

        #endregion
    }
}