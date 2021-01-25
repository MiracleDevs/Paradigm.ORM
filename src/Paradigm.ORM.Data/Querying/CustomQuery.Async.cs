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
            var formatProvider = this.Connector.GetCommandFormatProvider();

            if (!string.IsNullOrWhiteSpace(whereClause))
                builder.AppendFormat(" WHERE {0}", whereClause);

            using var command = this.Connector.CreateCommand();
            if (parameters != null)
            {
                for (var index = 0; index < parameters.Length; index++)
                {
                    var oldName = $"@{index + 1}";
                    var newName = formatProvider.GetParameterName($"p{(index + 1)}");

                    builder.Replace(oldName, newName);

                    var parameter = parameters[index];
                    var type = parameter == null ? typeof(object) : parameter.GetType();
                    var commandParameter = command.AddParameter(newName, DbTypeConverter.FromType(type));
                    commandParameter.Value = parameter;
                }
            }

            command.CommandText = builder.ToString();
            return await this.Connector.ExecuteReaderAsync(command, async reader => await this.Mapper.MapAsync(reader));
        }

        #endregion
    }
}