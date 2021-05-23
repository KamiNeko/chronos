using Chronos.WorkLogs.Analysis;
using Chronos.WorkLogs.Parsing;
using Spectre.Console;
using System.Collections.Generic;
using System.Linq;

namespace Chronos.Commands.Utilities
{
    internal sealed class FileScanResultPrinting
    {
        internal static void Print(
            IEnumerable<WorkLogItem> workLogItems,
            IEnumerable<WorkLogItemIssue> workLogItemIssues,
            WorkLogItemsStatistics workLogItemsStatistics)
        {

            PrintWorklogItemsTable(workLogItems, workLogItemIssues);
            PrintStatistics(workLogItemsStatistics);
        }

        private static void PrintWorklogItemsTable(IEnumerable<WorkLogItem> workLogItems, IEnumerable<WorkLogItemIssue> workLogItemIssues)
        {
            AnsiConsole.Render(new Markup($"[white]Found and parsed {workLogItems.Count()} work log items:[/]").LeftAligned());
            AnsiConsole.WriteLine();

            var table = new Table();
            table.Border = TableBorder.Horizontal;
            table.AddColumn(new TableColumn("[yellow]Booked[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Date[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Timespan[/]").Centered());
            table.AddColumn(new TableColumn("[yellow]Comment[/]").LeftAligned());
            table.AddColumn(new TableColumn("[yellow]Jira Key[/]").Centered());

            foreach (var workLogItem in workLogItems)
            {
                var workLogItemIssue = workLogItemIssues.FirstOrDefault(x => x.WorkLogItem == workLogItem ||
                    (x as WorkLogItemOverlapIssue)?.SecondWorkLogItem == workLogItem ||
                    (x as WorkLogItemGapIssue)?.SecondWorkLogItem == workLogItem);

                var jiraKeyIssue = false;
                var overlapIssue = false;
                var gapIssue = false;

                if (workLogItemIssue is WorkLogItemMissingJiraKeyIssue)
                {
                    jiraKeyIssue = true;
                }
                else if (workLogItemIssue is WorkLogItemOverlapIssue)
                {
                    overlapIssue = true;
                }
                else if (workLogItemIssue is WorkLogItemGapIssue)
                {
                    gapIssue = true;
                }

                var isBooked = workLogItem.IsBooked ? "X" : "-";
                var isBookedText = $"{isBooked}";
                var dateText = workLogItem.Date.ToShortDateString();
                var timeSpanText = $"{ workLogItem.StartTime.ToString(@"hh\:mm") } - { workLogItem.EndTime.ToString(@"hh\:mm")}";
                var commentText = $"{workLogItem.ProjectName}: { workLogItem.Comment}";
                var jiraKeyText = string.IsNullOrEmpty(workLogItem.JiraKey) ? "N/A" : workLogItem.JiraKey;

                if (jiraKeyIssue)
                {
                    jiraKeyText = $"[red slowblink]{jiraKeyText}[/]";
                }

                if (overlapIssue)
                {
                    timeSpanText = $"[red slowblink]{timeSpanText}[/]";
                }

                if (gapIssue)
                {
                    timeSpanText = $"[yellow]{timeSpanText}[/]";
                }

                table.AddRow(isBookedText, dateText, timeSpanText, commentText, jiraKeyText);

                if (gapIssue && workLogItem == workLogItemIssue.WorkLogItem)
                {
                    var workLogItemGapIssue = workLogItemIssue as WorkLogItemGapIssue;
                    table.AddRow("", "", $"[yellow italic dim]GAP ({workLogItemGapIssue.GapDuration.TotalMinutes}m)[/]", "", "");
                }
            }

            table.Expand();
            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }

        private static void PrintStatistics(WorkLogItemsStatistics workLogItemsStatistics)
        {
            AnsiConsole.Render(new Markup($"[white]Statistics:[/]").LeftAligned());
            AnsiConsole.WriteLine();

            var table = new Table();
            table.Border = TableBorder.Square;
            table.HideHeaders();
            table.AddColumn(new TableColumn(""));
            table.AddColumn(new TableColumn(""));

            var statisticsStartTimeText = workLogItemsStatistics.StartTime.ToString(@"hh\:mm");
            var statisticsEndTimeText = workLogItemsStatistics.EndTime.ToString(@"hh\:mm");
            var staticticsWorkDurationText = workLogItemsStatistics.WorkDuration.ToString(@"hh\:mm");
            var statisticsBreakDurationText = workLogItemsStatistics.BreakDuration.ToString(@"hh\:mm");

            table.AddRow("Start time", statisticsStartTimeText);
            table.AddRow("End time", statisticsEndTimeText);
            table.AddRow("Work duration", staticticsWorkDurationText);
            table.AddRow("Break duration", statisticsBreakDurationText);

            AnsiConsole.Render(table);
            AnsiConsole.WriteLine();
        }
    }
}
