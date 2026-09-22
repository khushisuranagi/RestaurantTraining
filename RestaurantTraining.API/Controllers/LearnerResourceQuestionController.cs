using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.ResourceQuestions.Commands.GetOrGenerateResourceQuestion;
using RestaurantTraining.Application.Features.ResourceQuestions.Commands.SubmitResourceAnswer;
using RestaurantTraining.Application.Features.ResourceSummaries.Commands.GetOrGenerateResourceSummary;

namespace RestaurantTraining.API.Controllers;

[Authorize]
[ApiController]
[Route("api/learner/resources")]
public class LearnerResourceQuestionController : ControllerBase
{
    private readonly IMediator _mediator;

    public LearnerResourceQuestionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    private int? GetCurrentUserId()
    {
        var claim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        return int.TryParse(claim, out var id) ? id : null;
    }

    // GET /api/learner/resources/5/question
    // Returns the stored MCQ, generating + saving it on first request.
    [HttpGet("{resourceId:int}/question")]
    public async Task<IActionResult> GetQuestion(int resourceId)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        var result = await _mediator.Send(new GetOrGenerateResourceQuestionCommand
        {
            ResourceId = resourceId,
            UserId = userId.Value
        });

        if (result.NotAvailable || result.Question is null)
            return NotFound(new { Message = "No question is available for this resource." });

        return Ok(result.Question);
    }

    // POST /api/learner/resources/5/answer   body: { "selectedOptionId": 12 }
    [HttpPost("{resourceId:int}/answer")]
    public async Task<IActionResult> SubmitAnswer(int resourceId, [FromBody] SubmitResourceAnswerCommand command)
    {
        var userId = GetCurrentUserId();
        if (userId is null) return Forbid();

        command.ResourceId = resourceId;
        command.UserId = userId.Value;

        var result = await _mediator.Send(command);

        if (!result.HasQuestion)
            return NotFound(new { Message = "No question is available for this resource." });

        return Ok(new { result.IsCorrect, result.Explanation, result.PointsAwarded, result.TotalPoints });
    }

    // GET /api/learner/resources/5/summary
    // Returns the stored AI summary, generating + saving it on first request.
    [HttpGet("{resourceId:int}/summary")]
    public async Task<IActionResult> GetSummary(int resourceId)
    {
        var result = await _mediator.Send(
            new GetOrGenerateResourceSummaryCommand { ResourceId = resourceId });

        if (result.NotAvailable)
            return NotFound(new { Message = "A summary could not be generated for this resource." });

        return Ok(new { summaryText = result.SummaryText });
    }
}
