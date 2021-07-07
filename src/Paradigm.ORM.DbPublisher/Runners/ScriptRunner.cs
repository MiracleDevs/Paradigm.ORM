using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Paradigm.ORM.Data.Database;
using Paradigm.ORM.Data.Extensions;
using Paradigm.ORM.DbPublisher.Builders;
using Paradigm.ORM.DbPublisher.Logging;

namespace Paradigm.ORM.DbPublisher.Runners
{
    public class ScriptRunner: IScriptRunner
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
        /// Initializes a new instance of the <see cref="ScriptRunner"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public ScriptRunner(IServiceProvider serviceProvider)
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
        public async Task RunAsync(IScriptBuilder scriptBuilder, bool verbose)
        {
            var loggingService = this.ServiceProvider.GetRequiredService<ILoggingService>();
            var connector = this.ServiceProvider.GetRequiredService<IDatabaseConnector>();

            await connector.OpenAsync();

            foreach (var scriptFileName in scriptBuilder.Scripts.Keys)
            {
                try
                {
                    if (verbose)
                    {
                        loggingService.WriteLine($"Running script: {Path.GetFileName(scriptFileName)}");
                    }

                    var script = scriptBuilder.GetScript(scriptFileName);

                    try
                    {
                        await connector.ExecuteNonQueryAsync(script.Content);
                    }
                    catch
                    {
                        if (!script.IgnoreErrors)
                        {
                            throw;
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