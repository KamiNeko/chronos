using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class WorkLogItemCreate
    {
        [JsonProperty("issueKey")]
        public string IssueKey { get; set; }

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

        [JsonProperty("authorAccountId")]
        public string AuthorAccountId { get; set; }

        [JsonProperty("remainingEstimateSeconds")]
        public int RemainingEstimateSeconds { get; set; }

        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
    }
}
