using System.Text.Json.Serialization;

namespace InHues.Domain.DTO.V1.Identity.Response
{
    public class AuthSuccessResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; set; }
        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}
