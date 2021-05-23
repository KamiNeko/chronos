using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Links
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
