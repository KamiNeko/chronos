using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class OuterContent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("content")]
        public List<InnerContent> Content { get; set; }
    }
}
