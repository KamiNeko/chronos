using System.Text;
using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class IssueRoot
    {
        [JsonPropertyName("expand")]
        public string Expand { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("fields")]
        public Fields Fields { get; set; }
    }
}
