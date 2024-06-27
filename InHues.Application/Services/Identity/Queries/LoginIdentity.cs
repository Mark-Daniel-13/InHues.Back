using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.DTO.V1.Identity.Response;
using InHues.Domain.BaseModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using InHues.Domain.Persistence;

namespace InHues.Application.Services.Identity.Queries
{
    public class LoginIdentity : LoginIdentityRequest, IRequest<(Result, AuthSuccessResponse)>
    {
    }
    public class LoginHandler : IRequestHandler<LoginIdentity, (Result, AuthSuccessResponse)>
    {
        private readonly IIdentityService _identity;

        public LoginHandler(IIdentityService identity)
        {
            _identity = identity;
        }

        public async Task<(Result, AuthSuccessResponse)> Handle(LoginIdentity request, CancellationToken cancellationToken)
        {
            var result = await _identity.LoginUserAsync(request);
            return result;
        }
    }
}
