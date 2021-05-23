using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class ProgressField
    {
        [JsonPropertyName("progress")]
        public int Progress { get; set; }

        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("percent")]
        public int Percent { get; set; }
    }
}
