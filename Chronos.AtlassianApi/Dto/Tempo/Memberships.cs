using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Memberships
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("active")]
        public Active Active { get; set; }
    }
}
