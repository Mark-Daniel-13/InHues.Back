using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.DTO.V1.Identity.Response;
using InHues.Domain.BaseModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using InHues.Domain.Persistence;

namespace InHues.Application.Services.Identity.Queries
{
    public class RefreshToken : RefreshTokenRequest, IRequest<(Result, AuthSuccessResponse)>
    {
    }
    public class RefreshTokenHandler : IRequestHandler<RefreshToken, (Result, AuthSuccessResponse)>
    {
        private readonly IIdentityService _identity;

        public RefreshTokenHandler(IIdentityService identity)
        {
            _identity = identity;
        }

        public async Task<(Result, AuthSuccessResponse)> Handle(RefreshToken request, CancellationToken cancellationToken)
        {
            var result = await _identity.RefreshTokenAsync(request);
            return result;
        }
    }
}
