using Chronos.AtlassianApi;
using Chronos.AtlassianApi.Dto.Jira;
using Chronos.AtlassianApi.Dto.Tempo;
using Chronos.Commands.Utilities;
using Chronos.WorkLogs.Analysis;
using Chronos.WorkLogs.Parsing;
using Chronos.WorkLogs.Processing;
using Spectre.Console;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Chronos.Commands
{
    internal class ProcessFileCommand
    {
        internal static Command Create(ApiSettings apiSettings)
        {
            var scanDirectoryCommand = new Command("process-file")
                {
                    new Option<string>("--input", description: "The input file name")
                    {
                        IsRequired = true
                    }
                };

            scanDirectoryCommand.Description = "Process a time tracking file";
            scanDirectoryCommand.Handler = CommandHandler.Create<string>(async (input) =>
            {
                await Run(input, apiSettings);
            });

            return scanDirectoryCommand;
        }

        internal static async Task Run(string filename, ApiSettings apiSettings)
        {
            AnsiConsole.ResetColors();
            AnsiConsole.WriteLine();
            AnsiConsole.Render(new Markup($"Processing file [italic]'{filename}'[/]").LeftAligned());
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

                if (!AnsiConsole.Confirm("Do you want to upload the time tracking entries to Tempo?"))
                {
                    return;
                }

                var stopwatch = new Stopwatch();
                stopwatch.Start();

                AnsiConsole.WriteLine();

                var jiraApi = new JiraApi(apiSettings.Email, apiSettings.JiraCloudInstanceName, apiSettings.TempoToken, apiSettings.JiraToken);
                IssueRoot jiraIssue = null;

                await AnsiConsole.Status()
                    .StartAsync("Processing...", async ctx =>
                    {
                        ctx.Spinner(Spinner.Known.Star);
                        ctx.SpinnerStyle(Style.Parse("yellow"));

                        foreach (var workLogItem in workLogItems)
                        {
                            string processingItemText = $"Processing entry {workLogItem.StartTime.ToString(@"hh\:mm")} - {workLogItem.EndTime.ToString(@"hh\:mm")}: ";

                            if (string.IsNullOrEmpty(workLogItem.JiraKey))
                            {
                                AnsiConsole.MarkupLine(processingItemText + "[red]ERROR (no Jira key)[/]");
                                continue;
                            }

                            if (workLogItem.IsBooked)
                            {
                                AnsiConsole.MarkupLine(processingItemText + "[white]SKIP (already booked)[/]");
                                continue;
                            }

                            jiraIssue = await jiraApi.GetJiraIssue(workLogItem.JiraKey);

                            if (jiraIssue.Fields.Customfield_10216 != null)
                            {
                                int accountId = jiraIssue.Fields.Customfield_10216.Id;
                                var accountKey = jiraApi.GetAccountKeyByAccountId(accountId);

                                var workLogItemCreateEntry = Translate(workLogItem, accountKey, apiSettings.JiraUserId);

                                var result = jiraApi.CreateWorklog(workLogItemCreateEntry);

                                if (result.Issue == null)
                                {
                                    continue;
                                }

                                workLogItem.IsBooked = true;

                                AnsiConsole.MarkupLine(processingItemText + "[green]OK[/]");
                            }
                            else
                            {
                                AnsiConsole.MarkupLine(processingItemText + "[red]ERROR (no default issue account)[/]");
                            }
                        }
                    });

                AnsiConsole.WriteLine();
                AnsiConsole.MarkupLine("Updating booking state in time tracking file.");

                var processor = new WorkLogDocumentProcessor(filename, workLogItems);
                processor.UpdateDocument();

                AnsiConsole.MarkupLine($"Processing finished in [italic]{stopwatch.ElapsedMilliseconds}ms[/].");
                AnsiConsole.WriteLine();
            }
            catch
            {
                AnsiConsole.Render(new Markup("[bold red]Unexpected error![/]").LeftAligned());
                AnsiConsole.WriteLine();
                AnsiConsole.WriteLine();
            }
        }

        private static WorkLogItemCreate Translate(WorkLogItem workLogItem, string accountKey, string authorId)
        {
            bool setActivityType = false;
            string activityTypeText = "";

            if (workLogItem.Comment.Contains("Besprechung") || workLogItem.Comment.Contains("Meeting"))
            {
                setActivityType = true;
                activityTypeText = "Meeting";
            }
            else if (workLogItem.Comment.Contains("Testen") || workLogItem.Comment.Contains("Tests"))
            {
                setActivityType = true;
                activityTypeText = "Testing";
            }
            else if (workLogItem.Comment.Contains("Zeiterfassung"))
            {
                setActivityType = true;
                activityTypeText = "Administrative";
            }

            var timespan = workLogItem.EndTime - workLogItem.StartTime;
            var timeSpentInSeconds = (int)timespan.TotalSeconds;

            var workLogItemCreateEntry = new WorkLogItemCreate
            {
                IssueKey = workLogItem.JiraKey,
                AuthorAccountId = authorId,
                Description = workLogItem.Comment,
                StartDate = workLogItem.Date.ToString("yyyy-MM-dd"),
                StartTime = workLogItem.StartTime.ToString(@"hh\:mm\:ss"),
                BillableSeconds = timeSpentInSeconds,
                TimeSpentSeconds = timeSpentInSeconds,
                Attributes = new System.Collections.Generic.List<Chronos.AtlassianApi.Dto.Tempo.Attribute>()
            };

            workLogItemCreateEntry.Attributes.Add(new Chronos.AtlassianApi.Dto.Tempo.Attribute
            {
                Key = "_Kostenstelle_",
                Value = accountKey
            });

            if (setActivityType)
            {
                workLogItemCreateEntry.Attributes.Add(new Chronos.AtlassianApi.Dto.Tempo.Attribute
                {
                    Key = "_Tätigkeitstyp_",
                    Value = activityTypeText
                });
            }

            return workLogItemCreateEntry;
        }
    }
}
