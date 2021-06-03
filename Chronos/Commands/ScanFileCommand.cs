using Chronos.Commands.Utilities;
using Chronos.WorkLogs.Analysis;
using Chronos.WorkLogs.Parsing;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Chronos.Commands
{
    internal sealed class ScanFileCommand
    {
        internal static Command Create()
        {
            var scanFileCommand = new Command("scan-file")
            {
                new Option<string>("--input", description: "The input file name")
                {
                    IsRequired = true
                }
            };

            scanFileCommand.Description = "Scan a time tracking *.txt file";
            scanFileCommand.Handler = CommandHandler.Create<string>((input) =>
            {
                Run(input);
            });

            return scanFileCommand;
        }

        internal static void Run(string filename)
        {
            AnsiConsole.ResetColors();
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Markup($"Scanning file [italic]'{filename}'[/]").LeftAligned());
            AnsiConsole.WriteLine();
            AnsiConsole.WriteLine();

            if (!FileErrorPrinting.CheckFileExistanceAndPrintError(filename))
            {
                return;
            }

            if (!FileErrorPrinting.CheckFileExtensionAndPrintError(filename))
            {
                return;
            }

            try
            {
                var parser = new WorkLogDocumentParser(filename);
                var workLogItems = parser.Parse();

                var analyzer = new WorkLogItemsAnalizer(workLogItems);
                var analysisResult = analyzer.Analyze();
                var workLogItemIssues = analysisResult.WorkLogItemIssues;
                var workLogItemStatistics = analysisResult.WorkLogItemsStatistics;

                FileScanResultPrinting.Print(workLogItems, workLogItemIssues, workLogItemStatistics);
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
