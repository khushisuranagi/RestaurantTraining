using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.Auth.Commands.ChangePassword;
using RestaurantTraining.Application.Features.Auth.Commands.DeleteAccount;
using RestaurantTraining.Application.Features.Auth.Commands.UpdateEmail;   


using System.Security.Claims;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SettingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    // POST /api/settings/change-password
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        command.UserId = userId.Value;

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


    // POST /api/settings/update-email
    [HttpPost("update-email")]
    public async Task<IActionResult> UpdateEmail([FromBody] UpdateEmailCommand command)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        command.UserId = userId.Value;

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


    // POST /api/settings/delete-account
    [HttpPost("delete-account")]
    public async Task<IActionResult> DeleteAccount([FromBody] DeleteAccountCommand command)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        command.UserId = userId.Value;

        var result = await _mediator.Send(command);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return Ok(result);
    }


   
}