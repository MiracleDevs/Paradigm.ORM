using System;
using System.IO;
using Microsoft.Extensions.CommandLineUtils;
using Paradigm.ORM.DataExport.Configuration;
using Paradigm.ORM.DataExport.Export;
using Paradigm.ORM.DataExport.Logging;
using Newtonsoft.Json;

namespace Paradigm.ORM.DataExport
{
    internal class Program
    {
        private static ILoggingService LoggingService { get; set; }

        private static void Main(string[] args)
        {
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
                var fileName = commandLineApplication.Option("-f | --file <configuration-file>", "A path to a configuration file.", CommandOptionType.SingleValue);
                var verbose = commandLineApplication.Option("-v | --verbose", "Indicates the tool to log in detail what is doing.", CommandOptionType.NoValue);

                commandLineApplication.HelpOption("-? | -h | --help");
                commandLineApplication.OnExecute(() => Process(fileName.Value(), verbose.HasValue()));

                commandLineApplication.Execute(args);
            }
            catch (Exception ex)
            {
                LoggingService.Error(ex.Message);
            }
        }

        private static int Process(string fileName, bool verbose)
        {
            using (var exporter = ExporterFactory.Create(LoggingService, GetConfiguration(fileName, verbose), verbose))
            {
                exporter.Export();
            }

            return 0;
        }

        private static Configuration.Configuration GetConfiguration(string fileName, bool verbose)
        {
            if (verbose)
            {
                LoggingService.WriteLine($"Opening configuration file: [{fileName}]\n");
            }

            if (!File.Exists(fileName))
                throw new FileNotFoundException($"The file '{fileName}' can not be found.");

            var path = Directory.GetCurrentDirectory();
            fileName = Path.IsPathRooted(fileName) ? fileName : Path.GetFullPath($"{path}/{fileName}");

            var configuration = JsonConvert.DeserializeObject<Configuration.Configuration>(File.ReadAllText(fileName));

            if (verbose)
            {
                LoggingService.Notice("Soruce");
                LoggingService.WriteLine($"Source Type:      [{configuration.SourceDatabase.DatabaseType}]");
                LoggingService.WriteLine($"Database Name:    [{configuration.SourceDatabase.DatabaseName}]");
                LoggingService.WriteLine($"Tables:           [{string.Join(", ", configuration.TableNames)}]\n");

                LoggingService.Notice("Destination");
                LoggingService.WriteLine($"Export Type:      [{configuration.ExportType}]");

                if (configuration.ExportType == ExportType.File)
                {
                    LoggingService.WriteLine($"Export File Type: [{configuration.DestinationFile.FileType}]");
                    LoggingService.WriteLine($"Export File Name: [{configuration.DestinationFile.FileName}]");
                }
                else
                {
                    LoggingService.WriteLine($"Export db Type:   [{configuration.DestinationDatabase.DatabaseType}]");
                    LoggingService.WriteLine($"Export db Name:   [{configuration.DestinationDatabase.DatabaseName}]\n");
                }
            }

            return configuration;
        }
    }
}