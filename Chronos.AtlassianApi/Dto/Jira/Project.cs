using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class Project
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("projectTypeKey")]
        public string ProjectTypeKey { get; set; }

        [JsonPropertyName("simplified")]
        public bool Simplified { get; set; }

        [JsonPropertyName("avatarUrls")]
        public AvatarUrls AvatarUrls { get; set; }

        [JsonPropertyName("projectCategory")]
        public ProjectCategory ProjectCategory { get; set; }
    }
}
