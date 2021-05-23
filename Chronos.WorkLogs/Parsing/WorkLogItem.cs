using System;

namespace Chronos.WorkLogs.Parsing
{
    public class WorkLogItem
    {
        public string ProjectName { get; set; }
        public string Comment { get; set; }
        public string JiraKey { get; set; }
        public string JiraLink { get; set; }
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; }
        public string RawTextLine { get; set; }

        public void Print()
        {
            var isBooked = IsBooked ? "X" : " ";
            var jiraKeyText = string.IsNullOrEmpty(JiraKey) ? "N/A" : JiraKey;
            Console.WriteLine($"[{isBooked}] {Date.ToShortDateString()} | {StartTime.ToString(@"hh\:mm")} - {EndTime.ToString(@"hh\:mm")} | {ProjectName}: {Comment} ({jiraKeyText})");
        }
    }
}
