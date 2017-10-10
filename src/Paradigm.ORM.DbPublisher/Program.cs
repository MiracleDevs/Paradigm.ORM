using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.CommandLineUtils;
using Paradigm.ORM.DbPublisher.Builders;
using Paradigm.ORM.DbPublisher.Configuration;
using Paradigm.ORM.DbPublisher.DI;
using Paradigm.ORM.DbPublisher.Logging;
using Paradigm.ORM.DbPublisher.Runners;
using Newtonsoft.Json;

namespace Paradigm.ORM.DbPublisher
{
    internal class Program
    {
        private static ILoggingService LoggingService { get; set; }

        private static void Main(string[] args)
        {
            LoggingService = new ConsoleLoggingService();
            var started = DateTime.Now;

            LoggingService.Notice("------------------------------------------------------------------------------------------------------");
            LoggingService.Notice("Miracle Devs - ORM");
            LoggingService.Notice("DbPublisher Tool");
            LoggingService.Notice("");
            LoggingService.Notice($"Started at: {started}");
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");

            try
            {
                var commandLineApplication = new CommandLineApplication(false);
                var fileName = commandLineApplication.Option("-c | --config <filename>", "Indicates the path of the configuration file.", CommandOptionType.SingleValue);
                var verbose = commandLineApplication.Option("-v | --verbose", "Indicates the tool to log in detail what is doing.", CommandOptionType.NoValue);
                commandLineApplication.HelpOption("-? | -h | --help");
                commandLineApplication.OnExecute(() => OpenConfigurationFileAsync(fileName.Value(), verbose.HasValue()).Result);
                commandLineApplication.Execute(args);
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex.Message);
            }

            var ended = DateTime.Now;
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");
            LoggingService.Notice($"Ended at: {ended}");
            LoggingService.Notice($"Elapsed: { new TimeSpan(ended.Subtract(started).Ticks).TotalSeconds } sec");
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");

#if DEBUG
            Console.ReadKey();
#endif
        }

        private static async Task<int> OpenConfigurationFileAsync(string fileName, bool verbose)
        {
            var location = Directory.GetCurrentDirectory();
            fileName = Path.GetFullPath($"{location}/{fileName}");

            if (string.IsNullOrWhiteSpace(fileName))
                throw new Exception("Please provide a configuration file name.");

            if (!File.Exists(fileName))
                throw new Exception("Configuration file does not exist.");

            var publishConfiguration = JsonConvert.DeserializeObject<PublishConfiguration>(File.ReadAllText(fileName));

            if (publishConfiguration == null)
                throw new Exception("Configuration file couldn't be opened");

            var container = DependencyInjection.Register(publishConfiguration);
            var builder = container.Resolve<IScriptBuilder>();
            var runner = container.Resolve<IScriptRunner>();
            var scripts = GetConfigurationFiles(fileName, publishConfiguration, verbose).ToList();

            LoggingService.WriteLine($"{scripts.Count} script files discovered.");

            if (publishConfiguration.GenerateScript)
            {
                LoggingService.Notice("Generating script");
                builder.Build(scripts);              
                builder.SaveScript(Path.IsPathRooted(publishConfiguration.OutputFileName) ? publishConfiguration.OutputFileName : Path.GetFullPath($"{Path.GetDirectoryName(fileName)}/{publishConfiguration.OutputFileName}"));
                LoggingService.Notice("Script generated to: ");
                LoggingService.WriteLine(publishConfiguration.OutputFileName);
            }

            if (publishConfiguration.ExecuteScript)
            {
                LoggingService.Notice("Executing scripts");
                await runner.RunAsync(builder);
                LoggingService.Notice("Scripts Executed Successfully.");
            }

            return 0;
        }

        private static IEnumerable<string> GetConfigurationFiles(string configFileName, PublishConfiguration configuration, bool verbose)
        {   
            var path = Path.GetDirectoryName(configFileName);
            var files = new List<string>();

            if (verbose)
            {
                LoggingService.Notice($"Discovering Files");
                LoggingService.WriteLine($"    Directory:       [{path}]");
            }

            foreach (var fileName in configuration.Files)
            {
                var fullFileName = Path.IsPathRooted(fileName) ? fileName : Path.GetFullPath($"{path}/{fileName}");

                if (File.Exists(fullFileName))
                    files.Add(fullFileName);
                else
                    LoggingService.Error($"File not found [{fullFileName}]");
            }

            foreach (var directory in configuration.Paths)
            {
                var fullDirectoryPath = Path.IsPathRooted(directory) ? directory : Path.GetFullPath($"{path}/{directory}");

                if (verbose)
                    LoggingService.WriteLine($"     Processing Directory [{fullDirectoryPath}]");

                if (Directory.Exists(fullDirectoryPath))
                    files.AddRange(Directory.EnumerateFiles(fullDirectoryPath, "*.sql", configuration.TopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories).OrderBy(x => x));
                else
                    LoggingService.Error($"Directory not found [{fullDirectoryPath}]");
            }

            LoggingService.WriteLine(string.Empty);
            LoggingService.Notice("Individual Files:");
            files.ForEach(x => LoggingService.WriteLine($"    {x}"));

            return files;
        }
    }
}