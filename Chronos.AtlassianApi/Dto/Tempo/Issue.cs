using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Issue
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("key")]
        public string Key { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
