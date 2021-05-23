using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Active
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("commitmentPercent")]
        public int CommitmentPercent { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public object To { get; set; }

        [JsonProperty("role")]
        public Role Role { get; set; }
    }
}
