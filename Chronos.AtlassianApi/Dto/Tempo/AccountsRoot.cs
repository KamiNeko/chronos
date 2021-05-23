using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class AccountsRoot
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("results")]
        public List<Result> Results { get; set; }
    }
}
