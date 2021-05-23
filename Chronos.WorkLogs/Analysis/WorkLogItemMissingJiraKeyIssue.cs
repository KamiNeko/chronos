using System;

namespace Chronos.WorkLogs.Analysis
{
    public class WorkLogItemMissingJiraKeyIssue : WorkLogItemIssue
    {
        public override void Print()
        {
            Console.WriteLine($"Found missing jira key issue in following work log item:");
            WorkLogItem.Print();
            Console.WriteLine();
        }
    }
}
