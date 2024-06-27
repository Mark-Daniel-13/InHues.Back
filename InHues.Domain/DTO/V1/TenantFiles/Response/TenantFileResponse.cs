using System;
using System.Text.Json.Serialization;

namespace InHues.Domain.DTO.V1.TenantFile.Response
{
    public class TenantFileResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("url")]
        public string URL { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("extension")]
        public string Extension { get; set; }

        [JsonPropertyName("categoryId")]
        public int CategoryId { get; set; }
        
        [JsonPropertyName("statusId")]
        public int StatusId { get; set; }

        [JsonPropertyName("gDriveId")]
        public string GDriveId { get; set; }
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
