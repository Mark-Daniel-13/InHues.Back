using InHues.Domain.DTO.V1.Identity.Response;
using InHues.Domain.Persistence;
using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InHues.Application.Services.Identity.Queries
{
    public class GetUsers : IRequest<IQueryable<UserResponseDto>>
    {

    }
    public class GetUsersHandler : IRequestHandler<GetUsers, IQueryable<UserResponseDto>>
    {
        private readonly IIdentityService _identityService;
        public GetUsersHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public Task<IQueryable<UserResponseDto>> Handle(GetUsers request, CancellationToken cancellationToken)
        {
            return Task.Run(() => { 
                var userList = _identityService.GetUsers().Where(x=> x.UserName != "admin");
                if (userList is null || !userList.Any()) return null;

                return userList;
            });
        }
    }
}
