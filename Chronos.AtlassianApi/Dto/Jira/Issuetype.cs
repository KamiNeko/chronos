﻿using System.Text.Json.Serialization;

namespace Chronos.AtlassianApi.Dto.Jira
{
    // Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);
    public class Issuetype
    {
        [JsonPropertyName("self")]
        public string Self { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("iconUrl")]
        public string IconUrl { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("subtask")]
        public bool Subtask { get; set; }

        [JsonPropertyName("avatarId")]
        public int AvatarId { get; set; }
    }
}
