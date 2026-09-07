using MediatR;

namespace RestaurantTraining.Application.Features.QuizOptions.Commands.UpdateQuizOption
{
    public class UpdateQuizOptionCommand : IRequest<UpdateQuizOptionResponse>
    {
        public int OptionId { get; set; }

        public int QuestionId { get; set; }

        public string OptionText { get; set; } = string.Empty;

        public bool IsCorrect { get; set; }
    }
}