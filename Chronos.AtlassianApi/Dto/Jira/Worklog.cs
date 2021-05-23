using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Worklog
    {
        [JsonPropertyName("startAt")]
        public int StartAt { get; set; }

        [JsonPropertyName("maxResults")]
        public int MaxResults { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("worklogs")]
        public List<Worklog> Worklogs { get; set; }
    }
}
