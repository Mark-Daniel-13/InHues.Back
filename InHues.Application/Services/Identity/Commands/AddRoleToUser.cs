using InHues.Domain.BaseModels;
using InHues.Domain.Persistence;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace InHues.Application.Services.Identity.Commands
{
    public class AddRoleToUser : IRequest<Result>
    {
        public string UserId { get; set; }
        public string Role { get; set; }
    }
    public class AddRoleToUserHandler : IRequestHandler<AddRoleToUser, Result>
    {
        readonly IIdentityService _identityService;

        public AddRoleToUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(AddRoleToUser request, CancellationToken cancellationToken)
        {
            try {
                if (string.IsNullOrEmpty(request.UserId) || string.IsNullOrEmpty(request.Role)) return Result.Failure(new [] { "UserId or Role is missing"});

                var checkRoles = await _identityService.IsInRoleAsync(request.UserId, request.Role);
                if (checkRoles) return Result.Failure(new[] { "User already have this role." });

                return Result.Success(await _identityService.AddRoleToUserAsync(request.UserId, request.Role));
            } catch (Exception e) {
                return Result.Failure(new[] { e.Message }); ;
            }
        }
    }
}
