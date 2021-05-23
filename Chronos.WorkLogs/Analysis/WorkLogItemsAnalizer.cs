using System;
using System.Collections.Generic;
using System.Linq;
using Chronos.WorkLogs.Parsing;

namespace Chronos.WorkLogs.Analysis
{
    public class WorkLogItemsAnalizer
    {
        public WorkLogItemsAnalizer(IEnumerable<WorkLogItem> workLogItems)
        {
            this.workLogItems = workLogItems ?? throw new ArgumentNullException(nameof(workLogItems));
        }

        public WorkLogItemAnalysisResult Analyze()
        {
            var analysisResult = new WorkLogItemAnalysisResult();
            var workLogItemIssues = new List<WorkLogItemIssue>();
            var orderedWorklogItems = workLogItems.OrderBy(x => x.StartTime).ToArray();

            for (int i = 0; i < orderedWorklogItems.Count() - 1; ++i)
            {
                var currentWorkLogItem = orderedWorklogItems[i];

                CheckForMissingJiraKeyIssue(currentWorkLogItem, workLogItemIssues);

                for (int j = i + 1; j < orderedWorklogItems.Count(); ++j)
                {
                    var nextWorkLogItem = orderedWorklogItems[j];
                    bool workLogItemsAreAdjacent = j == i + 1;

                    CheckForOverlappingTimesAndAppendIssue(currentWorkLogItem, nextWorkLogItem, workLogItemIssues);
                    CheckForGapAndAppendIssue(currentWorkLogItem, nextWorkLogItem, workLogItemsAreAdjacent, workLogItemIssues);
                }
            }

            // Check last one for missing jira issue key
            if (orderedWorklogItems.Count() > 1)
            {
                var lastWorkLogItem = orderedWorklogItems.Last();
                CheckForMissingJiraKeyIssue(lastWorkLogItem, workLogItemIssues);
            }

            analysisResult.WorkLogItemIssues = workLogItemIssues;
            analysisResult.WorkLogItemsStatistics = CreateWorkLogItemsStatistics(workLogItemIssues);

            return analysisResult;
        }

        private WorkLogItemsStatistics CreateWorkLogItemsStatistics(IEnumerable<WorkLogItemIssue> workLogItemIssues)
        {
            var workLogItemsStatistics = new WorkLogItemsStatistics()
            {
                StartTime = TimeSpan.MaxValue,
                EndTime = TimeSpan.MinValue,
                WorkDuration = TimeSpan.FromMinutes(0),
                BreakDuration = TimeSpan.FromMinutes(0)
            };

            foreach (var workLogItem in workLogItems)
            {
                if (workLogItem.StartTime < workLogItemsStatistics.StartTime)
                {
                    workLogItemsStatistics.StartTime = workLogItem.StartTime;
                }

                if (workLogItem.EndTime > workLogItemsStatistics.EndTime)
                {
                    workLogItemsStatistics.EndTime = workLogItem.EndTime;
                }

                workLogItemsStatistics.WorkDuration += workLogItem.EndTime - workLogItem.StartTime;
            }

            foreach (var workLogItemIssue in workLogItemIssues)
            {
                if (workLogItemIssue is WorkLogItemGapIssue workLogItemGapIssue)
                {
                    workLogItemsStatistics.BreakDuration += workLogItemGapIssue.GapDuration;
                }
            }

            return workLogItemsStatistics;
        }

        private void CheckForOverlappingTimesAndAppendIssue(WorkLogItem firstWorkLogItem, WorkLogItem secondWorkLogItem, ICollection<WorkLogItemIssue> workLogItemIssues)
        {
            bool itemsOverlap = firstWorkLogItem.EndTime > secondWorkLogItem.StartTime;

            if (itemsOverlap)
            {
                TimeSpan overlapDuration = firstWorkLogItem.EndTime - secondWorkLogItem.StartTime;
                var workLogItemOverlapIssue = new WorkLogItemOverlapIssue
                {
                    WorkLogItem = firstWorkLogItem,
                    SecondWorkLogItem = secondWorkLogItem,
                    OverlapDuration = overlapDuration
                };

                workLogItemIssues.Add(workLogItemOverlapIssue);
            }
        }

        private void CheckForGapAndAppendIssue(WorkLogItem firstWorkLogItem, WorkLogItem secondWorkLogItem, bool workLogItemsAreAdjacent, ICollection<WorkLogItemIssue> workLogItemIssues)
        {
            // Only check adjacent work log items
            if (!workLogItemsAreAdjacent)
            {
                return;
            }

            bool timesHaveAGap = secondWorkLogItem.StartTime - firstWorkLogItem.EndTime > TimeSpan.FromMinutes(0);

            if (timesHaveAGap)
            {
                TimeSpan apDuration = secondWorkLogItem.StartTime - firstWorkLogItem.EndTime;
                var workLogItemGapIssue = new WorkLogItemGapIssue
                {
                    WorkLogItem = firstWorkLogItem,
                    SecondWorkLogItem = secondWorkLogItem,
                    GapDuration = apDuration
                };

                workLogItemIssues.Add(workLogItemGapIssue);
            }
        }

        private void CheckForMissingJiraKeyIssue(WorkLogItem workLogItem, ICollection<WorkLogItemIssue> workLogItemIssues)
        {
            bool jiraKeyIsMissing = string.IsNullOrEmpty(workLogItem.JiraKey);

            if (jiraKeyIsMissing)
            {
                var workLogItemMissingJiraKeyIssue = new WorkLogItemMissingJiraKeyIssue
                {
                    WorkLogItem = workLogItem
                };

                workLogItemIssues.Add(workLogItemMissingJiraKeyIssue);
            }
        }

        private readonly IEnumerable<WorkLogItem> workLogItems;
    }
}
