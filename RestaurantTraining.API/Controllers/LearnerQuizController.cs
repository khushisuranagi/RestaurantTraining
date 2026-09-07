using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.LearnerQuiz.Commands.SubmitQuiz;
using RestaurantTraining.Application.Features.LearnerQuiz.Queries.GetQuiz;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/learner/quiz")]
public class LearnerQuizController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearnerQuizController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    private string? GetRoleName()
        => User.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

    // GET /api/learner/quiz/5  → questions + options WITHOUT the correct answers
    [HttpGet("{moduleId:int}")]
    public async Task<IActionResult> GetQuiz(int moduleId)
    {
        var roleName = GetRoleName();
        if (string.IsNullOrWhiteSpace(roleName)) return Forbid();

        var result = await _mediator.Send(new GetQuizQuery
        {
            RoleName = roleName,
            ModuleId = moduleId
        });

        return result is null
            ? NotFound(new { Message = "This module is not assigned to your role." })
            : Ok(result);
    }

    // POST /api/learner/quiz/5/submit  → grades, records a QuizAttempt, returns the result
    [HttpPost("{moduleId:int}/submit")]
    public async Task<IActionResult> Submit(int moduleId, [FromBody] SubmitQuizCommand command)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var roleName = GetRoleName();
        if (string.IsNullOrWhiteSpace(roleName)) return Forbid();

        // Fill in the values that come from the route / token (not the body).
        command.UserId = userId.Value;
        command.ModuleId = moduleId;
        command.RoleName = roleName;

        var outcome = await _mediator.Send(command);

        if (outcome.NotAssigned)
            return NotFound(new { Message = "This module is not assigned to your role." });

        if (outcome.NoQuiz)
            return BadRequest(new { Message = "This module has no quiz." });

        return Ok(outcome.Result);
    }
}
