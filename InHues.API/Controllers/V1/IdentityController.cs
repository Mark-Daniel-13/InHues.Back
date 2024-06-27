using InHues.Application.Common.Extensions;
using InHues.Application.Common.Validators;
using InHues.Application.Services.Identity.Commands;
using InHues.Application.Services.Identity.Queries;
using InHues.Domain.DTO.Custom;
using InHues.Domain.DTO.V1.Identity.Request;
using InHues.Domain.DTO.V1.Identity.Response;
using InHues.Infrastructure.Identity;
using InHues.Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace InHues.API.Controllers.V1
{
    [Produces("application/json")]
    [AllowAnonymous]
    public class IdentityController: ControllerBase
    {
        private ISender _mediator;
        private ISender Mediator => _mediator ??= HttpContext.RequestServices.GetService<ISender>();
        private  readonly UserManager<ApplicationUser> UserManager;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly MainDbContext MainDbContext;
        public IdentityController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, ISender mediator, MainDbContext mainDbContext)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            _mediator = mediator;
            MainDbContext = mainDbContext;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<ActionResult> Register([FromBody] RegisterIdentity identity) {
            var validateModel = new RegisterIdentityRequestValidator().Validate(identity);
            if (!validateModel.IsValid) return BadRequest(validateModel.GetErrorMessages());

            var response = await Mediator.Send(identity);
            if (!response.Item1.Succeeded) return BadRequest(response.Item1.Errors);

            return Ok(response.Item2);
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<ActionResult> Login([FromBody] LoginIdentity identity)
        {
            var validate = new LoginIdentityRequestValidator().Validate(identity);

            if (!validate.IsValid) return BadRequest(validate.GetErrorMessages());

            var response = await Mediator.Send(identity);

            //Limit response to generic for security purpose - can be changed base on preference
            if (!response.Item1.Succeeded) return Unauthorized("Username or password is incorrect.");

            return Ok(response.Item2);
        }
        [HttpPost(ApiRoutes.Identity.Refresh)]
        public async Task<ActionResult> Refresh([FromBody] RefreshToken request)
        {
            var response = await Mediator.Send(request);
            if (!response.Item1.Succeeded) return BadRequest(response.Item1.Errors);

            return Ok(response.Item2);
        }
        [HttpPatch(ApiRoutes.Identity.Update)]
        public async Task<ActionResult> Update([FromBody] UpdateUser request)
        {
            var response = await Mediator.Send(request);
            if (!response.Succeeded) return BadRequest(response.Errors);

            return Ok(response);
        }

        [HttpPost(ApiRoutes.Identity.ValidateUsername)]
        public async Task<ActionResult> ValidateUserName([FromBody] ValidateUsername payload)
        {
            if (string.IsNullOrEmpty(payload.Username)) return BadRequest("Username must not be empty!");
            var response = await Mediator.Send(new ValidateUserName() { UserName = payload .Username});
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpGet(ApiRoutes.Identity.FetchRoles)]
        public async Task<IActionResult> FetchRoles()
        {
            return Ok(RoleManager.Roles.Where(r=>r.Name != "Administrator"));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost(ApiRoutes.Identity.AddRoleToUser)]
        public async Task<ActionResult> AddRoleToUser([FromBody] AddRoleToUser payload)
        {
            var response = await Mediator.Send(payload);
            return Ok(response);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost(ApiRoutes.Identity.GetUserRole)]
        public async Task<ActionResult> GetUserRole([FromBody] GetUserRoleRequest payload)
        {
            try
            {
                if (string.IsNullOrEmpty(payload.UserId)) throw new Exception("UserId is missing");

                var user = UserManager.Users.SingleOrDefault(u => u.Id == payload.UserId);
                //remove previous roles - since its one to one relationship atm
                var roles = await UserManager.GetRolesAsync(user);
                return Ok(string.Join(',', roles));
            }
            catch (Exception e) {
                return BadRequest(e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpGet(ApiRoutes.Identity.GetUsers)]
        public async Task<ActionResult> GetUsers([FromBody] GetUsers payload)
        {
            try
            {
                var response = await Mediator.Send(payload);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        [HttpPost(ApiRoutes.Identity.GetUser)]
        public async Task<ActionResult> GetUser([FromBody] GetUserById payload)
        {
            try
            {
                if (string.IsNullOrEmpty(payload.UserId)) throw new Exception("UserId is missing");
                var response = await Mediator.Send(payload);
                return Ok(response);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(ApiRoutes.Identity.ResetPassword)]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPassword payload)
        {
            var response = await Mediator.Send(payload);
            if (!response.Succeeded) return BadRequest(response.Errors);

            //send email
            var serializerOpt = new JsonSerializerOptions()
            {
                NumberHandling = JsonNumberHandling.Strict,
                PropertyNameCaseInsensitive = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            //var body = $"Hi, a request to reset your InHues password has been made.\r\n\r\nIf you did not make this request, simply ignore this email. If you did make this request, please reset your password by clicking this link.\r\n\r\n https://staging.dermtrics.com/ChangePassword/{payload.Email}/{response.Value}";
            //var emailPayload = new SendEmail()
            //{
            //    Subject = "Password Reset",
            //    Recipient = payload.Email,
            //    PlainTextMessage = body,
            //};

            //BYPASS SSL CERT SI  NCE NOTIF SERVICE DOESN'T HAVE SSL YET - REMOVE WHEN SSL UPDATED
            //HttpClientHandler clientHandler = new HttpClientHandler();
            //clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            //var HttpClient = new HttpClient(clientHandler);
            //HttpClient.DefaultRequestHeaders.CacheControl = CacheControlHeaderValue.Parse("no-cache");
            //var httpRequest = new HttpRequestMessage
            //{
            //    Method = new HttpMethod("POST"),
            //    RequestUri = new Uri($"https://notification.dermtrics.com/api/email/sendgrid"),
            //    Content = JsonContent.Create(emailPayload, null, serializerOpt)
            //};
            //httpRequest.Headers.Add("Accept", "*/*");

            //var sendEmail = await HttpClient.SendAsync(httpRequest);
            //if (!sendEmail.IsSuccessStatusCode) return BadRequest("Email sending failed.");

            return Ok();
        }
        [HttpPost(ApiRoutes.Identity.ResetPasswordRequest)]
        public async Task<ActionResult> ResetPasswordRequest([FromBody] ResetPasswordAction payload)
        {
            var response = await Mediator.Send(payload);
            if (!response.Succeeded) return BadRequest(response.Errors);
            return Ok();
        }
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        //[HttpGet(ApiRoutes.Identity.InitializeDb)]
        //public async Task<ActionResult> InitializeDb([FromRoute] string key)
        //{
        //    try
        //    {
        //        if (key != "1nHu3s!!!") return BadRequest();
        //        await MainDbContextSeed.SeedDefaultUserAsync(UserManager, RoleManager, MainDbContext);
        //        return Ok();
        //    }
        //    catch (Exception e)
        //    {
        //        return BadRequest(e.Message);
        //    }
        //}
    }
}
