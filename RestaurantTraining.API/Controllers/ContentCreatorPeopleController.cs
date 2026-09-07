using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.ContentCreatorPeople.Queries.GetPeople;

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
}
