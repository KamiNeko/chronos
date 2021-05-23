using System;
using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Worklog2
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("author")]
        public Author Author { get; set; }

        [JsonPropertyName("updateAuthor")]
        public UpdateAuthor UpdateAuthor { get; set; }

        [JsonPropertyName("comment")]
        public Comment Comment { get; set; }

        [JsonPropertyName("created")]
        public DateTime Created { get; set; }

        [JsonPropertyName("updated")]
        public DateTime Updated { get; set; }

        [JsonPropertyName("started")]
        public DateTime Started { get; set; }

        [JsonPropertyName("timeSpent")]
        public string TimeSpent { get; set; }

        [JsonPropertyName("timeSpentSeconds")]
        public int TimeSpentSeconds { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("issueId")]
        public string IssueId { get; set; }
    }
}
