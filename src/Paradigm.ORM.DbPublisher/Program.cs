using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Paradigm.Core.DependencyInjection.Interfaces;
using Paradigm.ORM.DbPublisher.Builders;
using Paradigm.ORM.DbPublisher.Configuration;
using Paradigm.ORM.DbPublisher.DI;
using Paradigm.ORM.DbPublisher.Logging;
using Paradigm.ORM.DbPublisher.Runners;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Paradigm.ORM.DbPublisher
{
    internal class Program
    {
        private static ILoggingService LoggingService { get; set; }

        private static object Locker { get; set; }

        private static void Main(string[] args)
        {
            Locker = new object();
            LoggingService = new ConsoleLoggingService();
            var started = DateTime.Now;

            LoggingService.Notice("------------------------------------------------------------------------------------------------------");
            LoggingService.Notice("Miracle Devs - Paradigm.ORM");
            LoggingService.Notice("DbPublisher Tool");
            LoggingService.Notice("");
            LoggingService.Notice($"Started at: {started}");
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");

            try
            {
                var commandLineApplication = new CommandLineApplication(false);
                var fileName = commandLineApplication.Option("-c | --config <filename>", "Indicates the path of the configuration file.", CommandOptionType.SingleValue);
                var verbose = commandLineApplication.Option("-v | --verbose", "Indicates the tool to log in detail what is doing.", CommandOptionType.NoValue);
                var watch = commandLineApplication.Option("-w | --watch <path>", "Indicates the tool to watch for changes in the path and execute the publish automatically.", CommandOptionType.SingleValue);
                commandLineApplication.HelpOption("-? | -h | --help");
                commandLineApplication.OnExecute(() => Execute(fileName.Value(), verbose.HasValue(), watch.Value()));
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

        private static int Execute(string configurationFileName, bool verbose, string watch)
        {
            var location = Directory.GetCurrentDirectory();
            configurationFileName = Path.IsPathRooted(configurationFileName) ? configurationFileName : Path.GetFullPath($"{location}/{configurationFileName}");

            if (string.IsNullOrWhiteSpace(configurationFileName))
            {
                throw new Exception("Please provide a configuration file name.");
            }

            if (!File.Exists(configurationFileName))
            {
                throw new Exception("Configuration file does not exist.");
            }

            var publishConfiguration = JsonConvert.DeserializeObject<PublishConfiguration>(File.ReadAllText(configurationFileName));

            if (publishConfiguration == null)
            {
                throw new Exception("Configuration file couldn't be opened");
            }

            var container = DependencyInjection.Register(publishConfiguration);
            var outputFileName = Path.IsPathRooted(publishConfiguration.OutputFileName)
                ? publishConfiguration.OutputFileName
                : Path.GetFullPath($"{Path.GetDirectoryName(configurationFileName)}/{publishConfiguration.OutputFileName}");

            if (watch == null)
            {
                ExecuteConfiguration(container, configurationFileName, outputFileName, verbose, publishConfiguration);
            }
            else
            {
                LoggingService.WriteLine("Starting watch mode: ");


                var watcher = new FileSystemWatcher();
                watcher.BeginInit();
                watcher.IncludeSubdirectories = true;
                watcher.EnableRaisingEvents = true;

                var attr = File.GetAttributes(watch);

                if (attr.HasFlag(FileAttributes.Directory))
                {
                    watcher.Path = watch;
                }
                else
                {
                    watcher.Path = Path.GetDirectoryName(watch);
                    watcher.Filter = Path.GetFileName(watch);
                }

                watcher.Changed += (s, e) => ExecuteConfigurationWatcher(e.ChangeType, e.FullPath, container, configurationFileName, outputFileName, verbose, publishConfiguration);
                watcher.Renamed += (s, e) => ExecuteConfigurationWatcher(e.ChangeType, e.FullPath, container, configurationFileName, outputFileName, verbose, publishConfiguration);
                watcher.Created += (s, e) => ExecuteConfigurationWatcher(e.ChangeType, e.FullPath, container, configurationFileName, outputFileName, verbose, publishConfiguration);
                watcher.Deleted += (s, e) => ExecuteConfigurationWatcher(e.ChangeType, e.FullPath, container, configurationFileName, outputFileName, verbose, publishConfiguration);
                watcher.Error += (s, e) => LoggingService.Error(e.GetException().Message);
                watcher.EndInit();

                while (true)
                {
                }
            }

            return 0;
        }

        private static void ExecuteConfigurationWatcher(WatcherChangeTypes changeType, string changePath, IDependencyContainer container, string configurationFileName, string outputFileName, bool verbose, PublishConfiguration publishConfiguration)
        {
            lock (Locker)
            {
                if (changePath == outputFileName)
                {
                    return;
                }

                Console.Clear();
                LoggingService.Notice("------------------------------------------------------------------------------------------------------");
                LoggingService.Notice($"Path Changed: {changePath}");
                LoggingService.Notice($"Change Type: {changeType}");
                LoggingService.Notice("------------------------------------------------------------------------------------------------------");
                publishConfiguration = JsonConvert.DeserializeObject<PublishConfiguration>(File.ReadAllText(configurationFileName));

                if (publishConfiguration == null)
                {
                    throw new Exception("Configuration file couldn't be opened");
                }

                ExecuteConfiguration(container, configurationFileName, outputFileName, verbose, publishConfiguration);               
            }
        }

        private static void ExecuteConfiguration(IDependencyContainer container, string configurationFileName, string outputFileName, bool verbose, PublishConfiguration publishConfiguration)
        {
            try
            {
                var builder = container.Resolve<IScriptBuilder>();
                var runner = container.Resolve<IScriptRunner>();
                var scripts = GetConfigurationFiles(configurationFileName, publishConfiguration, verbose).ToList();

                LoggingService.WriteLine($"{scripts.Count} script files discovered.");
                LoggingService.Notice("Generating script");
                builder.Build(scripts);

                if (publishConfiguration.ExecuteScript)
                {
                    LoggingService.Notice("Executing scripts");
                    runner.RunAsync(builder, verbose).Wait();
                    LoggingService.Notice("Scripts Executed Successfully.");
                }

                if (publishConfiguration.GenerateScript)
                {
                    builder.SaveScript(outputFileName, verbose);

                    if (verbose)
                    {
                        LoggingService.Notice("Script generated to: ");
                        LoggingService.WriteLine(publishConfiguration.OutputFileName);
                    }
                }
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex.Message);
            }
        }

        private static IEnumerable<string> GetConfigurationFiles(string configFileName, PublishConfiguration configuration, bool verbose)
        {
            var path = Path.GetDirectoryName(configFileName);
            var files = new List<string>();

            if (verbose)
            {
                LoggingService.Notice("Discovering Files");
                LoggingService.WriteLine($"    Directory:       [{path}]");
            }

            foreach (var fileName in configuration.Files)
            {
                var fullFileName = Path.IsPathRooted(fileName) ? fileName : Path.GetFullPath($"{path}/{fileName}");

                if (File.Exists(fullFileName))
                {
                    files.Add(fullFileName);
                }
                else
                {
                    LoggingService.Error($"File not found [{fullFileName}]");
                }
            }

            foreach (var directory in configuration.Paths)
            {
                var fullDirectoryPath = Path.IsPathRooted(directory) ? directory : Path.GetFullPath($"{path}/{directory}");

                if (verbose)
                {
                    LoggingService.WriteLine($"     Processing Directory [{fullDirectoryPath}]");
                }

                if (Directory.Exists(fullDirectoryPath))
                {
                    files.AddRange(Directory.EnumerateFiles(fullDirectoryPath, "*.sql", configuration.TopDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories).OrderBy(x => x));
                }
                else
                {
                    LoggingService.Error($"Directory not found [{fullDirectoryPath}]");
                }
            }

            if (verbose)
            {
                LoggingService.WriteLine(string.Empty);
                LoggingService.Notice("Individual Files:");
                files.ForEach(x => LoggingService.WriteLine($"    {x}"));
            }

            return files;
        }
    }
}