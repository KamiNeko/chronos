using System;
using Chronos.WorkLogs.Parsing;

namespace Chronos.WorkLogs.Analysis
{
    public class WorkLogItemOverlapIssue : WorkLogItemIssue
    {
        public WorkLogItem SecondWorkLogItem { get; set; }

        public TimeSpan OverlapDuration { get; set; }

        public override void Print()
        {
            Console.WriteLine($"Found overlap issue ({OverlapDuration.TotalMinutes}m) in following work log items:");
            WorkLogItem.Print();
            SecondWorkLogItem.Print();
            Console.WriteLine();
        }
    }
}
