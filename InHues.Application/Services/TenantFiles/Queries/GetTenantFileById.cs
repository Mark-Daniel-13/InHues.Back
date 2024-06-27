using InHues.Application.Common.Interfaces;
using InHues.Domain.Persistence;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using InHues.Application.Common.Extensions;
using InHues.Domain.DTO.V1.TenantFile.Response;

namespace InHues.Application.Services.TenantFile.Queries
{
    public class GetTenantFileById : IRequest<TenantFileResponse>
    {
        public int Id { get; set; }
    }
    public class GetTenantFileByIdHandler : IRequestHandler<GetTenantFileById, TenantFileResponse>
    {
        private readonly IMainContext _mainContext;
        readonly ICurrentUserService _currentUserService;

        public GetTenantFileByIdHandler(IMainContext mainContext, ICurrentUserService currentUserService)
        {
            _mainContext = mainContext;
            _currentUserService = currentUserService;
        }

        public async Task<TenantFileResponse> Handle(GetTenantFileById request, CancellationToken cancellationToken)
        {
            if (!_mainContext.CustomerFiles.Any()) return null;
            var data = await _mainContext.CustomerFiles.FindAsync(request.Id);
            if (data.UserId != new System.Guid(_currentUserService.UserId) ) return null;
            return data.CustomAdapt<TenantFileResponse>();
        }
    }
}
