using System;

namespace Chronos.WorkLogs.Analysis
{
    public class WorkLogItemsStatistics
    {
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public TimeSpan WorkDuration { get; set; }
        public TimeSpan BreakDuration { get; set; }

        public void Print()
        {
            Console.WriteLine($"Start time:\t{StartTime.ToString(@"hh\:mm")}");
            Console.WriteLine($"End time:\t{EndTime.ToString(@"hh\:mm")}");
            Console.WriteLine($"Work duration:\t{WorkDuration.ToString(@"hh\:mm")}");
            Console.WriteLine($"Break duration:\t{BreakDuration.ToString(@"hh\:mm")}");
        }
    }
}
