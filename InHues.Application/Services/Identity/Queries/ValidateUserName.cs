using InHues.Domain.Persistence;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace InHues.Application.Services.Identity.Queries
{
    public class ValidateUserName : IRequest<string?>
    {
        public string UserName { get; set; }
    }
    public class ValidateUserNameHandler : IRequestHandler<ValidateUserName, string?>
    {
        IIdentityService _identityService;

        public ValidateUserNameHandler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task<string?> Handle(ValidateUserName request, CancellationToken cancellationToken)
        {
            var user = _identityService.GetUsers().FirstOrDefault(x => x.UserName == request.UserName);
            return user?.Id;
        }
    }
}
