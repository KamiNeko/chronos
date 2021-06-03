using Chronos.WorkLogs.Documents;
using Spectre.Console;
using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.IO;
using System.Linq;

namespace Chronos.Commands
{
    internal sealed class ScanDirectoryCommand
    {
        internal static Command Create()
        {
            var scanDirectoryCommand = new Command("scan-dir")
            {
                new Option<string>("--input", description: "The input directory path")
                {
                    IsRequired = true
                }
            };

            scanDirectoryCommand.Description = "Scan a base directory for time tracking files";
            scanDirectoryCommand.Handler = CommandHandler.Create<string>((input) =>
            {
                Run(input);
            });

            return scanDirectoryCommand;
        }

        internal static void Run(string directory)
        {
            AnsiConsole.ResetColors();
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Markup($"Scanning directory [italic]'{directory}'[/]").LeftAligned());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();

            if (!Directory.Exists(directory))
            {
                AnsiConsole.Render(new Markup("[bold red]Directory not found, please make sure to enter a valid directory![/]").LeftAligned());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
                return;
            }

            try
            {
                var analyzer = new Analyzer(directory);
                var analysisResult = analyzer.Analyse();

                var documents = analysisResult.Documents;

                // Table listing all worklog items
                var table = new Table();
                table.Border = TableBorder.Horizontal;
                table.AddColumn(new TableColumn("[yellow]Date[/]").Centered());
                table.AddColumn(new TableColumn("[yellow]Work Time[/]").Centered());
                table.AddColumn(new TableColumn("[yellow]Break Time[/]").Centered());
                table.AddColumn(new TableColumn("[yellow]# Items[/]").Centered());
                table.AddColumn(new TableColumn("[yellow]# Booked[/]").Centered());
                table.AddColumn(new TableColumn("[yellow]Filename[/]").Centered());

                var lastDate = DateTime.MinValue;

                foreach (var document in documents)
                {
                    var workLogItemIssues = document.WorkLogItemAnalysis.WorkLogItemIssues;

                    var dateText = document.Date.ToShortDateString();

                    var numberItems = document.WorkLogItems.Count();
                    var numberBooked = document.WorkLogItems.Count(x => x.IsBooked);

                    var numberItemsText = $"{numberItems}";
                    var numberItemsBookedText = $"{numberBooked}";
                    var staticticsWorkDurationText = document.WorkLogItemAnalysis.WorkLogItemsStatistics.WorkDuration.ToString(@"hh\:mm");
                    var statisticsBreakDurationText = document.WorkLogItemAnalysis.WorkLogItemsStatistics.BreakDuration.ToString(@"hh\:mm");
                    string filenameText = Path.GetFileName(document.Filename);

                    if (numberItems != numberBooked)
                    {
                        numberItemsText = $"[red slowblink]{numberItemsText}[/]";
                        numberItemsBookedText = $"[red slowblink]{numberItemsBookedText}[/]";
                    }

                    if (lastDate != DateTime.MinValue && lastDate.Month != document.Date.Month)
                    {
                        table.AddEmptyRow();
                    }

                    table.AddRow(dateText, staticticsWorkDurationText, statisticsBreakDurationText, numberItemsText, numberItemsBookedText, filenameText);

                    lastDate = document.Date;
                }

                AnsiConsole.Render(table);
                AnsiConsole.WriteLine();
            }
            catch
            {
                AnsiConsole.Render(new Markup("[bold red]Unexpected error![/]").LeftAligned());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
            }
        }
    }
}
