using System;
using System.Text.Json.Serialization;

namespace InHues.Domain.DTO.V1.Identity.Response
{
    public class UserResponseDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("normalizedUserName")]
        public string NormalizedUserName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("normalizedEmail")]
        public string NormalizedEmail { get; set; }
        [JsonPropertyName("emailConfirmed")]
        public bool EmailConfirmed { get; set; }
        [JsonPropertyName("twoFactorEnabled")]
        public bool TwoFactorEnabled { get; set; }
        [JsonPropertyName("phoneNumberConfirmed")]
        public bool PhoneNumberConfirmed { get; set; }
        [JsonPropertyName("phoneNumber")]
        public string PhoneNumber { get; set; }
        [JsonPropertyName("concurrencyStamp")]
        public string ConcurrencyStamp { get; set; }
        [JsonPropertyName("securityStamp")]
        public string SecurityStamp { get; set; }
        [JsonPropertyName("lockoutEnabled")]
        public bool LockoutEnabled { get; set; }
        [JsonPropertyName("lockoutEnd")]
        public DateTimeOffset LockoutEnd { get; set; }
        [JsonPropertyName("accessFailedCount")]
        public int AccessFailedCount { get; set; }
        [JsonPropertyName("isEnabled")]
        public bool IsEnabled { get; set; }
        [JsonPropertyName("initialPassword")]
        public string InitialPassword { get; set; }
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
