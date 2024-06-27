using InHues.Domain.DTO.V1.Identity.Response;
using InHues.Domain.Persistence;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace InHues.Application.Services.Identity.Queries
{
    public class GetUserById : IRequest<UserResponseDto>
    {
        public string UserId { get; set; }
    }
    public class GetUserByIdHandler : IRequestHandler<GetUserById, UserResponseDto>
    {
        IIdentityService _identityService;
        public GetUserByIdHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<UserResponseDto> Handle(GetUserById request, CancellationToken cancellationToken)
        {
            return await _identityService.GetById(request.UserId);
        }
    }
}
