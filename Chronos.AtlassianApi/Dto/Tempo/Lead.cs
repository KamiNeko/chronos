using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Lead
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("accountId")]
        public string AccountId { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }
    }
}
