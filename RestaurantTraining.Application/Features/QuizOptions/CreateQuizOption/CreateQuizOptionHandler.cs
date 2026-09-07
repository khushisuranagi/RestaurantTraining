using MediatR;
using RestaurantTraining.Application.Common.Interfaces;
using RestaurantTraining.Domain.Entities;

namespace RestaurantTraining.Application.Features.QuizOptions.Commands.CreateQuizOption
{
    public class CreateQuizOptionCommandHandler
        : IRequestHandler<CreateQuizOptionCommand, CreateQuizOptionResponse>
    {
        private readonly IQuizOptionRepository _quizOptionRepository;

        public CreateQuizOptionCommandHandler(
            IQuizOptionRepository quizOptionRepository)
        {
            _quizOptionRepository = quizOptionRepository;
        }

        public async Task<CreateQuizOptionResponse> Handle(
            CreateQuizOptionCommand request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.OptionText))
            {
                return new CreateQuizOptionResponse
                {
                    Success = false,
                    Message = "Option text is required."
                };
            }

            var option = new QuizOption
            {
                QuestionId = request.QuestionId,
                OptionText = request.OptionText,
                IsCorrect = request.IsCorrect
            };

            var newOptionId =
                await _quizOptionRepository.AddQuizOptionAsync(
                    option,
                    cancellationToken);

            return new CreateQuizOptionResponse
            {
                Success = true,
                Message = "Quiz option created successfully.",
                OptionId = newOptionId
            };
        }
    }
}