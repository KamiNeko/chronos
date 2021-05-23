using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Contact
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }


}
