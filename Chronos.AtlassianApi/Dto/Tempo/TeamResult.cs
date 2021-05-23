using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class TeamResult : Result
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("lead")]
        public Lead Lead { get; set; }

        [JsonProperty("program")]
        public object Program { get; set; }

        [JsonProperty("links")]
        public Links Links { get; set; }

        [JsonProperty("members")]
        public Members Members { get; set; }

        [JsonProperty("permissions")]
        public Permissions Permissions { get; set; }
    }
}
