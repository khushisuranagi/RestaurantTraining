using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.Profile.Commands.UpdateProfile;
using RestaurantTraining.Application.Features.Profile.Queries.GetProfile;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/profile")]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int? GetCurrentUserId() //the token
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    // GET /api/profile  signed-in user's own details
    [HttpGet]
    public async Task<IActionResult> GetProfile()
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(new GetProfileQuery { UserId = userId.Value });

        return result is null ? NotFound() : Ok(result);
    }

    // PUT /api/profile  update the signed-in user's own name & phone
    [HttpPut]



    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        // The user id always comes from the token, never the request body.
        command.UserId = userId.Value;

        var result = await _mediator.Send(command);

        return result.Success ? Ok(result) : BadRequest(result);
    }
}
