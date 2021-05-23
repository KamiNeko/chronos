using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Comment
    {
        [JsonPropertyName("comments")]
        public List<object> Comments { get; set; }

        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("maxResults")]
        public int MaxResults { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("startAt")]
        public int StartAt { get; set; }

        [JsonPropertyName("version")]
        public int Version { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("content")]
        public List<OuterContent> Content { get; set; }
    }
}
