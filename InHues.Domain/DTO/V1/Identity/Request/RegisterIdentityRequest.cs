using System.Collections.Generic;

namespace InHues.Domain.DTO.V1.Identity.Request
{
    public class RegisterIdentityRequest
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public List<string> Roles { get; set; }

    }
}
