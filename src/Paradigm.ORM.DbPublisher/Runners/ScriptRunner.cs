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
        private IServiceProvider ServiceProvider { get; }

        public ScriptRunner(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public async Task RunAsync(IScriptBuilder scriptBuilder)
        {
            var loggingService = this.ServiceProvider.GetService<ILoggingService>();
            var connector = this.ServiceProvider.GetService<IDatabaseConnector>();

            await connector.OpenAsync();

            foreach (var script in scriptBuilder.Scripts.Keys)
            {
                try
                {
                    loggingService.WriteLine($"Running script: {Path.GetFileName(script)}");
                    await connector.ExecuteNonQueryAsync(scriptBuilder.GetScript(script));
                }
                catch (Exception ex)
                {
                    do
                    {
                        loggingService.Error($"Error in {Path.GetFileName(script)}: {ex.Message}");
                    } while ((ex = ex.InnerException) != null);
                    break;
                }
            }

            await connector.CloseAsync();
        }
    }
}