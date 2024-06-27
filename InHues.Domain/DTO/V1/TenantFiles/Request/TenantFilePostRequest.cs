using System;
using System.Text.Json.Serialization;

namespace InHues.Domain.DTO.V1.TenantFile.Request
{
    public class TenantFilePostRequest
    {
        public Guid TenantId { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public string GDriveId { get; set; }
    }
}
