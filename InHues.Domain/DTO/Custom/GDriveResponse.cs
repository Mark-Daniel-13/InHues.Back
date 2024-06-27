
using System.Text.Json.Serialization;

namespace InHues.Domain.DTO.Custom
{
    public class GDriveResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }
        [JsonPropertyName("webViewLink")]
        public string Url { get; set; }
    }
}
