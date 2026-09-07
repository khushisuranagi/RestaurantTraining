using MediatR;

namespace RestaurantTraining.Application.Features.QuizOptions.Commands.CreateQuizOption
{
    public class CreateQuizOptionCommand : IRequest<CreateQuizOptionResponse>
    {
        public int QuestionId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}