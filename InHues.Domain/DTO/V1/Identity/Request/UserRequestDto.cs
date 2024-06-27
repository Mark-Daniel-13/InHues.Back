using System;

namespace InHues.Domain.DTO.V1.Identity.Request
{
    public class UserRequestDto
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsDeleted { get; set; }
    }
}
