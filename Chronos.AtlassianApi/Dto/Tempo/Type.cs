using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Type
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }


}
