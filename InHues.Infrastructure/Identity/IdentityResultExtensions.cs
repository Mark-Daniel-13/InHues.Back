﻿using InHues.Domain.BaseModels;
using Microsoft.AspNetCore.Identity;
using System.Linq;

namespace InHues.Infrastructure.Identity
{
    public static class IdentityResultExtensions
    {
        public static Result ToApplicationResult(this IdentityResult result)
        {
            return result.Succeeded
                ? Result.Success()
                : Result.Failure(result.Errors.Select(e => e.Description));
        }
    }
}