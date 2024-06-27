using InHues.Domain.BaseModels;
using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InHues.Application.Services.Identity.Commands
{
    public class UpdateUser : UserRequestDto, IRequest<Result>
    {
    }
    public class UpdateUserHandler : IRequestHandler<UpdateUser, Result>
    {
        private readonly IIdentityService _identityService;
        public UpdateUserHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<Result> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            return await _identityService.Update(request);
        }
    }
}
