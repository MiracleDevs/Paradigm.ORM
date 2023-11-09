using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.DbPublisher.Builders;
using Paradigm.ORM.DbPublisher.Logging;

namespace Paradigm.ORM.DbPublisher.Runners
{
    public class CommandScriptRunner : ICommandScriptRunner
    {
        #region Properties

        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        private IServiceProvider ServiceProvider { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandScriptRunner"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public CommandScriptRunner(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs the scripts.
        /// </summary>
        /// <param name="scriptBuilder">The script builder.</param>
        /// <param name="verbose">if set to <c>true</c> [verbose].</param>
        public async Task RunAsync(ICommandScriptBuilder scriptBuilder, bool verbose)
        {
            var loggingService = this.ServiceProvider.GetRequiredService<ILoggingService>();
            var connector = this.ServiceProvider.GetRequiredService<IDatabaseConnector>();

            await connector.OpenAsync();

            foreach (var scriptFileName in scriptBuilder.CommandScripts.Keys)
            {
                try
                {
                    var commandScripts = scriptBuilder.GetCommandScript(scriptFileName);

                    if (verbose)
                    {
                        loggingService.WriteLine($"Running file '{Path.GetFileName(scriptFileName)}' with '{commandScripts.Count()}' command scripts");
                    }

                    foreach (var commandScript in commandScripts)
                    {
                        try
                        {
                            await connector.ExecuteNonQueryAsync(commandScript.Content);
                        }
                        catch
                        {
                            if (!commandScript.IgnoreErrors)
                            {
                                throw;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    do
                    {
                        loggingService.Error($"Error in {Path.GetFileName(scriptFileName)}: {ex.Message}");
                    } while ((ex = ex.InnerException) != null);
                    break;
                }
            }

            await connector.CloseAsync();
        }

        #endregion
    }
}