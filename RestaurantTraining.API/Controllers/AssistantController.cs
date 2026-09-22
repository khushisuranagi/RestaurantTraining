using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.Assistant.Commands.ChatAssistant;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/assistant")]
public class AssistantController : ControllerBase
{
    private readonly IMediator _mediator;

    public AssistantController(IMediator mediator)
    {
        _mediator = mediator;
    }

    
    // POST /api/assistant/chat   body: { "messages": [ { fromUser, text }, ... ] }
    [HttpPost("chat")]
    public async Task<IActionResult> Chat([FromBody] ChatAssistantCommand command)
    {
        command.RoleName = User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? string.Empty;

        // Always overwrite UserId from the JWT so a caller can't send someone else's id in the body.
        var idClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        command.UserId = int.TryParse(idClaim, out var userId) ? userId : 0;

        var result = await _mediator.Send(command);

        return Ok(new { reply = result.Reply });
    }
}
