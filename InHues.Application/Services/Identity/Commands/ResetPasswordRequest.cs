using InHues.Domain.BaseModels;
using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InHues.Application.Services.Identity.Commands
{
    public class ResetPasswordAction : ResetPasswordRequest, IRequest<Result>
    {
    }
    public class ResetPasswordRequestHandler : IRequestHandler<ResetPasswordAction, Result>
    {
        readonly IIdentityService _identityService;

        public ResetPasswordRequestHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(ResetPasswordAction request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(request.Email)) return Result.Failure(new[] { "Missing email." });

            var reset = await _identityService.ResetPasswordRequest(request);
            if (!reset.Succeeded) return reset;

            //send email
            return reset;
        }
    }
}
