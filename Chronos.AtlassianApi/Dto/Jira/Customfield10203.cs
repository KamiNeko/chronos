using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Customfield10203
    {
        [JsonPropertyName("hasEpicLinkFieldDependency")]
        public bool HasEpicLinkFieldDependency { get; set; }

        [JsonPropertyName("showField")]
        public bool ShowField { get; set; }

        [JsonPropertyName("nonEditableReason")]
        public NonEditableReason NonEditableReason { get; set; }
    }
}
