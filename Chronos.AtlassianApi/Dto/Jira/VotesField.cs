using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class VotesField
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("votes")]
        public int Votes { get; set; }

        [JsonPropertyName("hasVoted")]
        public bool HasVoted { get; set; }
    }
}
