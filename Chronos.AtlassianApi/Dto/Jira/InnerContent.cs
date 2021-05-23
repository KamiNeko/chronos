using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    public class InnerContent
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
