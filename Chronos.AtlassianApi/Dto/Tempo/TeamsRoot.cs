using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class TeamsRoot : Root
    {
        [JsonProperty("results")]
        public List<TeamResult> Results { get; set; }
    }
}
