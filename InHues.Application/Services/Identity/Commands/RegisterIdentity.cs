using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.BaseModels;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using InHues.Domain.Persistence;
using InHues.Domain.DTO.V1.Identity.Response;

namespace InHues.Application.Services.Identity.Commands
{
    public class RegisterIdentity:RegisterIdentityRequest, IRequest<(Result, AuthSuccessResponse)>
    {
        //public TModel test { get; set; }
    }
    public class RegisterHandler : IRequestHandler<RegisterIdentity, (Result, AuthSuccessResponse)>
    {
        private readonly IIdentityService _identity;

        public RegisterHandler(IIdentityService identity)
        {
            _identity = identity;
        }

        public async Task<(Result, AuthSuccessResponse)> Handle(RegisterIdentity request, CancellationToken cancellationToken)
        {
            var result = await _identity.CreateUserAsync(request,request.Roles);
            return result;
        }
    }
}
