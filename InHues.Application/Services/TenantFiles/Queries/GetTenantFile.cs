using InHues.Application.Common.Interfaces;
using InHues.Domain.Persistence;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using InHues.Domain.DTO.V1.TenantFile.Response;
using InHues.Application.Common.Extensions;

namespace InHues.Application.Services.TenantFile.Queries
{
    public class GetTenantFile : IRequest<IQueryable<TenantFileResponse>>
    {
    }
    public class GetTenantFileHandler : IRequestHandler<GetTenantFile, IQueryable<TenantFileResponse>>
    {
        private readonly IMainContext _mainDbContext;
        readonly ICurrentUserService _currentUserService;

        public GetTenantFileHandler(IMainContext mainDbContext, ICurrentUserService currentUserService)
        {
            _mainDbContext = mainDbContext;
            _currentUserService = currentUserService;
        }

        public async Task<IQueryable<TenantFileResponse>> Handle(GetTenantFile request, CancellationToken cancellationToken)
        {
            var data = _mainDbContext.CustomerFiles.Where(x => x.UserId == new System.Guid(_currentUserService.UserId));
            var results = data.CustomProjectToType<Domain.Entities.CustomerFile, TenantFileResponse>();
            return results is not null ? results : null;
        }
    }
}
