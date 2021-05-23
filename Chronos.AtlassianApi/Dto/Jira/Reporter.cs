using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Reporter
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("accountId")]
        public string AccountId { get; set; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonPropertyName("displayName")]
        public string DisplayName { get; set; }

        [JsonPropertyName("active")]
        public bool Active { get; set; }

        [JsonPropertyName("timeZone")]
        public string TimeZone { get; set; }

        [JsonPropertyName("accountType")]
        public string AccountType { get; set; }
    }
}
