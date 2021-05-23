using Newtonsoft.Json;
using System.Collections.Generic;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class WorkLogsRoot : Root
    {
        [JsonProperty("results")]
        public List<WorkLogResult> Results { get; set; }
    }
}
