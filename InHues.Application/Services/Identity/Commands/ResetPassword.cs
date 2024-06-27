using InHues.Application.Common.Interfaces;
using InHues.Domain.BaseModels;
using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InHues.Application.Services.Identity.Commands
{
    public class ResetPassword : PasswordResetDto, IRequest<Result>
    {
    }
    public class ResetPasswordHandler : IRequestHandler<ResetPassword, Result>
    {
        readonly IIdentityService  _identityService;

        public ResetPasswordHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(ResetPassword request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email)) return Result.Failure(new[] { "Missing email."});

            var reset = await _identityService.ResetPassword(request.Email);
            if (!reset.Succeeded) return reset;

            //send email
            return reset;
        }
    }
}
