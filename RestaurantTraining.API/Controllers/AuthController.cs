//POST /api/Auth/login this code
using MediatR;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.Auth.Commands.Login;
using RestaurantTraining.Application.Features.Auth.Commands.Register;
using RestaurantTraining.Application.Features.Auth.Queries.GetRegistrationRoles;

namespace RestaurantTraining.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(
            [FromBody] LoginCommand command)   //. This receives the login data
        {
            var response = await _mediator.Send(command);  //sends login command to mediat-> login command handler

            if (!response.Success)
            {
                return Unauthorized(response);
            }

            return Ok(response);
        }

        [HttpGet("registration-roles")]
        public async Task<IActionResult> GetRegistrationRoles()
        {
            return Ok(await _mediator.Send(new GetRegistrationRolesQuery()));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var response = await _mediator.Send(command);
            return response.Success ? Ok(response) : BadRequest(response);
        }
    }
}
