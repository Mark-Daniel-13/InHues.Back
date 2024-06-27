#nullable enable
using Microsoft.AspNetCore.Identity;
using System;

namespace InHues.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsEnabled { get; set; }
        public bool IsDeleted { get; set; }
        public string InitialPassword { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTimeOffset? PasswordResetExpire { get; set; }
    }
}
