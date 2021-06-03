using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Chronos.WorkLogs.Parsing;

namespace Chronos.WorkLogs.Processing
{
    public class WorkLogDocumentProcessor
    {
        public WorkLogDocumentProcessor(string filename, IEnumerable<WorkLogItem> workLogItems)
        {
            this.filename = filename ?? throw new ArgumentNullException(nameof(filename));
            this.workLogItems = workLogItems ?? throw new ArgumentNullException(nameof(workLogItems));
        }

        public void UpdateDocument(DateTime? date = null, bool removeWorkLogItems = false, bool removeBreakMarks = false)
        {
            string text = File.ReadAllText(filename);
            string[] originalLines = text.Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            bool dateTimeIfWorkItemsFound = false;
            List<string> newLines = new List<string>();

            foreach (var line in originalLines)
            {
                // Search date (format: '-- 2021-05-05 --')
                if (date.HasValue && line.StartsWith("--") && line.EndsWith("--") && !dateTimeIfWorkItemsFound)
                {
                    dateTimeIfWorkItemsFound = true;
                    newLines.Add($"-- {date.Value.ToShortDateString()} --");
                    continue;
                }

                // Remove break marks, when option is set
                if (removeBreakMarks && line.Trim().StartsWith("-- PAUSE --"))
                {
                    continue;
                }

                // Search worklog item
                if (!line.StartsWith("["))
                {
                    newLines.Add(line);
                    continue;
                }

                if (removeWorkLogItems)
                {
                    continue;
                }

                var currentWorkLogItem = workLogItems.FirstOrDefault(x => x.RawTextLine == line);

                if (currentWorkLogItem == null)
                {
                    // TODO: Warning, this should not happen
                    newLines.Add(line);
                    continue;
                }

                string modifiedLine = GetModifiedLineWithBookingState(currentWorkLogItem, line);
                newLines.Add(modifiedLine);
            }

            File.WriteAllLines(filename, newLines);
        }

        private string GetModifiedLineWithBookingState(WorkLogItem workLogItem, string line)
        {
            if (!line.StartsWith("["))
            {
                return line;
            }

            string trimmedLine = line[3..];
            string bookingMark = workLogItem.IsBooked ? "x" : " ";
            string bookingText = $"[{bookingMark}]";

            return bookingText + trimmedLine;
        }

        private readonly string filename;
        private readonly IEnumerable<WorkLogItem> workLogItems;
    }
}
