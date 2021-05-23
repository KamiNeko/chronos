using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Chronos.WorkLogs.Parsing
{
    public class WorkLogDocumentParser
    {
        public WorkLogDocumentParser(string filename)
        {
            this.filename = filename ?? throw new ArgumentNullException(nameof(filename));
        }

        public ICollection<WorkLogItem> Parse()
        {
            string text = File.ReadAllText(filename);
            string[] lines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            bool dateTimeIfWorkItemsFound = false;

            DateTime dateOfDocument = DateTime.MinValue;
            List<WorkLogItem> worklogItems = new List<WorkLogItem>();

            foreach (var line in lines)
            {
                // Search date (format: '-- 2021-05-05 --')
                if (line.StartsWith("--") && line.EndsWith("--") && !dateTimeIfWorkItemsFound)
                {
                    string datetimeText = line.Substring(3, 10);
                    dateOfDocument = DateTime.Parse(datetimeText);
                    dateTimeIfWorkItemsFound = true;
                }

                // Search worklog item
                if (line.StartsWith("["))
                {
                    if (!dateTimeIfWorkItemsFound)
                    {
                        throw new InvalidDataException("Did not find any valid date time for the document");
                    }

                    var worklogItem = ParseWorkLogItem(line, out bool worklogItemParsingSuccess);

                    if (worklogItemParsingSuccess)
                    {
                        worklogItem.Date = dateOfDocument;
                        worklogItems.Add(worklogItem);
                    }
                }
            }

            return worklogItems;
        }

        private WorkLogItem ParseWorkLogItem(string line, out bool success)
        {
            var workLogItem = new WorkLogItem();

            if (!line.StartsWith("["))
            {
                success = false;
                return workLogItem;
            }

            try
            {
                workLogItem.RawTextLine = line;

                string workLogItemDoneText = line.Substring(1, 1);
                workLogItem.IsBooked = workLogItemDoneText == "x";

                int indexOfFirstColon = line.IndexOf(':');
                int indexOfSecondColon = line.IndexOf(':', indexOfFirstColon + 1);

                int indexOfStartTime = indexOfFirstColon - 2;
                int indexOfEndTime = indexOfSecondColon - 2;

                string startTimeText = line.Substring(indexOfStartTime, 5);
                string endTimeText = line.Substring(indexOfEndTime, 5);

                workLogItem.StartTime = TimeSpan.Parse(startTimeText);
                workLogItem.EndTime = TimeSpan.Parse(endTimeText);

                int indexOfThirdColon = line.IndexOf(':', indexOfSecondColon + 1);
                int indexOfProjectName = indexOfSecondColon + 3;

                int commentStartIndex = indexOfThirdColon + 1;

                // Older entries don't have a project prefix
                string projectNameTestSubstring = line.Substring(indexOfThirdColon - 5, 5);

                if (projectNameTestSubstring == "https")
                {
                    workLogItem.ProjectName = "";
                    commentStartIndex = indexOfProjectName;
                }
                else 
                {
                    int projectNameLength = indexOfThirdColon - indexOfProjectName;
                    workLogItem.ProjectName = line.Substring(indexOfProjectName, projectNameLength).Trim();
                }

                int linkStartIndex = line.IndexOf(@"(https://");

                bool jiraLinkFound = true;

                // If there is no link, proceed with full remaining line
                if (linkStartIndex == -1)
                {
                    linkStartIndex = line.Length;
                    jiraLinkFound = false;
                }

                int commentLength = linkStartIndex - commentStartIndex;
                workLogItem.Comment = line.Substring(commentStartIndex, commentLength).Trim();

                if (jiraLinkFound)
                {
                    int linkEndIndex = line.IndexOf(")", linkStartIndex);
                    int linkTextLength = linkEndIndex - linkStartIndex;

                    // Remove opening and closing parenthesis with +1 and -1
                    workLogItem.JiraLink = line.Substring(linkStartIndex + 1, linkTextLength - 1);

                    int indexOfJiraKeyStart = workLogItem.JiraLink.LastIndexOf('/') + 1;
                    int lengthOfJiraKey = workLogItem.JiraLink.Length - indexOfJiraKeyStart;
                    workLogItem.JiraKey = workLogItem.JiraLink.Substring(indexOfJiraKeyStart, lengthOfJiraKey);
                }

                success = true;
            }
            catch (Exception)
            {
                success = false;
            }

            return workLogItem;
        }

        private readonly string filename;
    }
}
