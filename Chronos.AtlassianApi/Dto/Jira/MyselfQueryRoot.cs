using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class MyselfQueryRoot
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonProperty("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("active")]
        public bool Active { get; set; }

        [JsonProperty("timeZone")]
        public string TimeZone { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("groups")]
        public Groups Groups { get; set; }

        [JsonProperty("applicationRoles")]
        public ApplicationRoles ApplicationRoles { get; set; }

        [JsonProperty("expand")]
        public string Expand { get; set; }
    }


}
