using System;

namespace InHues.Domain.Persistence
{
    public interface ICurrentUserService
    {
        string UserId { get; }
        string Roles { get; }
        string Email { get; }
        bool IsEnabled { get; }
    }
}
