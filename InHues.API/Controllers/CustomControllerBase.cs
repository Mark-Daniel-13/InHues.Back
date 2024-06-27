using MediatR;
using Microsoft.AspNetCore.OData.Routing.Controllers;

namespace InHues.API.Controllers
{
    public abstract class CustomControllerBase : ODataController
    {
        private ISender _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
    }
}
