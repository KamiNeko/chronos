using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Tempo
{
    public class Result
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("key")]
        public string Key { get; set; }

        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("global")]
        public bool Global { get; set; }

        [JsonPropertyName("lead")]
        public Lead Lead { get; set; }

        [JsonPropertyName("category")]
        public Category Category { get; set; }

        [JsonPropertyName("links")]
        public Links Links { get; set; }

        [JsonPropertyName("contact")]
        public Contact Contact { get; set; }

        [JsonPropertyName("customer")]
        public Customer Customer { get; set; }

        [JsonPropertyName("monthlyBudget")]
        public int? MonthlyBudget { get; set; }
    }


}
