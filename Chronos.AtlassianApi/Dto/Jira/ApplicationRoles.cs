using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class ApplicationRoles
    {
        [JsonProperty("size")]
        public int Size { get; set; }

        [JsonProperty("items")]
        public List<object> Items { get; set; }
    }


}
