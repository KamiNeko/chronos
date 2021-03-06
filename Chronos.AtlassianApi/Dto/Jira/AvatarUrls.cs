using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class AvatarUrls
    {
        [JsonPropertyName("48x48")]
        public string _48x48 { get; set; }

        [JsonPropertyName("24x24")]
        public string _24x24 { get; set; }

        [JsonPropertyName("16x16")]
        public string _16x16 { get; set; }

        [JsonPropertyName("32x32")]
        public string _32x32 { get; set; }
    }
}
