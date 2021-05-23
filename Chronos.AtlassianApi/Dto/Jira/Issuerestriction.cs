using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Issuerestriction
    {
        [JsonPropertyName("issuerestrictions")]
        public Issuerestrictions Issuerestrictions { get; set; }

        [JsonPropertyName("shouldDisplay")]
        public bool ShouldDisplay { get; set; }
    }
}
