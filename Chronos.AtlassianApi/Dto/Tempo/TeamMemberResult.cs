using Newtonsoft.Json;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class TeamMemberResult : Result
    {
        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("member")]
        public Member Member { get; set; }

        [JsonProperty("memberships")]
        public Memberships Memberships { get; set; }
    }
}
