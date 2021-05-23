using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Attributes
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("values")]
        public List<object> Values { get; set; }
    }
}
