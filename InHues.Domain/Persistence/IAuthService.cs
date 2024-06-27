using InHues.Domain.DTO.V1.Identity.Request;
using System.Threading.Tasks;

namespace InHues.Domain.Persistence
{
    public interface IAuthService
    {
        public Task LogoutAsync();
        public Task<bool> LoginAsync(string username,string password);
        public Task<bool> RegisterAsync(RegisterIdentityRequest payload);
    }
}
