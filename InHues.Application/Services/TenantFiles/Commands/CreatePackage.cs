using InHues.Application.Common.Interfaces;
using InHues.Domain.BaseModels;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Mapster;
using InHues.Application.Common.Extensions;
using InHues.Domain.DTO.V1.TenantFile.Request;
using InHues.Domain.DTO.V1.TenantFile.Response;

namespace InHues.Application.Services.Packages.Commands
{
    public class CreateTenantFile: TenantFilePostRequest, IRequest<Result>
    {
    }
    public class CreateTenantFileHandler : IRequestHandler<CreateTenantFile, Result>
    {
        private readonly IMainContext _mainContext;

        public CreateTenantFileHandler(IMainContext mainContext)
        {
            _mainContext = mainContext;
        }

        public async Task<Result> Handle(CreateTenantFile request, CancellationToken cancellationToken)
        {
            var model = request.Adapt<Domain.Entities.CustomerFile>();
            await _mainContext.CustomerFiles.AddAsync(model);
            var query = await _mainContext.SaveChangesAsync(cancellationToken);
            if (query == 0) return Result.Failure(new List<string> { "Failed to create Tenant File" });
            return Result.Success(model.CustomAdapt<TenantFileResponse>());
        }
    }
}
