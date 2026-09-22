using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.ContentCreatorPeople.Queries.GetPeople;
using RestaurantTraining.Application.Features.ContentCreatorPeople.Commands.SetLearnerActiveStatus;
using RestaurantTraining.Application.Features.ContentCreatorPeople.Queries.GetLearnerProfile;

namespace RestaurantTraining.API.Controllers;

[Authorize(Roles = "Content Creator")]
[ApiController]
[Route("api/content-creator/people")]
public class ContentCreatorPeopleController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContentCreatorPeopleController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // GET /api/content-creator/people  → users grouped by role, with each role's modules
    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        var result = await _mediator.Send(new GetPeopleQuery());
        return Ok(result);
    }


    // GET /api/content-creator/people/{userId}  → one learner's general info + active status
    [HttpGet("{userId:int}")]
    public async Task<IActionResult> GetLearnerProfile(int userId)
    {
        var result = await _mediator.Send(new GetLearnerProfileQuery { UserId = userId });
        return result is null ? NotFound() : Ok(result);
    }

    // PATCH /api/content-creator/people/{userId}/status  → set a learner active or inactive
    [HttpPatch("{userId:int}/status")]
    public async Task<IActionResult> SetLearnerActiveStatus(int userId, [FromBody] SetLearnerActiveStatusCommand command)
    {
        command.UserId = userId;

        var result = await _mediator.Send(command);

        return result.Success ? Ok(result) : BadRequest(result);
    }


}

