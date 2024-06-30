using InHues.Application.Common.Extensions;
using InHues.Application.Common.Interfaces;
using InHues.Application.Common.Options;
using InHues.Domain.BaseModels;
using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.DTO.V1.Identity.Response;
using InHues.Domain.Entities;
using InHues.Domain.Persistence;
using Google.Apis.Drive.v3.Data;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace InHues.Infrastructure.Identity
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _userClaimsPrincipalFactory;
        private readonly IAuthorizationService _authorizationService;
        private readonly JwtOptions _jwtOptions;
        private readonly IMainContext _mainContext;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public IdentityService(
            JwtOptions jwtOptions,
            IMainContext mainContext,
            TokenValidationParameters tokenValidationParameters,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserClaimsPrincipalFactory<ApplicationUser> userClaimsPrincipalFactory,
            IAuthorizationService authorizationService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mainContext = mainContext;
            _userClaimsPrincipalFactory = userClaimsPrincipalFactory;
            _authorizationService = authorizationService;
            _jwtOptions = jwtOptions;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<string> GetUserNameAsync(string userId)
        {
            var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

            return user.UserName;
        }

        public async Task<(Result, AuthSuccessResponse)> CreateUserAsync(RegisterIdentityRequest identity, List<string> roles)
        {
            try
            {
                if (string.IsNullOrEmpty(identity.UserName)) {
                    IEnumerable<string> errors = new List<string>() { "Username is missing!" };
                    return (Result.Failure(errors), null);
                }

                var isUserNameTaken = await _userManager.FindByNameAsync(identity.UserName);
                if (isUserNameTaken != null)
                {
                    IEnumerable<string> errors = new List<string>() { "Username is already taken!" };
                    return (Result.Failure(errors), null);
                }

                var user = identity.Adapt<ApplicationUser>();
                user.InitialPassword = identity.Password;
                user.Email = string.Empty;
                var userRequest = await _userManager.CreateAsync(user, identity.Password);
                if (!userRequest.Succeeded) return (Result.Failure(userRequest.Errors.Select(x => x.Description)), null);

                if (roles is not null && roles.Any())
                {
                    var validateRoles = ValidateRoles(roles);
                    if (!validateRoles.Succeeded) return (validateRoles, null);

                    await _userManager.AddToRolesAsync(user, roles);
                }

                return (userRequest.ToApplicationResult(), await GetTokenAsync(user));
            }
            catch (Exception e) {
                return (Result.Failure(new List<string> { e.Message }), null);
            }
        }

        public async Task<bool> IsInRoleAsync(string userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            return await _userManager.IsInRoleAsync(user, role);
        }
        public async Task<IList<string>> GetUserRolesAsync(string userId) {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
            return await _userManager.GetRolesAsync(user);
        }
        public async Task<bool> AddRoleToUserAsync(string userId, string role)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            //remove previous roles - since its one to one relationship atm
            var roles = await _userManager.GetRolesAsync(user);
            if (roles.Any())
            {
                await _userManager.RemoveFromRolesAsync(user, roles.ToArray());
            }

            //Add the new selected role
            var query = await _userManager.AddToRolesAsync(user, new[] { role });
            return query.Succeeded;
        }
        public async Task<bool> UpdateRoleEnable(string userId, bool isEnabled) {
            try {
                var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);
                user.IsEnabled = isEnabled;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded;
            }
            catch {
                return false;
            }
        }
        public async Task<bool> AuthorizeAsync(string userId, string policyName)
        {
            var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

            var principal = await _userClaimsPrincipalFactory.CreateAsync(user);

            var result = await _authorizationService.AuthorizeAsync(principal, policyName);

            return result.Succeeded;
        }

        public async Task<Result> ResetPassword(string email) {
            
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result.Failure(new[] { "User does not exist!" });

            DateTimeOffset now = DateTimeOffset.Now;
            //DateTimeOffset today = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset);

            if (user.PasswordResetToken is not null && user.PasswordResetExpire > now) {
                return Result.Success(user.PasswordResetToken);
            }

            user.PasswordResetToken = Guid.NewGuid().ToString();
            user.PasswordResetExpire = now.AddHours(6);
            await _userManager.UpdateAsync(user);
            return Result.Success(user.PasswordResetToken);
        }
        public async Task<Result> ResetPasswordRequest(ResetPasswordRequest payload) {

            var user = await _userManager.FindByEmailAsync(payload.Email);
            if (user is null) return Result.Failure(new[] { "User does not exist!" });

            DateTimeOffset now = DateTimeOffset.Now;
            //DateTimeOffset today = new DateTimeOffset(now.Year, now.Month, now.Day, 0, 0, 0, now.Offset);

            if(user.PasswordResetToken is null) return Result.Failure(new[] { "User reset password request missing!" });

            if (user.PasswordResetToken is not null && user.PasswordResetExpire < now)
            {
                user.PasswordResetToken = null;
                user.PasswordResetExpire = null;
                await _userManager.UpdateAsync(user);
                return Result.Failure(new[] { "Reset key expired!" });
            }

            if(user.PasswordResetToken != payload.ResetKey) Result.Failure(new[] { "Invalid reset key!" });
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var update = await _userManager.ResetPasswordAsync(user, token, payload.NewPassword);
            return update.ToApplicationResult();
        }
        public async Task<(Result, AuthSuccessResponse)> LoginUserAsync(LoginIdentityRequest login)
        {
            ApplicationUser user = null;
            user = await _userManager.FindByEmailAsync(login.Email);
            if (user is null)
            {
                user = await _userManager.FindByNameAsync(login.Email);
                if (user is null)
                {
                    IEnumerable<string> errors = new List<string>() { "Email / Username not found!" };
                    return (Result.Failure(errors), null);
                }
            }

            var validateUserPassword = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!validateUserPassword)
            {
                IEnumerable<string> errors = new List<string>() { "Email or password is incorrect!" };
                return (Result.Failure(errors), null);
            }

            return (Result.Success(), await GetTokenAsync(user));
        }
        public async Task<(Result, AuthSuccessResponse)> RefreshTokenAsync(RefreshTokenRequest request)
        {
            var validatedToken = GetPrincipalFromToken(request.Token);
            if (validatedToken == null) return (Result.Failure(new List<string>() { "Invalid Token!"}), null);

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var dbRefreshToken = await _mainContext.RefreshTokens.SingleOrDefaultAsync(x => x.Token == request.RefreshToken);

            if(dbRefreshToken is null) return (Result.Failure(new List<string>() { "Refresh Token does not exist!" }), null);
            if(DateTime.UtcNow > dbRefreshToken.Expirydate) return (Result.Failure(new List<string>() { "Refresh Token already expired!" }), null);
            if(dbRefreshToken.IsInvalidated) return (Result.Failure(new List<string>() { "Refresh Token is invalid!" }), null);
            if(dbRefreshToken.IsUsed) return (Result.Failure(new List<string>() { "Refresh Token has been used!" }), null);
            if(dbRefreshToken.JwtId != jti) return (Result.Failure(new List<string>() { "Refresh Token has invalid jti!" }), null);

            dbRefreshToken.IsUsed = true;
            _mainContext.RefreshTokens.Update(dbRefreshToken);
            await _mainContext.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(validatedToken.Claims.Single(x => x.Type == "id").Value);
            return (Result.Success(), await GetTokenAsync(user));
        }
        #region Helpers
        private Result ValidateRoles(List<string> roles) {
            var errors = new List<string>();
            if (roles is null || !roles.Any())
            {
                errors.Add("Roles not found!");
                return Result.Failure(errors);
            }

            roles.ForEach(role =>
            {
                if (_roleManager.Roles.All(r => r.Name != role))
                {
                    errors.Add($"Role: [{role}] is not listed on our database. Please check input role name");
                }
            });

            if (errors.Any()) return Result.Failure(errors);

            return Result.Success();
        }
        private async Task<AuthSuccessResponse> GetTokenAsync(ApplicationUser user) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);
            var claims = new List<Claim> {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//will set id for jwt 
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id),
                    new Claim("isEnabled", $"{user.IsEnabled}"),
                    new Claim("email", $"{user.Email}")

            };
            var userRoles = await _userManager.GetRolesAsync(user);
            claims.AddRange(userRoles.Select(role=>new Claim(ClaimTypes.Role,role)));

            var tokenDesc = new SecurityTokenDescriptor { 
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDesc);
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                CustomerId = user.Id,
                IsEnabled = user.IsEnabled,
                Expirydate = DateTime.UtcNow.AddMonths(1),
                CreatedOn = DateTime.UtcNow,
                
            };
            await _mainContext.RefreshTokens.AddAsync(refreshToken);
            await _mainContext.SaveChangesAsync();

            var tokenString = tokenHandler.WriteToken(token);
            return new AuthSuccessResponse() { Token = tokenString, RefreshToken = refreshToken.Token };
        }
        private ClaimsPrincipal GetPrincipalFromToken(string token) {
            var tokenHandler = new JwtSecurityTokenHandler();
            try {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken)) return null;
                return principal;
            }
            catch { return null; }
        }
        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken) {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase);
        }
        #endregion

        #region REST METHODS
        public IQueryable<UserResponseDto> GetUsers()
        {
            var users = _userManager.Users.CustomProjectToType<ApplicationUser, UserResponseDto>();
            return users is not null ? users : null;
        }
        public async Task<UserResponseDto> GetById(string userId)
        {
            var user =  await _userManager.FindByIdAsync(userId);
            if (user is null) return null;
            return user.Adapt<UserResponseDto>();
        }
        public async Task<Result> Update(UserRequestDto request) {
            var user = await _userManager.FindByIdAsync(request.Id);
            user.Email = request.Email;
            user.UserName = request.Username;
            user.IsDeleted = request.IsDeleted;
            var result = await _userManager.UpdateAsync(user);
            return result.ToApplicationResult();
        }
        #endregion
    }
}
