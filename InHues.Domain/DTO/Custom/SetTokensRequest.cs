namespace InHues.Domain.DTO.Custom
{
    public class SetTokensRequest
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public string UserId { get; set; }
    }
}
