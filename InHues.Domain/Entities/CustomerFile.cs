using System;

namespace InHues.Domain.Entities
{
    public class CustomerFile
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public string Name { get; set; }
        public string Extension { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public string GDriveId { get; set; }
        public Guid UserId { get; set; }
    }
}
