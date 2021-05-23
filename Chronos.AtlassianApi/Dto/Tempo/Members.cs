using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Members
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
