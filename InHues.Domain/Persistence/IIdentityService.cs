using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.DTO.V1.Identity.Response;
using InHues.Domain.BaseModels;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace InHues.Domain.Persistence
{
    public interface IIdentityService
    {
        Task<string> GetUserNameAsync(string userId);

        Task<bool> IsInRoleAsync(string userId, string role);
        Task<bool> AddRoleToUserAsync(string userId, string role);
        Task<IList<string>> GetUserRolesAsync(string userId);
        Task<bool> UpdateRoleEnable(string userId, bool isEnabled);
        Task<Result> ResetPassword(string email);
        Task<Result> ResetPasswordRequest(ResetPasswordRequest payload);

        Task<bool> AuthorizeAsync(string userId, string policyName);

        Task<(Result, AuthSuccessResponse)> CreateUserAsync(RegisterIdentityRequest identity, List<string> roles);
        Task<(Result,AuthSuccessResponse)> LoginUserAsync(LoginIdentityRequest login);
        Task<(Result,AuthSuccessResponse)> RefreshTokenAsync(RefreshTokenRequest request);
        IQueryable<UserResponseDto> GetUsers();
        Task<UserResponseDto> GetById(string userId);
        Task<Result> Update(UserRequestDto request);
    }
}
