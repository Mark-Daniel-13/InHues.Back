using InHues.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace InHues.Application.Common.Interfaces
{
    public interface IMainContext
    {
        DbSet<ErrorLog> ErrorLogs { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }
        DbSet<CustomerFile> CustomerFiles { get; set; }
        DatabaseFacade GetDatabase { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
