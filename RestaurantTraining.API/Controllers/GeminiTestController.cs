using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Common.Interfaces;

namespace RestaurantTraining.API.Controllers;

// Simple dev/test endpoint to confirm the Gemini connection works.
[Authorize]
[ApiController]
[Route("api/gemini-test")]
public class GeminiTestController : ControllerBase
{
    private readonly IAiTextGenerator _ai;

    public GeminiTestController(IAiTextGenerator ai)
    {
        _ai = ai;
    }

    // GET /api/gemini-test?prompt=Say hello in one word
    [HttpGet]
    public async Task<IActionResult> Test([FromQuery] string prompt)
    {
        if (string.IsNullOrWhiteSpace(prompt))
            return BadRequest(new { Message = "Please type a prompt." });

        var answer = await _ai.GenerateTextAsync(prompt, HttpContext.RequestAborted);

        if (answer is null)
            return StatusCode(502, new
            {
                Message = "Gemini returned no answer. Check the API key and model in configuration."
            });

        return Ok(new { prompt, answer });
    }
}
