using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTraining.Application.Features.QuizQuestions.Commands.CreateQuizQuestion;
using RestaurantTraining.Application.Features.QuizQuestions.Queries.GetQuizQuestions;
using RestaurantTraining.Application.Features.QuizQuestions.Commands.UpdateQuizQuestion;
using RestaurantTraining.Application.Features.QuizQuestions.Commands.DeleteQuizQuestion;

namespace RestaurantTraining.API.Controllers
{
    [Authorize(Roles = "Content Creator")]
    [ApiController]
    [Route("api/[controller]")]
    public class QuizQuestionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public QuizQuestionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // POST /api/QuizQuestions
        [HttpPost]
        public async Task<IActionResult> CreateQuizQuestion(
            [FromBody] CreateQuizQuestionCommand command)
        {
            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // GET /api/QuizQuestions
        [HttpGet]
        public async Task<IActionResult> GetQuizQuestions()
        {
            var questions = await _mediator.Send(
                new GetQuizQuestionsQuery());

            return Ok(questions);
        }


        // PUT /api/QuizQuestions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateQuizQuestion(
            int id,
            [FromBody] UpdateQuizQuestionCommand command)
        {
            command.QuestionId = id;

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }


        // DELETE /api/QuizQuestions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQuizQuestion(int id)
        {
            var command = new DeleteQuizQuestionCommand
            {
                QuestionId = id
            };

            var response = await _mediator.Send(command);

            if (!response.Success)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

    }
}