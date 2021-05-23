using System;
using System.Collections.Generic;
using Chronos.WorkLogs.Analysis;
using Chronos.WorkLogs.Parsing;

namespace Chronos.WorkLogs.Documents
{
    public class Document
    {
        public string Filename { get; set; }
        public DateTime Date { get; set; }
        public IEnumerable<WorkLogItem> WorkLogItems { get; set; }
        public WorkLogItemAnalysisResult WorkLogItemAnalysis { get; set; }
    }
}
