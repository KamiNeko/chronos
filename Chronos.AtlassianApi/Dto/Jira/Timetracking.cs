using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Timetracking
    {
        [JsonPropertyName("timeSpent")]
        public string TimeSpent { get; set; }

        [JsonPropertyName("timeSpentSeconds")]
        public int TimeSpentSeconds { get; set; }
    }
}
