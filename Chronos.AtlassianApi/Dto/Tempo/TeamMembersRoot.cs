using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class TeamMembersRoot : Root
    {
        [JsonProperty("results")]
        public List<TeamMemberResult> Results { get; set; }
    }
}
