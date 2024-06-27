namespace InHues.Domain.DTO.V1.Identity.Request
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
        public string ResetKey { get; set; }
    }
}
