using System.Security.Claims;

namespace InHues.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string UserId => _httpContextAccessor.HttpContext?.User?.FindFirstValue("id");
        public string Email => _httpContextAccessor.HttpContext?.User?.FindFirstValue("email");
        public bool IsEnabled => _httpContextAccessor.HttpContext?.User is null ? false : Convert.ToBoolean(_httpContextAccessor.HttpContext.User.FindFirstValue("isEnabled"));
        public Guid TenantId => string.IsNullOrEmpty(_httpContextAccessor.HttpContext?.User?.FindFirstValue("tenantId")) ? Guid.Empty: new Guid(_httpContextAccessor.HttpContext?.User?.FindFirstValue("tenantId"));
        public string Roles => string.Join(",", _httpContextAccessor.HttpContext?.User?.Claims.Where(x => x.Type == ClaimTypes.Role).Select(y => y.Value));
    }
}
