using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace InHues.Application.Common.Filter.AuthFilters
{
    public class IsEnabled : IAsyncAuthorizationFilter
    {
        private readonly ILogger<IsEnabled> _logger;

        public IsEnabled(ILogger<IsEnabled> logger)
        {
            _logger = logger;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Endpoint allows anonymous access, skip authorization logic
            if (context.ActionDescriptor.EndpointMetadata.Any(x => x.GetType() == typeof(AllowAnonymousAttribute))) return;

            if (context.HttpContext.User.FindFirst("isEnabled") != null && !Convert.ToBoolean(context.HttpContext.User.FindFirst("isEnabled").Value)) {
                _logger.LogWarning("User is not enabled");
                context.Result = new ForbidResult();
            }
        }
    }
}
