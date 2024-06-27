using InHues.Infrastructure.Identity;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InHues.API.Controllers.V1
{
    [Produces("application/json")]
    public class CustomController : ControllerBase
    {
        private ISender _mediator;
        private ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
        private readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly MainDbContext MainDbContext;
        public CustomController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISender mediator, MainDbContext mainDbContext)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            _mediator = mediator;
            MainDbContext = mainDbContext;
        }

    }
}
