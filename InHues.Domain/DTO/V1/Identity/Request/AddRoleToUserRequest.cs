namespace InHues.Domain.DTO.V1.Identity.Request
{
    public class AddRoleToUserRequest
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
}
