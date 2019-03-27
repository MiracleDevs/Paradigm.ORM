using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Extensions.CommandLineUtils;
using Paradigm.ORM.DbFirst.Configuration;
using Paradigm.ORM.DbFirst.Export;
using Paradigm.ORM.DbFirst.Logging;
using Paradigm.ORM.DbFirst.Mapping;
using Newtonsoft.Json;

namespace Paradigm.ORM.DbFirst
{
    internal class Program
    {
        private static ILoggingService LoggingService { get; set; }

        private static object Locker { get; set; }

        private static void Main(string[] args)
        {
            Locker = new object();
            Mappings.Initialize();

            LoggingService = new ConsoleLoggingService();
            var started = DateTime.Now;

            LoggingService.Notice("------------------------------------------------------------------------------------------------------");
            LoggingService.Notice("Miracle Devs - Paradigm.ORM");
            LoggingService.Notice("DbFirst Tool");
            LoggingService.Notice("");
            LoggingService.Notice($"Started at: {started}");
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");

            ParseCommandLine(args);

            var ended = DateTime.Now;
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");
            LoggingService.Notice($"Ended at: {ended}");
            LoggingService.Notice($"Elapsed: { new TimeSpan(ended.Subtract(started).Ticks).TotalSeconds } sec");
            LoggingService.Notice("------------------------------------------------------------------------------------------------------");

#if DEBUG
            Console.ReadKey();
#endif
        }

        private static void ParseCommandLine(params string[] args)
        {
            try
            {
                var commandLineApplication = new CommandLineApplication(false);
                var fileNames = commandLineApplication.Option("-f | --filenames <filename>", "Indicates the path of one or more configuration files.", CommandOptionType.MultipleValue);
                var directories = commandLineApplication.Option("-d | --directories <directory>", "Indicates one or more directories in which the tool should search for configuration files.", CommandOptionType.MultipleValue);
                var topDirectoryOnly = commandLineApplication.Option("-t | --topdirectory", "If directories were provided, indicates if the system should check only on the top directory.", CommandOptionType.NoValue);
                var extension = commandLineApplication.Option("-e | --extension <extension>", "Indicates the extension of configuration files when searching inside directories. The default extension is 'json'.", CommandOptionType.SingleValue);
                var verbose = commandLineApplication.Option("-v | --verbose", "Indicates the tool to log in detail what is doing.", CommandOptionType.NoValue);
                var watch = commandLineApplication.Option("-w | --watch <path>", "Indicates the tool to watch for changes in the path and execute the tool automatically.", CommandOptionType.SingleValue);

                commandLineApplication.HelpOption("-? | -h | --help");
                commandLineApplication.OnExecute(() =>  Execute(fileNames.Values, directories.Values, topDirectoryOnly.HasValue(), extension.Value(), verbose.HasValue(), watch.Value()));

                commandLineApplication.Execute(args);
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex.Message);
            }
        }

        private static int Execute(List<string> fileNames, List<string> directories, bool topDirectoryOnly, string extension, bool verbose, string watch)
        {
            if (watch == null)
            {
                IterateOverFiles(fileNames, directories, topDirectoryOnly, extension, verbose);
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

                watcher.Changed += (s, e) => DetectChanges(e.ChangeType, e.FullPath, fileNames, directories, topDirectoryOnly, extension, verbose);
                watcher.Renamed += (s, e) => DetectChanges(e.ChangeType, e.FullPath, fileNames, directories, topDirectoryOnly, extension, verbose);
                watcher.Created += (s, e) => DetectChanges(e.ChangeType, e.FullPath, fileNames, directories, topDirectoryOnly, extension, verbose);
                watcher.Deleted += (s, e) => DetectChanges(e.ChangeType, e.FullPath, fileNames, directories, topDirectoryOnly, extension, verbose);
                watcher.Error += (s, e) => LoggingService.Error(e.GetException().Message);
                watcher.EndInit();

                while (true)
                {
                }
            }

            return 0;
        }

        private static void DetectChanges(WatcherChangeTypes changeType, string changePath, List<string> fileNames, List<string> directories, bool topDirectoryOnly, string extension, bool verbose)
        {
            lock (Locker)
            {
                Console.Clear();
                LoggingService.Notice("------------------------------------------------------------------------------------------------------");
                LoggingService.Notice($"Path Changed: {changePath}");
                LoggingService.Notice($"Change Type: {changeType}");
                LoggingService.Notice("------------------------------------------------------------------------------------------------------");

                IterateOverFiles(fileNames, directories, topDirectoryOnly, extension, verbose);
            }
        }

        private static void IterateOverFiles(List<string> fileNames, List<string> directories, bool topDirectoryOnly, string extension, bool verbose)
        {
            foreach (var configurationFileName in GetConfigurationFiles(fileNames, directories, topDirectoryOnly, extension, verbose).ToList())
            {
                OpenConfigurationFile(configurationFileName);
            }
        }

        private static void OpenConfigurationFile(string configurationFileName)
        {
            LoggingService.WriteLine($"Opening configuration file [{Path.GetFileName(configurationFileName)}]");
            var configuration = JsonConvert.DeserializeObject<DbFirstConfiguration>(File.ReadAllText(configurationFileName));
            DbFirstExporterFactory.Create(configuration.DatabaseType).ExportAsync(configurationFileName, configuration).Wait();
        }

        private static IEnumerable<string> GetConfigurationFiles(List<string> fileNames, IEnumerable<string> directories, bool topDirectoryOnly, string extension, bool verbose)
        {
            var path = Directory.GetCurrentDirectory();
            var files = new List<string>();

            if (verbose)
            {
                LoggingService.Notice("Discovering Files");
                LoggingService.WriteLine($"    Directory:       [{path}]");
            }

            foreach (var fileName in fileNames)
            {
                var fullFileName = Path.IsPathRooted(fileName) ? fileName : Path.GetFullPath($"{path}/{fileName}");

                if (File.Exists(fullFileName))
                    files.Add(fullFileName);
                else
                    LoggingService.Error($"File not found [{fullFileName}]");
            }

            foreach (var directory in directories)
            {
                var fullDirectoryPath = Path.IsPathRooted(directory)? directory : Path.GetFullPath($"{path}/{directory}");

                if (verbose)
                    LoggingService.WriteLine($"     Processing Directory [{fullDirectoryPath}]");

                if (Directory.Exists(fullDirectoryPath))
                    files.AddRange(Directory.EnumerateFiles(fullDirectoryPath, $"*.{extension ?? "json"}", topDirectoryOnly ? SearchOption.TopDirectoryOnly : SearchOption.AllDirectories));
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