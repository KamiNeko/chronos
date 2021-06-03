using Chronos.WorkLogs.Documents;
using Chronos.WorkLogs.Parsing;
using Chronos.WorkLogs.Processing;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;

namespace Chronos.Commands
{
    internal sealed class NewFileCommand
    {
        internal static Command Create(ApiSettings apiSettings)
        {
            if (apiSettings is null)
            {
                throw new ArgumentNullException(nameof(apiSettings));
            }

            var command = new Command("new-file")
            {
                new Option<string>("--input", description: "The date for the new file")
                {
                    IsRequired = true
                }
            };

            command.Description = "Create a new time tracking file for a given date";
            command.Handler = CommandHandler.Create<string>((input) =>
            {
                Run(input, apiSettings);
            });

            return command;
        }

        internal static void Run(string dateText, ApiSettings apiSettings)
        {
            try
            {
                DateTime date = DateTime.Today;

                if (!string.IsNullOrEmpty(dateText))
                {
                    date = DateTime.Parse(dateText);
                }

                var analyzer = new Analyzer(apiSettings.BasePath);
                var analysisResult = analyzer.Analyse();

                var documents = analysisResult.Documents;

                var latestDate = documents.Where(x => x.Date < DateTime.Today)?.Max(x => x.Date);

                string filenameNewFile = Analyzer.GenerateDayFilePath(apiSettings.BasePath, date.Year, date.Month, date.Day);

                if (File.Exists(filenameNewFile))
                {
                    AnsiConsole.WriteLine($"[red]File: {filenameNewFile} already exists![/]");
                    return;
                }

                if (latestDate.HasValue)
                {
                    string filenameTemplate = Analyzer.GenerateDayFilePath(apiSettings.BasePath, latestDate.Value.Year, latestDate.Value.Month, latestDate.Value.Day);
                    CreateNewFileFromTemplate(filenameNewFile, filenameTemplate, date);
                }
                else
                {
                    CreateNewFile(filenameNewFile, date);
                }

                AnsiConsole.WriteLine($"Created new file: {filenameNewFile}");
            }
            catch
            {
                AnsiConsole.Render(new Markup("[bold red]Unexpected error![/]").LeftAligned());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
            }
        }

        private static void CreateNewFile(string filename, DateTime date)
        {
            CreateDirectoryForFileIfNotExists(filename);

            StringWriter stringWriter = new StringWriter();

            stringWriter.WriteLine($"-- {date.Year:D4}-{date.Month:D2}-{date.Day:D2} --");

            stringWriter.WriteLine();
            stringWriter.WriteLine();
            stringWriter.WriteLine();

            stringWriter.WriteLine("----------------");
            stringWriter.WriteLine("    Notizen     ");
            stringWriter.WriteLine("----------------");

            stringWriter.WriteLine();

            var lines = stringWriter.ToString();

            File.WriteAllText(filename, lines);
        }

        private static void CreateNewFileFromTemplate(string filenameNewFile, string templateFilename, DateTime date)
        {
            CreateDirectoryForFileIfNotExists(filenameNewFile);

            // Take template and make a 1:1 copy into new file
            File.Copy(templateFilename, filenameNewFile);

            // Process the new file, remove all worklog items
            var processor = new WorkLogDocumentProcessor(filenameNewFile, new List<WorkLogItem>());

            // Update file with given date
            processor.UpdateDocument(date: date, removeWorkLogItems: true, removeBreakMarks: true);
        }

        private static void CreateDirectoryForFileIfNotExists(string filename)
        {
            var directoryNameNewFile = Path.GetDirectoryName(filename);

            if (!Directory.Exists(directoryNameNewFile))
            {
                Directory.CreateDirectory(directoryNameNewFile);
            }
        }
    }
}
