using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.QuizQuestions.Commands.CreateQuizQuestion
{
    public class CreateQuizQuestionCommandHandler
        : IRequestHandler<CreateQuizQuestionCommand, CreateQuizQuestionResponse>
    {
        private readonly IQuizQuestionRepository _quizQuestionRepository;

        public CreateQuizQuestionCommandHandler(
            IQuizQuestionRepository quizQuestionRepository)
        {
            _quizQuestionRepository = quizQuestionRepository;
        }

        public async Task<CreateQuizQuestionResponse> Handle(
            CreateQuizQuestionCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.QuestionText))
            {
                return new CreateQuizQuestionResponse
                {
                    Success = false,
                    Message = "Question text is required."
                };
            }

            var question = new QuizQuestion
            {
                ModuleId = request.ModuleId,
                QuestionText = request.QuestionText,
                QuestionType = request.QuestionType,
                ImageUrl = request.ImageUrl,
                Explanation = request.Explanation,
                Marks = request.Marks
            };

            var newQuestionId =
                await _quizQuestionRepository.AddQuizQuestionAsync(
                    question,
                    cancellationToken);

            return new CreateQuizQuestionResponse
            {
                Success = true,
                Message = "Quiz question created successfully.",
                QuestionId = newQuestionId
            };
        }
    }
}