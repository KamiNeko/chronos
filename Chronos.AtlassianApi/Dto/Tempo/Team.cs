using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Team
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
