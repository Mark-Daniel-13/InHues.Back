using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace InHues.Domain.BaseModels
{
    public abstract class AuditableEntity
    {
        [Required]
        [JsonPropertyName("createdOn")]
        public DateTimeOffset CreatedOn { get; set; }
        [Required]
        [JsonPropertyName("createdBy")]
        public string CreatedBy { get; set; }

        [JsonPropertyName("lastModifiedOn")]
        public DateTimeOffset? LastModifiedOn { get; set; }
        [JsonPropertyName("lastModifiedBy")]
        public string LastModifiedBy { get; set; }

        [Required]
        [JsonPropertyName("isDeleted")]
        public bool IsDeleted { get; set; }

        [JsonPropertyName("deletedOn")]
        public DateTimeOffset? DeletedOn { get; set; }
        [JsonPropertyName("deletedBy")]
        public string DeletedBy { get; set; }

        [JsonPropertyName("customerId")]
        public string CustomerId { get; set; }
    }
}
