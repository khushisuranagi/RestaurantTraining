using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.ModuleCertificateSettings.Commands.SaveModuleCertificateSetting;
using RestaurantTraining.Application.Features.ModuleCertificateSettings.Queries.GetModuleCertificateSetting;

namespace RestaurantTraining.API.Controllers
{
    [Authorize(Roles = "Content Creator")]
    [ApiController]
    [Route("api/[controller]")]
    public class ModuleCertificateSettingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ModuleCertificateSettingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET /api/ModuleCertificateSettings/5  (returns null if not configured yet)
        [HttpGet("{moduleId}")]
        public async Task<IActionResult> GetByModule(int moduleId)
        {
            var dto = await _mediator.Send(
                new GetModuleCertificateSettingQuery { ModuleId = moduleId });

            return Ok(dto);
        }

        // PUT /api/ModuleCertificateSettings/5  (create or update)
        [HttpPut("{moduleId}")]
        public async Task<IActionResult> Save(
            int moduleId,
            [FromBody] SaveModuleCertificateSettingCommand command)
        {
            command.ModuleId = moduleId;   // trust the id from the URL

            var response = await _mediator.Send(command);

            if (!response.Success)
                return BadRequest(response);

            return Ok(response);
        }
    }
}