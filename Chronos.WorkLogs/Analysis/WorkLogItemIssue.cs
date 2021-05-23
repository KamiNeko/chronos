using Chronos.WorkLogs.Parsing;

namespace Chronos.WorkLogs.Analysis
{
    public abstract class WorkLogItemIssue
    {
        public WorkLogItem WorkLogItem { get; set; }

        public abstract void Print();
    }
}
