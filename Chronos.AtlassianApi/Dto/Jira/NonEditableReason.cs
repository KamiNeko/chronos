using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class NonEditableReason
    {
        [JsonPropertyName("reason")]
        public string Reason { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
