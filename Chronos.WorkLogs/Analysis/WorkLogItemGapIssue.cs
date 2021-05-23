using System;
using Chronos.WorkLogs.Parsing;

namespace Chronos.WorkLogs.Analysis
{
    public class WorkLogItemGapIssue : WorkLogItemIssue
    {
        public WorkLogItem SecondWorkLogItem { get; set; }
        public TimeSpan GapDuration { get; set; }

        public override void Print()
        {
            Console.WriteLine($"Found gap issue ({GapDuration.TotalMinutes}m) in following work log items:");
            WorkLogItem.Print();
            SecondWorkLogItem.Print();
            Console.WriteLine();
        }
    }
}
