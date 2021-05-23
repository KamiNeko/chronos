using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Permissions
    {
        [JsonProperty("self")]
        public string Self { get; set; }
    }
}
