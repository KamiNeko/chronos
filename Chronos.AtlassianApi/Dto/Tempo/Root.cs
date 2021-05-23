using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Root
    {
        [JsonProperty("self")]
        public string Self { get; set; }

        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
    }
}
