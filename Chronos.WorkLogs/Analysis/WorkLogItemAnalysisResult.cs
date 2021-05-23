using System.Collections.Generic;

namespace Chronos.WorkLogs.Analysis
{
    public class WorkLogItemAnalysisResult
    {
        public IEnumerable<WorkLogItemIssue> WorkLogItemIssues { get; set; }
        public WorkLogItemsStatistics WorkLogItemsStatistics { get; set; }
    }
}
