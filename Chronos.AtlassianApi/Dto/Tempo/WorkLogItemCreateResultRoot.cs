using Newtonsoft.Json;
using System;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class WorkLogItemCreateResultRoot : Root
    {
        [JsonProperty("tempoWorklogId")]
        public int TempoWorklogId { get; set; }

        [JsonProperty("jiraWorklogId")]
        public int JiraWorklogId { get; set; }

        [JsonProperty("issue")]
        public Issue Issue { get; set; }

        [JsonProperty("timeSpentSeconds")]
        public int TimeSpentSeconds { get; set; }

        [JsonProperty("billableSeconds")]
        public int BillableSeconds { get; set; }

        [JsonProperty("startDate")]
        public string StartDate { get; set; }

        [JsonProperty("startTime")]
        public string StartTime { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("createdAt")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime UpdatedAt { get; set; }

        [JsonProperty("author")]
        public Author Author { get; set; }

        [JsonProperty("attributes")]
        public Attributes Attributes { get; set; }
    }
}
