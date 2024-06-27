using System;
using System.Text.Json.Serialization;

namespace InHues.Domain.DTO.V1.Identity.Response
{
    public class IdentityRoleDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("normalizedName")]
        public string NormalizedName { get; set; }

        [JsonPropertyName("concurrencyStamp")]
        public Guid ConcurrencyStamp { get; set; }
    }
}
