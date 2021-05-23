using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Category
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("type")]
        public Type Type { get; set; }
    }


}
